using System;
using System.Configuration;
using System.Linq;

namespace com.fabioscagliola.Core.Data
{
    public abstract class Controller
    {
        public static ControllerType Type => Settings.Instance.Entity.ControllerType;

        public abstract void Delete();

        public abstract void Update();

        //public abstract ... Select(int id);

    }
}

