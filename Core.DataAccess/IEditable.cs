using System;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public interface IEditable
    {
        // Properties 
        long Id { get; }
        Guid Guid { get; }

        // Methods 
        void Update(Milieu milieu, SqlTransaction transaction);
        void Delete(Milieu milieu, SqlTransaction transaction, bool permanently);
    }

}

