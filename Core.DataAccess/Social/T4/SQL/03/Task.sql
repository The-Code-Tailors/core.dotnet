SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [TaskDelete]
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
			DELETE [Task] WHERE [VersioningId] = @Id
			DELETE [Task] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Task] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [TaskUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
	@Complete BIT,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Text NVARCHAR(MAX),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Task] ([Complete], [MasterEntity], [MasterGuid], [MasterId], [Text], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Complete], [MasterEntity], [MasterGuid], [MasterId], [Text], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Task] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Task] SET [Complete] = @Complete, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Text] = @Text, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Task] SET [Complete] = @Complete, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Text] = @Text, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


