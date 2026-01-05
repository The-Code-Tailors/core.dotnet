SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [FlexibleEntityInstanceSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([FlexibleEntityInstance].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id ORDER BY [Id] DESC
END

GO


CREATE PROCEDURE [FlexibleEntityInstanceSelectVersionHistoryItem]
(
	@Id BIGINT,
	@ItemId BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([FlexibleEntityInstance].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [FlexibleEntityInstance].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [FlexibleEntityInstance] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id AND [Id] = @ItemId
END

GO


