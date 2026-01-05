SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [TaskUser]
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
	[TaskId] BIGINT,
	[UserId] BIGINT
)

GO


ALTER TABLE [TaskUser] ADD CONSTRAINT [FK_Versioning_TaskUser] FOREIGN KEY ([VersioningId]) REFERENCES [TaskUser] ([Id])

ALTER TABLE [TaskUser] ADD CONSTRAINT [FK_Task_TaskUser] FOREIGN KEY ([TaskId]) REFERENCES [Task] ([Id]) ON DELETE CASCADE

ALTER TABLE [TaskUser] ADD CONSTRAINT [FK_User_TaskUser] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [TaskUserDelete]
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
			DELETE [TaskUser] WHERE [VersioningId] = @Id
			DELETE [TaskUser] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [TaskUser] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskUserInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@TaskId BIGINT,
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [TaskUser] ([TaskId], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@TaskId, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [TaskUserSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [TaskUser] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [TaskUserSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [TaskUser] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskUserSelectList]
AS
BEGIN
	SELECT * FROM [TaskUser] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskUserSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [TaskUser] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [TaskUserUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
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
		UPDATE [TaskUser] SET [TaskId] = @TaskId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [TaskUser] SET [TaskId] = @TaskId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


