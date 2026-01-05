SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [RemarkUser]
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
	[RemarkId] BIGINT,
	[UserId] BIGINT
)

GO


ALTER TABLE [RemarkUser] ADD CONSTRAINT [FK_Versioning_RemarkUser] FOREIGN KEY ([VersioningId]) REFERENCES [RemarkUser] ([Id])

ALTER TABLE [RemarkUser] ADD CONSTRAINT [FK_Remark_RemarkUser] FOREIGN KEY ([RemarkId]) REFERENCES [Remark] ([Id]) ON DELETE CASCADE

ALTER TABLE [RemarkUser] ADD CONSTRAINT [FK_User_RemarkUser] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [RemarkUserDelete]
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
			DELETE [RemarkUser] WHERE [VersioningId] = @Id
			DELETE [RemarkUser] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [RemarkUser] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RemarkUserInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@RemarkId BIGINT,
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [RemarkUser] ([RemarkId], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@RemarkId, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [RemarkUserSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [RemarkUser] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [RemarkUserSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [RemarkUser] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RemarkUserSelectList]
AS
BEGIN
	SELECT * FROM [RemarkUser] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RemarkUserSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [RemarkUser] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [RemarkUserUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@RemarkId BIGINT,
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [RemarkUser] ([RemarkId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [RemarkId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [RemarkUser] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [RemarkUser] SET [RemarkId] = @RemarkId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [RemarkUser] SET [RemarkId] = @RemarkId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


