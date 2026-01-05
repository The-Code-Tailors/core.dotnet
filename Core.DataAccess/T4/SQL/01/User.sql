SET QUOTED_IDENTIFIER ON

GO


ALTER PROCEDURE [UserSelect]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([User].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [User] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


ALTER PROCEDURE [UserSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([User].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [User] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [UserSelectList]
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([User].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [User] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [UserSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([User].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [User].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [User] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


