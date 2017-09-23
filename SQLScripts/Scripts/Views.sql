USE GranTorismo
GO

CREATE VIEW [Categories]
AS SELECT * FROM Category
GO

CREATE VIEW [ClientDetails] AS
SELECT U.IdCard,U.Username,U.FirstName,U.MiddleName,U.LastName,U.SecondLastName, C.AccountNumber
FROM [dbo].[User] U
LEFT JOIN [dbo].[Client] C ON U.IdCard = C.IdCard
GO