SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [UserDomain]
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
	[UserId] BIGINT
)

GO


ALTER TABLE [UserDomain] ADD CONSTRAINT [FK_Versioning_UserDomain] FOREIGN KEY ([VersioningId]) REFERENCES [UserDomain] ([Id])

ALTER TABLE [UserDomain] ADD CONSTRAINT [FK_Domain_UserDomain] FOREIGN KEY ([DomainId]) REFERENCES [Domain] ([Id]) ON DELETE CASCADE

ALTER TABLE [UserDomain] ADD CONSTRAINT [FK_User_UserDomain] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE

GO



GO


CREATE PROCEDURE [UserDomainDelete]
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
			DELETE [UserDomain] WHERE [VersioningId] = @Id
			DELETE [UserDomain] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [UserDomain] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserDomainInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@DomainId BIGINT,
	@UserId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [UserDomain] ([DomainId], [UserId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@DomainId, @UserId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [UserDomainSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [UserDomainSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserDomainSelectList]
AS
BEGIN
	SELECT * FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [UserDomainSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [UserDomainUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@DomainId BIGINT,
	@UserId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [UserDomain] ([DomainId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [DomainId], [UserId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [UserDomain] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [UserDomain] SET [DomainId] = @DomainId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [UserDomain] SET [DomainId] = @DomainId, [UserId] = @UserId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


