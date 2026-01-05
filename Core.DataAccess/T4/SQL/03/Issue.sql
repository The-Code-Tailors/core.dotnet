SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [IssueDelete]
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
			DELETE [Issue] WHERE [VersioningId] = @Id
			DELETE [Issue] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Issue] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [IssueUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,

	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Issue] ([Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Issue] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Issue] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Issue] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


