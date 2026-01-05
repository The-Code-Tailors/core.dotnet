namespace com.fabioscagliola.Core.DataAccess
{
    public class Initialization : InitializationBase
    {
        public static void Do()
        {
            if (IsDatabaseVoid())
            {
                Execute(Properties.Resources.Sql000User);
                Execute(Properties.Resources.Sql000Domain);
                Execute(Properties.Resources.Sql000UserDomain);
                Execute(Properties.Resources.Sql000Role);
                Execute(Properties.Resources.Sql000UserRole);
                Execute(Properties.Resources.Sql000RoleFunction);
                Execute(Properties.Resources.Sql000Config);
                Execute(Properties.Resources.Sql000Issue);

                Execute(Properties.Resources.Sql001User);
                Execute(Properties.Resources.Sql001Domain);
                Execute(Properties.Resources.Sql001UserDomain);
                Execute(Properties.Resources.Sql001Role);
                Execute(Properties.Resources.Sql001UserRole);
                Execute(Properties.Resources.Sql001RoleFunction);
                Execute(Properties.Resources.Sql001Config);
                Execute(Properties.Resources.Sql001Issue);

                Execute(Properties.Resources.Sql003User);
                Execute(Properties.Resources.Sql003Domain);
                Execute(Properties.Resources.Sql003UserDomain);
                Execute(Properties.Resources.Sql003Role);
                Execute(Properties.Resources.Sql003UserRole);
                Execute(Properties.Resources.Sql003RoleFunction);
                Execute(Properties.Resources.Sql003Config);
                Execute(Properties.Resources.Sql003Issue);

                Execute(Properties.Resources.Sql000LogEntry);
                Execute(Properties.Resources.Sql001LogEntry);
                Execute(Properties.Resources.Sql003LogEntry);

                Execute(Properties.Resources.Sql004User);
                Execute(Properties.Resources.Sql004Domain);
                Execute(Properties.Resources.Sql004UserDomain);
                Execute(Properties.Resources.Sql004Role);
                Execute(Properties.Resources.Sql004UserRole);
                Execute(Properties.Resources.Sql004RoleFunction);
                Execute(Properties.Resources.Sql004Config);
                Execute(Properties.Resources.Sql004Issue);
                Execute(Properties.Resources.Sql004LogEntry);

                Execute(Properties.Resources.Sql000FlexibleEntity);
                Execute(Properties.Resources.Sql000FlexibleColumn);
                Execute(Properties.Resources.Sql000FlexibleEntityColumn);
                Execute(Properties.Resources.Sql000FlexibleEntityInstance);
                Execute(Properties.Resources.Sql000FlexibleEntityColumnInstance);

                Execute(Properties.Resources.Sql001FlexibleEntity);
                Execute(Properties.Resources.Sql001FlexibleColumn);
                Execute(Properties.Resources.Sql001FlexibleEntityColumn);
                Execute(Properties.Resources.Sql001FlexibleEntityInstance);
                Execute(Properties.Resources.Sql001FlexibleEntityColumnInstance);

                Execute(Properties.Resources.Sql003FlexibleEntity);
                Execute(Properties.Resources.Sql003FlexibleColumn);
                Execute(Properties.Resources.Sql003FlexibleEntityColumn);
                Execute(Properties.Resources.Sql003FlexibleEntityInstance);
                Execute(Properties.Resources.Sql003FlexibleEntityColumnInstance);

                Execute(Properties.Resources.Sql004FlexibleEntity);
                Execute(Properties.Resources.Sql004FlexibleColumn);
                Execute(Properties.Resources.Sql004FlexibleEntityColumn);
                Execute(Properties.Resources.Sql004FlexibleEntityInstance);
                Execute(Properties.Resources.Sql004FlexibleEntityColumnInstance);

                ExecuteOnBlob(Properties.Resources.Sql000Blob);
                ExecuteOnBlob(Properties.Resources.Sql003Blob);

                Execute(Properties.Resources.Sql000Remark);
                Execute(Properties.Resources.Sql000RemarkUser);
                Execute(Properties.Resources.Sql000Task);
                Execute(Properties.Resources.Sql000TaskUser);
                Execute(Properties.Resources.Sql000Notification);
                Execute(Properties.Resources.Sql000Event);
                Execute(Properties.Resources.Sql000EventUser);
                Execute(Properties.Resources.Sql000Follower);

                Execute(Properties.Resources.Sql001Remark);
                Execute(Properties.Resources.Sql001RemarkUser);
                Execute(Properties.Resources.Sql001Task);
                Execute(Properties.Resources.Sql001TaskUser);
                Execute(Properties.Resources.Sql001Notification);
                Execute(Properties.Resources.Sql001Event);
                Execute(Properties.Resources.Sql001EventUser);
                Execute(Properties.Resources.Sql001Follower);

                Execute(Properties.Resources.Sql003Remark);
                Execute(Properties.Resources.Sql003RemarkUser);
                Execute(Properties.Resources.Sql003Task);
                Execute(Properties.Resources.Sql003TaskUser);
                Execute(Properties.Resources.Sql003Notification);
                Execute(Properties.Resources.Sql003Event);
                Execute(Properties.Resources.Sql003EventUser);
                Execute(Properties.Resources.Sql003Follower);

                Execute(Properties.Resources.Sql004Remark);
                Execute(Properties.Resources.Sql004RemarkUser);
                Execute(Properties.Resources.Sql004Task);
                Execute(Properties.Resources.Sql004TaskUser);
                Execute(Properties.Resources.Sql004Notification);
                Execute(Properties.Resources.Sql004Event);
                Execute(Properties.Resources.Sql004EventUser);
                Execute(Properties.Resources.Sql004Follower);

                Config config = new Config();
                config.Key = "DataAccessSchema";
                config.Value = "4";
                config.Update(Milieu.SystemMilieu);
            }
        }

    }
}

