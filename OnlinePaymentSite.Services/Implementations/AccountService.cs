using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Interfaces.Account;
using OnlinePaymentSite.Repository.Interfaces.UserAccount;
using OnlinePaymentSite.Services.DTOs.Account;
using OnlinePaymentSite.Services.Interfaces;

namespace OnlinePaymentSite.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public AccountService(IAccountRepository accountRepository, IUserAccountRepository userAccountRepository)
        {
            _accountRepository = accountRepository;
            _userAccountRepository = userAccountRepository;
        }

        public async Task<GetAccountResponse> GetByIdAsync(int accountId)
        {
            var account = await _accountRepository.RetrieveAsync(accountId);
            if (account.Balance < 0)
                throw new Exception("Invalid account balance detected");

            return new GetAccountResponse
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };
        }

        public async Task<GetAllAccountsResponse> GetAllAsync()
        {
            var accounts = await _accountRepository.RetrieveCollectionAsync(new AccountFilter()).ToListAsync();
            foreach (var account in accounts)
            {
                if (account.Balance < 0)
                    throw new Exception($"Invalid balance for account {account.AccountNumber}");
                if (account.AccountNumber.Length < 10 || !Regex.IsMatch(account.AccountNumber, @"^[a-zA-Z0-9]+$"))
                    throw new Exception($"Invalid account number format for account {account.AccountNumber}");
            }

            return new GetAllAccountsResponse
            {
                Accounts = accounts.Select(a => new AccountInfo
                {
                    AccountId = a.AccountId,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance
                }).ToList(),
                TotalCount = accounts.Count
            };
        }

        public async Task<GetAllAccountsResponse> GetAccountsForUserAsync(int userId)
        {
            var accounts = await _userAccountRepository.GetAccountsForUserAsync(userId).ToListAsync();
            foreach (var account in accounts)
            {
                if (account.Balance < 0)
                    throw new Exception($"Invalid balance for account {account.AccountNumber}");
            }

            return new GetAllAccountsResponse
            {
                Accounts = accounts.Select(a => new AccountInfo
                {
                    AccountId = a.AccountId,
                    AccountNumber = a.AccountNumber,
                    Balance = a.Balance
                }).ToList(),
                TotalCount = accounts.Count
            };
        }
    }
}
