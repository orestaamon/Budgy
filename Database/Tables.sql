-- Lager tabellene 
USE BUDGY;
GO

CREATE TABLE STUDENT
(
    StudentId    int IDENTITY(1,1) PRIMARY KEY,
    Name         nvarchar(100) NOT NULL,
    Email        nvarchar(255) NOT NULL UNIQUE,
    PasswordHash nvarchar(255) NOT NULL,
    CreatedDate  datetime NOT NULL DEFAULT GETDATE()
);

CREATE TABLE CATEGORY
(
    CategoryId int IDENTITY(1,1) PRIMARY KEY,
    StudentId  int NULL FOREIGN KEY REFERENCES STUDENT(StudentId),
    Name       nvarchar(50) NOT NULL,
    Type       nvarchar(10) NOT NULL CHECK (Type IN ('Income', 'Expense')),
    Color      nvarchar(7) NULL
);

CREATE TABLE TRANSACTIONS
(
    TransactionId int IDENTITY(1,1) PRIMARY KEY,
    StudentId     int NOT NULL FOREIGN KEY REFERENCES STUDENT(StudentId),
    CategoryId    int NOT NULL FOREIGN KEY REFERENCES CATEGORY(CategoryId),
    Amount        decimal(10,2) NOT NULL CHECK (Amount > 0),
    [Date]        date NOT NULL,
    Description   nvarchar(255) NULL,
    RepeatMonthly bit NOT NULL DEFAULT 0
);

CREATE TABLE BUDGET
(
    BudgetId    int IDENTITY(1,1) PRIMARY KEY,
    StudentId   int NOT NULL FOREIGN KEY REFERENCES STUDENT(StudentId),
    CategoryId  int NOT NULL FOREIGN KEY REFERENCES CATEGORY(CategoryId),
    [Month]     int NOT NULL CHECK ([Month] BETWEEN 1 AND 12),
    [Year]      int NOT NULL,
    LimitAmount decimal(10,2) NOT NULL
);

CREATE TABLE SAVINGS_GOAL
(
    SavingId   int IDENTITY(1,1) PRIMARY KEY,
    StudentId  int NOT NULL FOREIGN KEY REFERENCES STUDENT(StudentId),
    Title      nvarchar(100) NOT NULL,
    GoalAmount decimal(10,2) NOT NULL,
    SavedSoFar decimal(10,2) NOT NULL DEFAULT 0,
    Deadline   date NULL
);
GO

-- View: transaksjoner sammen med navnet og typen til kategorien (JOIN)
CREATE VIEW TransactionView
AS
SELECT t.TransactionId, t.StudentId, t.CategoryId,
       c.Name AS CategoryName, c.Type,
       t.Amount, t.[Date], t.Description, t.RepeatMonthly
FROM TRANSACTIONS t
INNER JOIN CATEGORY c ON t.CategoryId = c.CategoryId;
GO