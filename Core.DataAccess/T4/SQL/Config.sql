SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Config]
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
	[Key] NVARCHAR(450)
)

GO


ALTER TABLE [Config] ADD CONSTRAINT [FK_Versioning_Config] FOREIGN KEY ([VersioningId]) REFERENCES [Config] ([Id])

GO


CREATE UNIQUE INDEX [AK_Config_Key] ON [Config] ([Key]) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL

GO


CREATE PROCEDURE [ConfigDelete]
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
			DELETE [Config] WHERE [VersioningId] = @Id
			DELETE [Config] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Config] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [ConfigInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Key NVARCHAR(450),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Config] ([Key], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Key, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [ConfigSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Config] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [ConfigSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [Config] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [ConfigSelectList]
AS
BEGIN
	SELECT * FROM [Config] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [ConfigSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Config] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [ConfigUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Key NVARCHAR(450),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Config] ([Key], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Key], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Config] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Config] SET [Key] = @Key, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Config] SET [Key] = @Key, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


