SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Event]
(
	[Id] BIGINT IDENTITY PRIMARY KEY,
	[Guid] UNIQUEIDENTIFIER,
	[InsertDate] DATETIME,
	[InsertUserId] BIGINT,
	[UpdateDate] DATETIME,
	[UpdateUserId] BIGINT,
	[DeleteDate] DATETIME,
	[DeleteUserId] BIGINT,
	[VersioningId] BIGINT,
	-- (DOCUMENT DataAccessXmlSchemaCollection)
	[Data] XML,
	[DataType] NVARCHAR(450),
	[BeginDate] DATETIME,
	[Description] NVARCHAR(MAX),
	[EndDate] DATETIME,
	[MasterEntity] NVARCHAR(450),
	[MasterGuid] UNIQUEIDENTIFIER,
	[MasterId] BIGINT,
	[ReminderDate] DATETIME,
	[Title] NVARCHAR(450)
)

GO


ALTER TABLE [Event] ADD CONSTRAINT [FK_Versioning_Event] FOREIGN KEY ([VersioningId]) REFERENCES [Event] ([Id])

GO



GO


CREATE PROCEDURE [EventDelete]
(
	@Id BIGINT,
	@DeleteDate DATETIME,
	@DeleteUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
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
		UPDATE [Event] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@BeginDate DATETIME,
	@Description NVARCHAR(MAX),
	@EndDate DATETIME,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@ReminderDate DATETIME,
	@Title NVARCHAR(450),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Event] ([BeginDate], [Description], [EndDate], [MasterEntity], [MasterGuid], [MasterId], [ReminderDate], [Title], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@BeginDate, @Description, @EndDate, @MasterEntity, @MasterGuid, @MasterId, @ReminderDate, @Title, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [EventSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Event] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [EventSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [Event] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventSelectList]
AS
BEGIN
	SELECT * FROM [Event] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Event] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [EventUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
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
		UPDATE [Event] SET [BeginDate] = @BeginDate, [Description] = @Description, [EndDate] = @EndDate, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [ReminderDate] = @ReminderDate, [Title] = @Title, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Event] SET [BeginDate] = @BeginDate, [Description] = @Description, [EndDate] = @EndDate, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [ReminderDate] = @ReminderDate, [Title] = @Title, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


