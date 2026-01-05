SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [RoleFunction]
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
	[Function] BIGINT,
	[RoleId] BIGINT
)

GO


ALTER TABLE [RoleFunction] ADD CONSTRAINT [FK_Versioning_RoleFunction] FOREIGN KEY ([VersioningId]) REFERENCES [RoleFunction] ([Id])

ALTER TABLE [RoleFunction] ADD CONSTRAINT [FK_Role_RoleFunction] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [RoleFunctionDelete]
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
			DELETE [RoleFunction] WHERE [VersioningId] = @Id
			DELETE [RoleFunction] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [RoleFunction] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RoleFunctionInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Function BIGINT,
	@RoleId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [RoleFunction] ([Function], [RoleId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Function, @RoleId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [RoleFunctionSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [RoleFunctionSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RoleFunctionSelectList]
AS
BEGIN
	SELECT * FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [RoleFunctionSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [RoleFunctionUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Function BIGINT,
	@RoleId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [RoleFunction] ([Function], [RoleId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Function], [RoleId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [RoleFunction] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [RoleFunction] SET [Function] = @Function, [RoleId] = @RoleId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [RoleFunction] SET [Function] = @Function, [RoleId] = @RoleId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


