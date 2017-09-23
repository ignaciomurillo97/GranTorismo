USE GranTorismo
GO
	
CREATE PROCEDURE [PR_CreateUser] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL,
	@responseMessage NVARCHAR(250) OUTPUT
) AS BEGIN
	SET NOCOUNT ON
	DECLARE @Salt UNIQUEIDENTIFIER = NEWID()
	BEGIN TRY
		INSERT INTO [User] ([IdCard], [Username], [PasswordHash], [FirstName], [MiddleName],[LastName], [SecondLastName], [Salt])
		VALUES(@IdCard, @Username, HASHBYTES('SHA2_512', @Password + CAST(@salt AS NVARCHAR(36))), @FirstName, @MiddleName, @LastName, @SecondLastName, @Salt)
		SET @responseMessage = 'Success'
    END TRY
    BEGIN CATCH
        SET @responseMessage = ERROR_MESSAGE()
    END CATCH
END
GO


CREATE PROCEDURE [PR_CreateClient] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL,
	@AccountNumber NUMERIC(20),
	@responseMessage NVARCHAR(250) OUTPUT
) AS BEGIN
	EXEC PR_CreateUser @IdCard, @Username, @Password, @FirstName, @MiddleName, @LastName, @SecondLastName, @responseMessage OUTPUT
	IF @responseMessage = 'Success'
	BEGIN
		BEGIN TRY
			INSERT INTO [Client](IdCard, [AccountNumber]) VALUES (@IdCard, @AccountNumber)
		SET @responseMessage = 'Success'
		END TRY
		BEGIN CATCH
			SET @responseMessage = ERROR_MESSAGE() 
		END CATCH
	END
END
GO

CREATE PROCEDURE [PR_UserLogin](
    @Username NVARCHAR(254),
    @Password NVARCHAR(50),
    @responseMessage NVARCHAR(250)='' OUTPUT,
	@IdCard NUMERIC(20) = NULL OUTPUT
)AS BEGIN
    SET NOCOUNT ON
    DECLARE @userID INT
	SET @IdCard=(SELECT [IdCard] FROM [User] WHERE Username=@Username AND PasswordHash=HASHBYTES('SHA2_512', @Password+CAST(Salt AS NVARCHAR(36))))
	IF(@IdCard IS NULL)
		SET @responseMessage='Incorrect password'
	ELSE 
		SET @responseMessage='User successfully logged in'
END
GO

CREATE PROCEDURE [PR_ClientLogin](
    @Username NVARCHAR(254),
    @Password NVARCHAR(50),
    @responseMessage NVARCHAR(250)='' OUTPUT,
	@IdCard NUMERIC(20) = NULL OUTPUT
)AS BEGIN
	IF EXISTS (SELECT TOP 1 C.[IdCard] FROM [Client] C INNER JOIN [User] U ON U.[IdCard] = C.[IdCard] WHERE U.Username=@Username)
		EXEC PR_UserLogin @Username, @Password, @responseMessage OUTPUT, @IdCard OUTPUT
	ELSE
		SET @responseMessage = 'Invalid Login'
END
GO