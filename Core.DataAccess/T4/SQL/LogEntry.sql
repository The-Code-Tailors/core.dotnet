SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [LogEntry]
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
	[MasterId] BIGINT
)

GO


ALTER TABLE [LogEntry] ADD CONSTRAINT [FK_Versioning_LogEntry] FOREIGN KEY ([VersioningId]) REFERENCES [LogEntry] ([Id])

GO



GO


CREATE PROCEDURE [LogEntryDelete]
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
			DELETE [LogEntry] WHERE [VersioningId] = @Id
			DELETE [LogEntry] WHERE [Id] = @Id
		END
	ELSE
		UPDATE [LogEntry] SET [DeleteDate] = @DeleteDate, [DeleteUserId] = @DeleteUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [LogEntryInsert]
(
	@Guid UNIQUEIDENTIFIER,
	@InsertDate DATETIME,
	@InsertUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@Id BIGINT OUTPUT
)
AS
BEGIN
	INSERT [LogEntry] ([MasterEntity], [MasterGuid], [MasterId], [Guid], [InsertDate], [InsertUserId], [Data], [DataType]) VALUES (@MasterEntity, @MasterGuid, @MasterId, @Guid, @InsertDate, @InsertUserId, @Data, @DataType)
	SELECT @Id = SCOPE_IDENTITY()
END

GO


CREATE PROCEDURE [LogEntrySelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


CREATE PROCEDURE [LogEntrySelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [LogEntrySelectList]
AS
BEGIN
	SELECT * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


CREATE PROCEDURE [LogEntrySelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


CREATE PROCEDURE [LogEntryUpdate]
(
	@Id BIGINT,
	@UpdateDate DATETIME,
	@UpdateUserId BIGINT,
	@Data XML,
    @DataType NVARCHAR(450),
	@MasterEntity NVARCHAR(450),
	@MasterGuid UNIQUEIDENTIFIER,
	@MasterId BIGINT,
	@DoVersioning BIT
)
AS
BEGIN
	IF @DoVersioning = 1
	BEGIN
		BEGIN TRANSACTION
		INSERT [LogEntry] ([MasterEntity], [MasterGuid], [MasterId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], [VersioningId], [Data], [DataType])
			SELECT [MasterEntity], [MasterGuid], [MasterId], [Guid], [InsertDate], [InsertUserId], [UpdateDate], [UpdateUserId], @Id, [Data], [DataType] FROM [LogEntry] WHERE [Id] = @Id AND [VersioningId] IS NULL
		UPDATE [LogEntry] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
		COMMIT
	END
	ELSE
	BEGIN
		UPDATE [LogEntry] SET [MasterEntity] = @MasterEntity, [MasterGuid] = @MasterGuid, [MasterId] = @MasterId, [UpdateDate] = @UpdateDate, [UpdateUserId] = @UpdateUserId, [Data] = @Data, [DataType] = @DataType WHERE [Id] = @Id
	END
END

GO


