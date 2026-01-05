SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [FlexibleEntityColumnInstanceDelete]
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
			DELETE [FlexibleEntityColumnInstance] WHERE [VersioningId] = @Id
			DELETE [FlexibleEntityColumnInstance] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [FlexibleEntityColumnInstance] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [FlexibleEntityColumnInstanceUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
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
		UPDATE [FlexibleEntityColumnInstance] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityInstanceId] = @FlexibleEntityInstanceId, [XmlValue] = @XmlValue, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [FlexibleEntityColumnInstance] SET [FlexibleColumnId] = @FlexibleColumnId, [FlexibleEntityInstanceId] = @FlexibleEntityInstanceId, [XmlValue] = @XmlValue, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


