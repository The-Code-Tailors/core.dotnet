SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Blob]
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
	-- (DOCUMENT FabManXmlSchemaCollection)
	[Data] XML,
    [DataType] NVARCHAR(450),
	[Content] VARBINARY(MAX),
	[ContentLength] BIGINT,
	[ContentType] NVARCHAR(450),
	[IsCompressed] BIT,
	[MasterEntity] NVARCHAR(450),
	[MasterGuid] UNIQUEIDENTIFIER,
	[MasterId] BIGINT,
	[Name] NVARCHAR(450)
)

GO


ALTER TABLE [Blob] ADD CONSTRAINT [FK_Versioning_Blob] FOREIGN KEY ([VersioningId]) REFERENCES [Blob] ([Id])

GO


CREATE UNIQUE INDEX [AK_Blob_Guid] ON [Blob] ([Guid]) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL

GO


CREATE PROCEDURE [BlobDelete]
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
			DELETE [Blob] WHERE [VersioningId] = @Id
			DELETE [Blob] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Blob] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [BlobInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Content VARBINARY(MAX),
	@ContentLength BIGINT,
	@ContentType NVARCHAR(450),
	@IsCompressed BIT,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Name NVARCHAR(450),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Blob] ([Content], [ContentLength], [ContentType], [IsCompressed], [MasterEntity], [MasterGuid], [MasterId], [Name], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Content, @ContentLength, @ContentType, @IsCompressed, @MasterEntity, @MasterGuid, @MasterId, @Name, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [BlobSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT
	[Id],
	[Guid],
	[InsertDate],
	[InsertUserId],
	[UpdateDate],
	[UpdateUserId],
	[DeleteDate],
	[DeleteUserId],
	[VersioningId],
	[Data],
	[DataType],
	--[Content],
	[ContentLength],
	[ContentType],
	[IsCompressed],
	[MasterEntity],
	[MasterGuid],
	[MasterId],
	[Name]
	FROM [Blob] WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [BlobSelectList]
AS
BEGIN
	SELECT
	[Id],
	[Guid],
	[InsertDate],
	[InsertUserId],
	[UpdateDate],
	[UpdateUserId],
	[DeleteDate],
	[DeleteUserId],
	[VersioningId],
	[Data],
	[DataType],
	--[Content],
	[ContentLength],
	[ContentType],
	[IsCompressed],
	[MasterEntity],
	[MasterGuid],
	[MasterId],
	[Name]
	FROM [Blob] WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [BlobSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT
	[Id],
	[Guid],
	[InsertDate],
	[InsertUserId],
	[UpdateDate],
	[UpdateUserId],
	[DeleteDate],
	[DeleteUserId],
	[VersioningId],
	[Data],
	[DataType],
	--[Content],
	[ContentLength],
	[ContentType],
	[IsCompressed],
	[MasterEntity],
	[MasterGuid],
	[MasterId],
	[Name]
	FROM [Blob] WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [BlobUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Content VARBINARY(MAX),
	@ContentLength BIGINT,
	@ContentType NVARCHAR(450),
	@IsCompressed BIT,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Name NVARCHAR(450),
	@DoVersioning BIT
)
AS
BEGIN

	SET @DoVersioning = 0  -- Deactivate versioning 

	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Blob] ([Content], [ContentLength], [ContentType], [IsCompressed], [MasterEntity], [MasterGuid], [MasterId], [Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Content], [ContentLength], [ContentType], [IsCompressed], [MasterEntity], [MasterGuid], [MasterId], [Name], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Blob] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Blob] SET [Content] = @Content, [ContentLength] = @ContentLength, [ContentType] = @ContentType, [IsCompressed] = @IsCompressed, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Blob] SET [Content] = @Content, [ContentLength] = @ContentLength, [ContentType] = @ContentType, [IsCompressed] = @IsCompressed, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


CREATE PROCEDURE [BlobSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT
	[Id],
	[Guid],
	[InsertDate],
	[InsertUserId],
	[UpdateDate],
	[UpdateUserId],
	[DeleteDate],
	[DeleteUserId],
	[VersioningId],
	[Data],
	[DataType],
	--[Content],
	[ContentLength],
	[ContentType],
	[IsCompressed],
	[MasterEntity],
	[MasterGuid],
	[MasterId],
	[Name]
	FROM [Blob] WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [BlobSelectContent]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT [Content] FROM [Blob] WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [BlobSelectListByMasterGuid]
(
	@MasterGuid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT
	[Id],
	[Guid],
	[InsertDate],
	[InsertUserId],
	[UpdateDate],
	[UpdateUserId],
	[DeleteDate],
	[DeleteUserId],
	[VersioningId],
	[Data],
	[DataType],
	--[Content],
	[ContentLength],
	[ContentType],
	[IsCompressed],
	[MasterEntity],
	[MasterGuid],
	[MasterId],
	[Name]
	FROM [Blob] WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


