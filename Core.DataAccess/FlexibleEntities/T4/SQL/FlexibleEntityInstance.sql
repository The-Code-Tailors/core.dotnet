SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [FlexibleEntityInstance]
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
	[FlexibleEntityId] BIGINT
)

GO


ALTER TABLE [FlexibleEntityInstance] ADD CONSTRAINT [FK_Versioning_FlexibleEntityInstance] FOREIGN KEY ([VersioningId]) REFERENCES [FlexibleEntityInstance] ([Id])

ALTER TABLE [FlexibleEntityInstance] ADD CONSTRAINT [FK_FlexibleEntity_FlexibleEntityInstance] FOREIGN KEY ([FlexibleEntityId]) REFERENCES [FlexibleEntity] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [FlexibleEntityInstanceDelete]
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
			DELETE [FlexibleEntityInstance] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityInstance] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityInstance] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleEntityId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [FlexibleEntityInstance] ([FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@FlexibleEntityId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceSelectList]
AS
BEGIN
	SELECT * FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleEntityId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [FlexibleEntityInstance] ([FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [FlexibleEntityInstance] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [FlexibleEntityInstance] SET [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityInstance] SET [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


