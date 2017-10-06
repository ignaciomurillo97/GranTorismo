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

CREATE PROCEDURE [PR_CreateOwner] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL,
	@responseMessage NVARCHAR(250) OUTPUT
) AS BEGIN
	EXEC PR_CreateUser @IdCard, @Username, @Password, @FirstName, @MiddleName, @LastName, @SecondLastName, @responseMessage OUTPUT
	IF @responseMessage = 'Success'
	BEGIN
		BEGIN TRY
			INSERT INTO [Owner](IdCard) VALUES (@IdCard)
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

CREATE PROCEDURE [PR_UsersLogin](
    @Username NVARCHAR(254),
    @Password NVARCHAR(50),
    @responseMessage NVARCHAR(250)='' OUTPUT,
	@IdCard NUMERIC(20) = NULL OUTPUT,
	@Rol VARCHAR(20) = NULL OUTPUT
)AS BEGIN
	SET @Rol = CASE 
		WHEN EXISTS(SELECT TOP 1 C.[IdCard] FROM [Client] C INNER JOIN [User] U ON U.[IdCard] = C.[IdCard] WHERE U.Username=@Username)
			THEN 'Client' 
		WHEN EXISTS(SELECT TOP 1 O.[IdCard] FROM [Owner] O INNER JOIN [User] U ON U.[IdCard] = O.[IdCard] WHERE U.Username=@Username)
			THEN 'Owner'
		WHEN EXISTS(SELECT TOP 1 A.[IdCard] FROM [Admin] A INNER JOIN [User] U ON U.[IdCard] = A.[IdCard] WHERE U.Username=@Username)
			THEN 'Admin'
		ELSE NULL END

	IF (@Rol IS NULL)
		SET @responseMessage = 'Invalid Login'
	ELSE
		EXEC PR_UserLogin @Username, @Password, @responseMessage OUTPUT, @IdCard OUTPUT
END
GO

CREATE PROCEDURE [PR_GetUser](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT * FROM [ClientDetails] WHERE IdCard = @IdCard
END
GO

CREATE PROCEDURE [PR_GetClient](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT CD.* FROM [ClientDetails] CD
	INNER JOIN [Client] C ON CD.IdCard = C.IdCard
	WHERE CD.IdCard = @IdCard
END
GO

alter PROCEDURE [PR_GetUserByUsername](
	@Username VARCHAR(25)
)AS BEGIN
	SELECT IdCard FROM [ClientDetails] WHERE Username = @Username
END
GO

CREATE PROCEDURE [PR_CreateService]
AS BEGIN
	INSERT INTO [Service] ([State], [CreationDate])
		VALUES (1, GETDATE()
	SELECT @@IDENTITY
END
GO

CREATE PROCEDURE [PR_EditCategory](
	@Name VARCHAR(MAX),
	@Id INT
)AS BEGIN
	UPDATE Category SET [Name] = @Name WHERE IdCategory = @Id
END
GO

CREATE PROCEDURE [PR_CreateCategory] (
	@Name VARCHAR(MAX)
)AS BEGIN
	INSERT INTO [Category] VALUES(@Name)
END
GO


CREATE PROCEDURE [PR_DeleteCategory] (
	@Id INT
)AS BEGIN
	DELETE [Category] WHERE IdCategory = @Id
END
GO

CREATE PROCEDURE [PR_DeleteAdmin](
	@id INT 
) 
AS BEGIN
	DELETE [Admin] WHERE IdCard = @id
	DELETE [User] WHERE IdCard = @id
END
GO

CREATE PROCEDURE [PR_CreateAdmin] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL,
	@responseMessage NVARCHAR(250) OUTPUT
) AS BEGIN
	EXEC PR_CreateUser @IdCard, @Username, @Password, @FirstName, @MiddleName, @LastName, @SecondLastName, @responseMessage OUTPUT
	IF @responseMessage = 'Success'
	BEGIN
		BEGIN TRY
			INSERT INTO [Admin](IdCard) VALUES (@IdCard)
		SET @responseMessage = 'Success'
		END TRY
		BEGIN CATCH
			SET @responseMessage = ERROR_MESSAGE() 
		END CATCH
	END
END
GO

CREATE PROCEDURE [PR_GetFollowing](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT U.* 
	FROM [Follower] F
	INNER JOIN [ClientDetails] U
		ON F.IdFriend = U.IdCard
	WHERE F.IdCard = @IdCard
END
GO

CREATE PROCEDURE [PR_GetFollowers](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT U.* 
	FROM [Follower] F
	INNER JOIN [ClientDetails] U
		ON F.IdFriend = @IdCard
	WHERE F.IdCard = U.IdCard
END
GO


CREATE PROCEDURE [PR_Follow](
	@IdCard NUMERIC(20),
	@IdFriend NUMERIC(20)
)AS BEGIN
	INSERT INTO [Follower] VALUES (@IdCard, @IdFriend)
END
GO



CREATE PROCEDURE [PR_Unfollow](
	@IdCard NUMERIC(20),
	@IdFriend NUMERIC(20)
)AS BEGIN
	DELETE FROM [Follower]
	WHERE IdCard = @IdCard AND IdFriend = @IdFriend
END
GO



CREATE PROCEDURE [PR_GetDistricts]
AS BEGIN

SELECT 
	IdDistrict AS [IdDistrict],
	CONCAT(P.Name, ' - ', C.Name, ' - ', D.Name) AS [Name]
FROM [Province] P
	INNER JOIN [Canton] C ON C.IdProvince = P.IdProvince
	INNER JOIN [District] D ON D.IdCanton = C.IdCanton

END
GO	

CREATE PROCEDURE [PR_AddToPackage](
	@IdPackage INT,
	@IdService INT
)AS BEGIN
	INSERT INTO [Package] VALUES(@IdPackage, @IdService)
END
GO


CREATE PROCEDURE [PR_GetPackage](
	@IdPackage INT
)AS BEGIN
	SELECT IdService FROM [Package] WHERE IdPackage = @IdPackage
END
GO

alter PROCEDURE [PR_DeletePackage](
	@IdPackage INT
)AS BEGIN
	DELETE [Package] WHERE IdPackage = @IdPackage
END
GO


CREATE PROCEDURE [PR_AddPCategory](
	@IdService INT,
	@IdCategory INT
)AS BEGIN
	INSERT INTO [ProductCategory] VALUES(@IdService, @IdCategory)
END
GO


CREATE PROCEDURE [PR_GetPCategory](
	@IdService INT
)AS BEGIN
	SELECT IdCategory FROM [ProductCategory] WHERE IdService = @IdService
END
GO

CREATE PROCEDURE [PR_DeletePCategory](
	@IdService INT
)AS BEGIN
	DELETE [ProductCategory] WHERE IdService = @IdService
END
GO
