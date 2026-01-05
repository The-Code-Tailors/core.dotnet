SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [UserDomainSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([UserDomain].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id ORDER BY [Id] DESC
END

GO


CREATE PROCEDURE [UserDomainSelectVersionHistoryItem]
(
	@Id BIGINT,
	@ItemId BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([UserDomain].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [UserDomain].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [UserDomain] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id AND [Id] = @ItemId
END

GO


