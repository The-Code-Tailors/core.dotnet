SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [User]
(
	[Id] BIGINT IDENTITY PRIMARY KEY,
	[Guid] UNIQUEIDENTIFIER,
	[InsertDate] DATETIME,
	[InsertUserId] BIGINT,
	[UpdateDate] DATETIME,
	[UpdateUserId] BIGINT,
	[DeleteDate] DATETIME,
	[DeleteUserId] BIGINT,
	[VersioningId] BIGINT,
	-- (DOCUMENT DataAccessXmlSchemaCollection)
	[Data] XML,
	[DataType] NVARCHAR(450),
	[PHash] NVARCHAR(450),
	[UName] NVARCHAR(450)
)

GO


ALTER TABLE [User] ADD CONSTRAINT [FK_Versioning_User] FOREIGN KEY ([VersioningId]) REFERENCES [User] ([Id])

GO


CREATE UNIQUE INDEX [AK_User_UName] ON [User] ([UName]) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL

GO


CREATE PROCEDURE [UserDelete]
(
	@Id BIGINT,
	@DeleteDate DATETIME,
	@DeleteUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
    @Permanently BIT
)
AS
BEGIN
	IF @Permanently = 1
		BEGIN
			DELETE [User] WHERE [VersioningId] = @Id
			DELETE [User] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [User] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@PHash NVARCHAR(450),
	@UName NVARCHAR(450),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [User] ([PHash], [UName], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@PHash, @UName, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [UserSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [User] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [UserSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [User] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserSelectList]
AS
BEGIN
	SELECT * FROM [User] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [User] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [UserUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@PHash NVARCHAR(450),
	@UName NVARCHAR(450),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [User] ([PHash], [UName], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [PHash], [UName], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [User] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [User] SET [PHash] = @PHash, [UName] = @UName, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [User] SET [PHash] = @PHash, [UName] = @UName, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


