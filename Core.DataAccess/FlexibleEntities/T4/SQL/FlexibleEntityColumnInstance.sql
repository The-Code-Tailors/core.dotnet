SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [FlexibleEntityColumnInstance]
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
	[FlexibleEntityInstanceId] BIGINT,
	[XmlValue] XML
)

GO


ALTER TABLE [FlexibleEntityColumnInstance] ADD CONSTRAINT [FK_Versioning_FlexibleEntityColumnInstance] FOREIGN KEY ([VersioningId]) REFERENCES [FlexibleEntityColumnInstance] ([Id])

ALTER TABLE [FlexibleEntityColumnInstance] ADD CONSTRAINT [FK_FlexibleColumn_FlexibleEntityColumnInstance] FOREIGN KEY ([FlexibleColumnId]) REFERENCES [FlexibleColumn] ([Id]) ON DELETE CASCADE

ALTER TABLE [FlexibleEntityColumnInstance] ADD CONSTRAINT [FK_FlexibleEntityInstance_FlexibleEntityColumnInstance] FOREIGN KEY ([FlexibleEntityInstanceId]) REFERENCES [FlexibleEntityInstance] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceDelete]
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
			DELETE [FlexibleEntityColumnInstance] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityColumnInstance] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityColumnInstance] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleColumnId BIGINT,
	@FlexibleEntityInstanceId BIGINT,
	@XmlValue XML,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [FlexibleEntityColumnInstance] ([FlexibleColumnId], [FlexibleEntityInstanceId], [XmlValue], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@FlexibleColumnId, @FlexibleEntityInstanceId, @XmlValue, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumnInstance] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumnInstance] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceSelectList]
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumnInstance] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [FlexibleEntityColumnInstance] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [FlexibleEntityColumnInstanceUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@FlexibleColumnId BIGINT,
	@FlexibleEntityInstanceId BIGINT,
	@XmlValue XML,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [FlexibleEntityColumnInstance] ([FlexibleColumnId], [FlexibleEntityInstanceId], [XmlValue], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [FlexibleColumnId], [FlexibleEntityInstanceId], [XmlValue], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [FlexibleEntityColumnInstance] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [FlexibleEntityColumnInstance] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityInstanceId] = @FlexibleEntityInstanceId, [XmlValue] = @XmlValue, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityColumnInstance] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityInstanceId] = @FlexibleEntityInstanceId, [XmlValue] = @XmlValue, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


