using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Models;
using OnlinePaymentSite.Repository.Helpers;
using OnlinePaymentSite.Repository.Interfaces.Account;
using OnlinePaymentSite.Repository.Interfaces.Payment;
using OnlinePaymentSite.Repository;
using OnlinePaymentSite.Services.DTOs.Payment;
using OnlinePaymentSite.Services.Interfaces;
using OnlinePaymentSite.Repository.Interfaces.UserAccount;
using OnlinePaymentSite.Repository.Implementations;

namespace OnlinePaymentSite.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IAccountRepository accountRepository,
            IUserAccountRepository userAccountRepository)
        {
            _paymentRepository = paymentRepository;
            _accountRepository = accountRepository;
            _userAccountRepository = userAccountRepository;
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            if (request.Amount <= 0)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Amount must be greater than 0" };
            if (string.IsNullOrEmpty(request.Reason) || request.Reason.Length > 32)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Reason must be between 1 and 32 characters" };
            if (request.FromAccountId == request.ToAccountId)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Cannot transfer to the same account" };

            var fromAccount = await _accountRepository.RetrieveAsync(request.FromAccountId);
            if (fromAccount == null)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Source account not found" };
            if (fromAccount.Balance < request.Amount)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Insufficient balance" };

            var toAccount = await _accountRepository.RetrieveAsync(request.ToAccountId);
            if (toAccount == null)
                return new CreatePaymentResponse { Success = false, ErrorMessage = "Destination account not found" };

            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                var newFromBalance = fromAccount.Balance - request.Amount;
                var newToBalance = toAccount.Balance + request.Amount;

                if (newFromBalance < 0)
                    throw new ValidationException("Resulting balance cannot be negative");

                var fromUpdate = new AccountUpdate { Balance = new SqlDecimal(newFromBalance) };
                var toUpdate = new AccountUpdate { Balance = new SqlDecimal(newToBalance) };

                using (var fromCommand = new UpdateCommand(connection, "Accounts", "AccountId", request.FromAccountId))
                {
                    fromCommand.AddSetClause("Balance", fromUpdate.Balance);
                    fromCommand.SqlCommand.Transaction = transaction;
                    await fromCommand.ExecuteNonQueryAsync();
                }

                using (var toCommand = new UpdateCommand(connection, "Accounts", "AccountId", request.ToAccountId))
                {
                    toCommand.AddSetClause("Balance", toUpdate.Balance);
                    toCommand.SqlCommand.Transaction = transaction;
                    await toCommand.ExecuteNonQueryAsync();
                }

                var payment = new Payment
                {
                    FromAccountId = request.FromAccountId,
                    ToAccountId = request.ToAccountId,
                    Amount = request.Amount,
                    Reason = request.Reason,
                    PaymentDate = DateTime.Now
                };

                var paymentId = await _paymentRepository.CreateAsync(payment);

                transaction.Commit();

                payment.PaymentId = paymentId;
                var paymentInfo = await MapToPaymentInfoAsync(payment);

                return new CreatePaymentResponse
                {
                    PaymentId = paymentId,
                    FromAccountId = paymentInfo.FromAccountId,
                    FromAccountNumber = paymentInfo.FromAccountNumber,
                    ToAccountId = paymentInfo.ToAccountId,
                    ToAccountNumber = paymentInfo.ToAccountNumber,
                    Amount = paymentInfo.Amount,
                    Reason = paymentInfo.Reason,
                    PaymentDate = paymentInfo.PaymentDate,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new CreatePaymentResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<GetAllPaymentsResponse> GetPaymentsForAccountAsync(int accountId)
        {
            var filter = new PaymentFilter
            {
                FromAccountId = new SqlInt32(accountId),
                ToAccountId = new SqlInt32(accountId)
            };

            var payments = await _paymentRepository.RetrieveCollectionAsync(filter).ToListAsync();
            var paymentInfos = new List<PaymentInfo>();

            foreach (var payment in payments)
            {
                if (payment.Amount <= 0)
                    throw new Exception($"Invalid payment amount for payment ID {payment.PaymentId}");
                if (string.IsNullOrEmpty(payment.Reason) || payment.Reason.Length > 32)
                    throw new Exception($"Invalid reason length for payment ID {payment.PaymentId}");

                paymentInfos.Add(await MapToPaymentInfoAsync(payment));
            }

            return new GetAllPaymentsResponse
            {
                Payments = paymentInfos,
                TotalCount = paymentInfos.Count
            };
        }

        private async Task<PaymentInfo> MapToPaymentInfoAsync(Payment payment)
        {
            var fromAccount = await _accountRepository.RetrieveAsync(payment.FromAccountId);
            var toAccount = await _accountRepository.RetrieveAsync(payment.ToAccountId);

            return new PaymentInfo
            {
                PaymentId = payment.PaymentId,
                FromAccountId = payment.FromAccountId,
                FromAccountNumber = fromAccount.AccountNumber,
                ToAccountId = payment.ToAccountId,
                ToAccountNumber = toAccount.AccountNumber,
                Amount = payment.Amount,
                Reason = payment.Reason,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}
