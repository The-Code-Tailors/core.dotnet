SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [BlobDelete]
(
	@Id BIGINT,
	@DeleteDate DATETIME,
	@DeleteUserId BIGINT,
	@Data XML,
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
		UPDATE [Blob] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [BlobUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
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
		UPDATE [Blob] SET [Content] = @Content, [ContentLength] = @ContentLength, [ContentType] = @ContentType, [IsCompressed] = @IsCompressed, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Blob] SET [Content] = @Content, [ContentLength] = @ContentLength, [ContentType] = @ContentType, [IsCompressed] = @IsCompressed, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Name] = @Name, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


