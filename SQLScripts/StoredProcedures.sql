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


CREATE PROCEDURE [PR_CreateService]
AS BEGIN
	INSERT INTO [Service] ([State], [CreationDate])
		VALUES (1, GETDATE())
	SELECT @@IDENTITY
END
GO



CREATE PROCEDURE [PR_GetServices]
AS BEGIN
SELECT * from [Service]
END
GO


CREATE PROCEDURE [PR_EditService](
	@Id int,
	@state bit 
)
AS BEGIN
Update Service SET [State] = @state WHERE IdService = @Id
END
GO



CREATE PROCEDURE [PR_CreateReview]
	@IdClient numeric,
	@IdCheck int,
	@Desc varchar(max),
	@Rating numeric

AS BEGIN
	INSERT INTO [Review] ( [IdClient], [IdCheck], [Date], [Description], [Rating])
		VALUES (@IdClient, @IdCheck, GETDATE(), @Desc, @Rating)	
	SELECT @@IDENTITY
END
GO
CREATE PROCEDURE PR_CreateCheck (
	@clientId Numeric
) AS BEGIN
	INSERT INTO [Check] ([Date], [IdClient]) 
	VALUES (GETDATE(), @clientId);
	SELECT @@IDENTITY;
END
GO

CREATE PROCEDURE PR_InsertCheckDetail (
	@checkId NUMERIC,
	@productName VARCHAR(50),
	@unitaryPrice NUMERIC,
	@quantity NUMERIC
) AS  BEGIN
	INSERT INTO CheckDetail ([IdCheck], [ProdcutName], [UnitaryPrice], [Quantity])
	VALUES (@checkId, @productName, @unitaryPrice, @quantity);
END
GO


CREATE PROCEDURE [PR_GetLike]
	@IdClient numeric,
	@IdService int
AS BEGIN
	Select count(*) from [Likes] where IdCard = @IdClient AND IdService = @IdService
END
GO

CREATE PROCEDURE [PR_CreateLike]
	@IdClient numeric,
	@IdService int
AS BEGIN
	INSERT INTO [Likes] ([IdCard],[IdService])
	VALUES (@IdClient, @IdService)
END
GO

CREATE PROCEDURE [PR_DeleteLike]
	@IdClient numeric,
	@IdService int
AS BEGIN
	DELETE FROM [Likes]  where IdCard = @IdClient AND IdService = @IdService
END
GO

CREATE PROCEDURE [PR_GetReviews](
	@IdClient NUMERIC
)AS BEGIN
	SELECT R.IdReview,R.[Date],R.[Description],R.Rating ,
	STUFF((SELECT ',' + ProdcutName FROM CheckDetail WHERE IdCheck=R.IdCheck FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') Products
	FROM [Review] R
	WHERE R.IdClient = @IdClient
END
GO