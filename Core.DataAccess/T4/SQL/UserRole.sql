SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [UserRole]
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
	[DomainId] BIGINT,
	[RoleId] BIGINT,
	[UserId] BIGINT
)

GO


ALTER TABLE [UserRole] ADD CONSTRAINT [FK_Versioning_UserRole] FOREIGN KEY ([VersioningId]) REFERENCES [UserRole] ([Id])

ALTER TABLE [UserRole] ADD CONSTRAINT [FK_Domain_UserRole] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([Id])

ALTER TABLE [UserRole] ADD CONSTRAINT [FK_Role_UserRole] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE

ALTER TABLE [UserRole] ADD CONSTRAINT [FK_User_UserRole] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [UserRoleDelete]
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
			DELETE [UserRole] WHERE [VersioningId] = @Id
			DELETE [UserRole] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [UserRole] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserRoleInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@DomainId BIGINT,
	@RoleId BIGINT,
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [UserRole] ([DomainId], [RoleId], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@DomainId, @RoleId, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [UserRoleSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [UserRoleSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserRoleSelectList]
AS
BEGIN
	SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserRoleSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [UserRoleUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@DomainId BIGINT,
	@RoleId BIGINT,
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [UserRole] ([DomainId], [RoleId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [DomainId], [RoleId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [UserRole] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [UserRole] SET [DomainId] = @DomainId, [RoleId] = @RoleId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [UserRole] SET [DomainId] = @DomainId, [RoleId] = @RoleId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


