SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [IssueSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([Issue].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [Issue] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id ORDER BY [Id] DESC
END

GO


CREATE PROCEDURE [IssueSelectVersionHistoryItem]
(
	@Id BIGINT,
	@ItemId BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([Issue].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [Issue].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [Issue] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id AND [Id] = @ItemId
END

GO


