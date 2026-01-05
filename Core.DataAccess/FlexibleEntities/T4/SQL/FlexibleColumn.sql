SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [FlexibleColumn]
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
	[Name] NVARCHAR(450)
)

GO


ALTER TABLE [FlexibleColumn] ADD CONSTRAINT [FK_Versioning_FlexibleColumn] FOREIGN KEY ([VersioningId]) REFERENCES [FlexibleColumn] ([Id])

GO


CREATE UNIQUE INDEX [AK_FlexibleColumn_Name] ON [FlexibleColumn] ([Name]) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL

GO


CREATE PROCEDURE [FlexibleColumnDelete]
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
			DELETE [FlexibleColumn] WHERE [VersioningId] = @Id
			DELETE [FlexibleColumn] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleColumn] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleColumnInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Name NVARCHAR(450),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [FlexibleColumn] ([Name], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Name, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [FlexibleColumnSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleColumn] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [FlexibleColumnSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [FlexibleColumn] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleColumnSelectList]
AS
BEGIN
	SELECT * FROM [FlexibleColumn] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleColumnSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleColumn] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [FlexibleColumnUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Name NVARCHAR(450),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [FlexibleColumn] ([Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [FlexibleColumn] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [FlexibleColumn] SET [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleColumn] SET [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


