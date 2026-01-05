SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Domain]
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
	[DataType] NVARCHAR(450)
)

GO


ALTER TABLE [Domain] ADD CONSTRAINT [FK_Versioning_Domain] FOREIGN KEY ([VersioningId]) REFERENCES [Domain] ([Id])

GO



GO


CREATE PROCEDURE [DomainDelete]
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
			DELETE [Domain] WHERE [VersioningId] = @Id
			DELETE [Domain] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Domain] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [DomainInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),

	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Domain] ([Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [DomainSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Domain] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [DomainSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [Domain] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [DomainSelectList]
AS
BEGIN
	SELECT * FROM [Domain] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [DomainSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Domain] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [DomainUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),

	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Domain] ([Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Domain] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Domain] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Domain] SET [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


