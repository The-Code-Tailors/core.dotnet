SET QUOTED_IDENTIFIER ON

GO


ALTER PROCEDURE [RoleFunctionSelect]
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
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [Id] = @Id AND [DeleteDate] IS NULL
END

GO


ALTER PROCEDURE [RoleFunctionSelectByGuid]
(
	@Guid UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([RoleFunction].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [Guid] = @Guid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

GO


ALTER PROCEDURE [RoleFunctionSelectList]
AS
BEGIN
	SELECT *
	, (SELECT [dbo].[BlobCount]([RoleFunction].[Guid])) [SocialCount1]
	, (SELECT COUNT(*) FROM [Event] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount2]
	, (SELECT COUNT(*) FROM [Remark] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount3]
	, (SELECT COUNT(*) FROM [Task] WHERE [MasterGuid] = [RoleFunction].[Guid] AND [DeleteDate] IS NULL AND [VersioningId] IS NULL) [SocialCount4]
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [DeleteDate] IS NULL AND [VersioningId] IS NULL
END

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
	FROM [RoleFunction] WITH (READUNCOMMITTED) WHERE [VersioningId] = @Id
END

GO


