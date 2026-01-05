namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnInstance
    {
        public static FlexibleEntityColumnInstance Select(Milieu milieu, long flexibleEntityInstanceId, long flexibleColumnId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)new FlexibleEntityColumnInstance().GetDefaultController();
            return controller.Select(flexibleEntityInstanceId, flexibleColumnId);
        }

    }
}

