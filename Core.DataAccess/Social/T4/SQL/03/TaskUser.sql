SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [TaskUserDelete]
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
			DELETE [TaskUser] WHERE [VersioningId] = @Id
			DELETE [TaskUser] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [TaskUser] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [TaskUserUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
	@TaskId BIGINT,
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [TaskUser] ([TaskId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [TaskId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [TaskUser] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [TaskUser] SET [TaskId] = @TaskId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [TaskUser] SET [TaskId] = @TaskId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


