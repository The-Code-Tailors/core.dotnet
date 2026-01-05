SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [EventDelete]
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
			DELETE [Event] WHERE [VersioningId] = @Id
			DELETE [Event] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Event] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [EventUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
	@BeginDate DATETIME,
	@Description NVARCHAR(MAX),
	@EndDate DATETIME,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@ReminderDate DATETIME,
	@Title NVARCHAR(450),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Event] ([BeginDate], [Description], [EndDate], [MasterEntity], [MasterGuid], [MasterId], [ReminderDate], [Title], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [BeginDate], [Description], [EndDate], [MasterEntity], [MasterGuid], [MasterId], [ReminderDate], [Title], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Event] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Event] SET [BeginDate] = @BeginDate, [Description] = @Description, [EndDate] = @EndDate, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [ReminderDate] = @ReminderDate, [Title] = @Title, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Event] SET [BeginDate] = @BeginDate, [Description] = @Description, [EndDate] = @EndDate, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [ReminderDate] = @ReminderDate, [Title] = @Title, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data WHERE [Id] = @Id
	END
END

GO


