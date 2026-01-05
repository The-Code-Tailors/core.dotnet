SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [FlexibleEntity]
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


ALTER TABLE [FlexibleEntity] ADD CONSTRAINT [FK_Versioning_FlexibleEntity] FOREIGN KEY ([VersioningId]) REFERENCES [FlexibleEntity] ([Id])

GO


CREATE UNIQUE INDEX [AK_FlexibleEntity_Name] ON [FlexibleEntity] ([Name]) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL

GO


CREATE PROCEDURE [FlexibleEntityDelete]
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
			DELETE [FlexibleEntity] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntity] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntity] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityInsert]
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
	INSERT [FlexibleEntity] ([Name], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Name, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [FlexibleEntitySelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntity] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntitySelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [FlexibleEntity] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntitySelectList]
AS
BEGIN
	SELECT * FROM [FlexibleEntity] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntitySelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntity] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [FlexibleEntityUpdate]
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
		INSERT [FlexibleEntity] ([Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [FlexibleEntity] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [FlexibleEntity] SET [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntity] SET [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


