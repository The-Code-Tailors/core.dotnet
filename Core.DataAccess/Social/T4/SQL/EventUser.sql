SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [EventUser]
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
	[EventId] BIGINT,
	[UserId] BIGINT
)

GO


ALTER TABLE [EventUser] ADD CONSTRAINT [FK_Versioning_EventUser] FOREIGN KEY ([VersioningId]) REFERENCES [EventUser] ([Id])

ALTER TABLE [EventUser] ADD CONSTRAINT [FK_Event_EventUser] FOREIGN KEY ([EventId]) REFERENCES [Event] ([Id]) ON DELETE CASCADE

ALTER TABLE [EventUser] ADD CONSTRAINT [FK_User_EventUser] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [EventUserDelete]
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
			DELETE [EventUser] WHERE [VersioningId] = @Id
			DELETE [EventUser] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [EventUser] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventUserInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@EventId BIGINT,
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [EventUser] ([EventId], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@EventId, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [EventUserSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [EventUser] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [EventUserSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [EventUser] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventUserSelectList]
AS
BEGIN
	SELECT * FROM [EventUser] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [EventUserSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [EventUser] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [EventUserUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@EventId BIGINT,
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [EventUser] ([EventId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [EventId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [EventUser] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [EventUser] SET [EventId] = @EventId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [EventUser] SET [EventId] = @EventId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


