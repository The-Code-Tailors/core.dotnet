SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Notification]
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
	[MasterEntity] NVARCHAR(450),
	[MasterGuid] UNIQUEIDENTIFIER,
	[MasterId] BIGINT,
	[Read] BIT,
	[Text] NVARCHAR(MAX),
	[UserId] BIGINT
)

GO


ALTER TABLE [Notification] ADD CONSTRAINT [FK_Versioning_Notification] FOREIGN KEY ([VersioningId]) REFERENCES [Notification] ([Id])

GO



GO


CREATE PROCEDURE [NotificationDelete]
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
			DELETE [Notification] WHERE [VersioningId] = @Id
			DELETE [Notification] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Notification] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [NotificationInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Read BIT,
	@Text NVARCHAR(MAX),
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Notification] ([MasterEntity], [MasterGuid], [MasterId], [Read], [Text], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@MasterEntity, @MasterGuid, @MasterId, @Read, @Text, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [NotificationSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Notification] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [NotificationSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [Notification] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [NotificationSelectList]
AS
BEGIN
	SELECT * FROM [Notification] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [NotificationSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Notification] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [NotificationUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Read BIT,
	@Text NVARCHAR(MAX),
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Notification] ([MasterEntity], [MasterGuid], [MasterId], [Read], [Text], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [MasterEntity], [MasterGuid], [MasterId], [Read], [Text], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Notification] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Notification] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Read] = @Read, [Text] = @Text, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Notification] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Read] = @Read, [Text] = @Text, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


