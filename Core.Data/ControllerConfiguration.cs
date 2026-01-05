using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.Data
{
    public abstract class ControllerConfiguration
    {
        public abstract SqlConnection GetConnection();

    }
}

