SET QUOTED_IDENTIFIER ON

GO

ALTER PROCEDURE [RoleFunctionSelectVersionHistory]
(
	@Id BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([RoleFunction].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id ORDER BY [Id] DESC
END

GO


CREATE PROCEDURE [RoleFunctionSelectVersionHistoryItem]
(
	@Id BIGINT,
	@ItemId BIGINT
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([RoleFunction].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id AND [Id] = @ItemId
END

GO


