create database OnlinePaymentSite

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,

);
GO

CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    AccountNumber NVARCHAR(20) NOT NULL UNIQUE,
    Balance DECIMAL(18,2) NOT NULL DEFAULT 0.00,

);
GO

CREATE TABLE UserAccounts (
    UserId INT NOT NULL,
    AccountId INT NOT NULL,
    PRIMARY KEY (UserId, AccountId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId) ON DELETE CASCADE
);
GO

CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    FromAccountId INT NOT NULL,
    ToAccountId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Reason NVARCHAR(32) NOT NULL,
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (FromAccountId) REFERENCES Accounts(AccountId),
    FOREIGN KEY (ToAccountId) REFERENCES Accounts(AccountId)
);
GO

INSERT INTO Users (Username, Password, FullName)
VALUES 
    ('georgi.georgiev', 'D5A7F659DDE7A2F2A9E0B6BFB3DDBF7F6A9A8E7E8F9C0A1B2C3D4E5F60718290', 'Georgi Georgiev'),
    ('rositsa.hristova', '8E9F7A8B9C0D1E2F3A4B5C6D7E8F901234567890ABCDEF1234567890ABCDEF12', 'Rositsa Hristova'),
    ('maria.dimitrova', '7B8C9D0E1F2A3B4C5D6E7F8091A2B3C4D5E6F7A8B9C0D1E2F3A4B5C6D7E8F901', 'Maria Dimitrova'); 
GO

INSERT INTO Accounts (AccountNumber, Balance)
VALUES 
    ('ACC1001', 1000.00),
    ('ACC1002', 500.00),
    ('ACC1003', 2000.00),
    ('ACC1004', 750.00),
    ('ACC1005', 300.00);
GO

INSERT INTO UserAccounts (UserId, AccountId)
VALUES 
    (1, 1), -- user1 owns ACC1001
    (1, 2), -- user1 owns ACC1002
    (2, 3), -- user2 owns ACC1003
    (2, 4), -- user2 owns ACC1004
    (3, 5); -- user3 owns ACC1005
GO

INSERT INTO Payments (FromAccountId, ToAccountId, Amount, Reason, PaymentDate)
VALUES 
    (1, 3, 100.00, 'Bill payment', '2025-06-07 10:00:00'), -- ACC1001 to ACC1003
    (2, 4, 50.00, 'Gift', '2025-06-07 15:30:00'), -- ACC1002 to ACC1004
    (3, 5, 200.00, 'Loan repayment', '2025-06-08 09:00:00'), -- ACC1003 to ACC1005
    (4, 1, 75.00, 'Service fee', '2025-06-08 14:00:00'); -- ACC1004 to ACC1001
GO