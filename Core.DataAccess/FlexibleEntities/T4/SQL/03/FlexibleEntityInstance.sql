SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [FlexibleEntityInstanceDelete]
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
			DELETE [FlexibleEntityInstance] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityInstance] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityInstance] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [FlexibleEntityInstanceUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
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
		UPDATE [FlexibleEntityInstance] SET [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityInstance] SET [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


