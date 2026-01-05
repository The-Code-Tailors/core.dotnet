SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [FlexibleEntityColumn]
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
	[FlexibleColumnId] BIGINT,
	[FlexibleEntityId] BIGINT
)

GO


ALTER TABLE [FlexibleEntityColumn] ADD CONSTRAINT [FK_Versioning_FlexibleEntityColumn] FOREIGN KEY ([VersioningId]) REFERENCES [FlexibleEntityColumn] ([Id])

ALTER TABLE [FlexibleEntityColumn] ADD CONSTRAINT [FK_FlexibleColumn_FlexibleEntityColumn] FOREIGN KEY ([FlexibleColumnId]) REFERENCES [FlexibleColumn] ([Id]) ON DELETE CASCADE

ALTER TABLE [FlexibleEntityColumn] ADD CONSTRAINT [FK_FlexibleEntity_FlexibleEntityColumn] FOREIGN KEY ([FlexibleEntityId]) REFERENCES [FlexibleEntity] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [FlexibleEntityColumnDelete]
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
			DELETE [FlexibleEntityColumn] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityColumn] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityColumn] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleColumnId BIGINT,
	@FlexibleEntityId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [FlexibleEntityColumn] ([FlexibleColumnId], [FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@FlexibleColumnId, @FlexibleEntityId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [FlexibleEntityColumnSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumn] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumn] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnSelectList]
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumn] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumn] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [FlexibleEntityColumnUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleColumnId BIGINT,
	@FlexibleEntityId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [FlexibleEntityColumn] ([FlexibleColumnId], [FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [FlexibleColumnId], [FlexibleEntityId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [FlexibleEntityColumn] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [FlexibleEntityColumn] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityColumn] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


