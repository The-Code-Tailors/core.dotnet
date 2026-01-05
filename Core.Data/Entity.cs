using System;
using System.ComponentModel;

namespace com.fabioscagliola.Core.Data
{
    public abstract class Entity<IdType>
    {
        [Browsable(false)]
        public IdType Id { get; set; }

        protected virtual Controller GetController(ControllerConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public virtual ControllerConfiguration GetControllerConfiguration()
        {
            ControllerConfiguration configuration = null;

            switch (Controller.Type)
            {
                case ControllerType.SqlLocalDB:
                    {
                        configuration = SqlLocalDBControllerConfiguration.GetDefault();
                        break;
                    }
                case ControllerType.SqlIntegratedSecurity:
                    {
                        configuration = SqlIntegratedSecurityControllerConfiguration.GetDefault();
                        break;
                    }
                case ControllerType.Sql:
                default:
                    {
                        configuration = SqlControllerConfiguration.GetDefault();
                        break;
                    }
            }

            return configuration;
        }

        public virtual Controller GetDefaultController()
        {
            ControllerConfiguration configuration = GetControllerConfiguration();
            return GetController(configuration);
        }

        public virtual void Delete()
        {
            Controller controller = GetDefaultController();
            controller.Delete();
        }

        public virtual void Update()
        {
            Controller controller = GetDefaultController();
            controller.Update();
        }

    }
}

