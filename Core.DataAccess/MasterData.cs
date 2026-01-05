using System;

namespace com.fabioscagliola.Core.DataAccess
{
    public class MasterData
    {
        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }

        public MasterData() { }

        public MasterData(DataAccessEntity master)
        {
            MasterEntity = master.GetType().FullName;
            MasterGuid = master.Guid;
            MasterId = master.Id;
        }

    }
}

