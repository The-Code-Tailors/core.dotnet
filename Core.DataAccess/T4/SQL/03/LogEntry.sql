SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [LogEntryDelete]
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
			DELETE [LogEntry] WHERE [VersioningId] = @Id
			DELETE [LogEntry] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [LogEntry] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [LogEntryUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [LogEntry] ([MasterEntity], [MasterGuid], [MasterId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [MasterEntity], [MasterGuid], [MasterId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [LogEntry] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [LogEntry] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [LogEntry] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


