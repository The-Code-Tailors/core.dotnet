SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [FlexibleEntityColumnDelete]
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
			DELETE [FlexibleEntityColumn] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityColumn] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityColumn] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [FlexibleEntityColumnUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
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
		UPDATE [FlexibleEntityColumn] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityColumn] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityId] = @FlexibleEntityId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


