
-- En midlertidig testbruker 
USE BUDGY;
GO

-- brukes til innlogging er laget
INSERT INTO STUDENT (Name, Email, PasswordHash)
VALUES (N'Testbruker', N'test@budgy.no', N'midlertidig');

-- Faste kategorier for alle 
INSERT INTO CATEGORY (StudentId, Name, Type, Color) VALUES
(NULL, N'Husleie',         'Expense', '#E76F51'),
(NULL, N'Mat',             'Expense', '#E9C46A'),
(NULL, N'Transport',       'Expense', '#2A9D8F'),
(NULL, N'Fritid',          'Expense', '#5B2DB5'),
(NULL, N'Studiemateriell', 'Expense', '#8D99AE'),
(NULL, N'Sport',           'Expense', '#F4A261'),
(NULL, N'Annet',           'Expense', '#B0B0B0'),
(NULL, N'Studielån',       'Income',  '#2E7D32'),
(NULL, N'Jobb',            'Income',  '#43A047'),
(NULL, N'Annen inntekt',   'Income',  '#81C784');

-- Resultatet
SELECT * FROM STUDENT;
SELECT * FROM CATEGORY;