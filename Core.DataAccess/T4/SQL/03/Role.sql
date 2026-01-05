SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [RoleDelete]
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
			DELETE [Role] WHERE [VersioningId] = @Id
			DELETE [Role] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Role] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [RoleUpdate]
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
		INSERT [Role] ([Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Role] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Role] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Role] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


