SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [UserRoleSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([UserRole].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [UserRole] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id ORDER BY [Id] DESC
END

GO


CREATE PROCEDURE [UserRoleSelectVersionHistoryItem]
(
	@Id BIGINT,
	@ItemId BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([UserRole].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [UserRole].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [UserRole] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id AND [Id] = @ItemId
END

GO


