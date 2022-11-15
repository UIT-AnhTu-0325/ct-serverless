using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ctMaster
{
    public class DatabaseCleanup
    {
        [FunctionName("DatabaseCleanup")]
        public static async Task Run([TimerTrigger("*/2 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            // Get the connection string from app settings and use it to create a connection.
            var str = Environment.GetEnvironmentVariable("sqldb_connection");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = "update [ct-master].dbo.Users\r\nset [Role] = 'GUEST'\r\nwhere UserName = 'tudang'\r\n";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    log.LogInformation($"{rows} rows were updated");
                }
            }
        }
    }
}
