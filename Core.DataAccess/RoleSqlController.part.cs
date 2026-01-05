using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleSqlController
    {
        public Role Select(string name)
        {
            SqlCommand command = new SqlCommand() { CommandText = "select * from [Role] where [Role].Data.query('/Role/Name').value('.', 'varchar(max)') = @Name and [Role].DeleteDate is null and [Role].VersioningId is null", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@Name", name);
            return Select(command);
        }

    }
}

