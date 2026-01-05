SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [Task]
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
	[Complete] BIT,
	[MasterEntity] NVARCHAR(450),
	[MasterGuid] UNIQUEIDENTIFIER,
	[MasterId] BIGINT,
	[Text] NVARCHAR(MAX)
)

GO


ALTER TABLE [Task] ADD CONSTRAINT [FK_Versioning_Task] FOREIGN KEY ([VersioningId]) REFERENCES [Task] ([Id])

GO



GO


CREATE PROCEDURE [TaskDelete]
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
			DELETE [Task] WHERE [VersioningId] = @Id
			DELETE [Task] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [Task] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Complete BIT,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Text NVARCHAR(MAX),
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [Task] ([Complete], [MasterEntity], [MasterGuid], [MasterId], [Text], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@Complete, @MasterEntity, @MasterGuid, @MasterId, @Text, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [TaskSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Task] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [TaskSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [Task] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskSelectList]
AS
BEGIN
	SELECT * FROM [Task] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [TaskSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [Task] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [TaskUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@Complete BIT,
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Text NVARCHAR(MAX),
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [Task] ([Complete], [MasterEntity], [MasterGuid], [MasterId], [Text], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [Complete], [MasterEntity], [MasterGuid], [MasterId], [Text], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [Task] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [Task] SET [Complete] = @Complete, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Text] = @Text, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [Task] SET [Complete] = @Complete, [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [Text] = @Text, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


