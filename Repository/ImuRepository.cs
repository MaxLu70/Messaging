using MessageContracts.IMU.Model;
using MessageContracts.IMU.Requests;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Repository
{
    public class ImuRepository
    {
        public async Task<IEnumerable<Contribuente>> FetchContribuenti(IContribuentiRequest request)
        {
            var ret = new List<Contribuente>();
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "gestione.portal.kibernetes.net,8433";
            builder.Encrypt = false;
            builder.TrustServerCertificate = true;
            builder.InitialCatalog = "ramostabile";
            builder.PersistSecurityInfo = true;
            builder.UserID = "KSDPortal";
            builder.Password = "Demo!16$Aru21";
            using (SqlConnection connection = new(builder.ConnectionString))
            {
                SqlCommand command = new("SELECT CodiceFiscale, Denominazione, Natura FROM UUE.Contribuenti WHERE (@CodiceFiscale IS NULL OR CodiceFiscale=@CodiceFiscale) AND (@Denominazione IS NULL OR Denominazione=@Denominazione)", connection);
                command.Parameters.AddWithValue("@CodiceFiscale", !string.IsNullOrWhiteSpace(request.CodiceFiscale) ? request.CodiceFiscale : DBNull.Value);
                command.Parameters.AddWithValue("@Denominazione", !string.IsNullOrWhiteSpace(request.Denominazione) ? request.Denominazione : DBNull.Value);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        int r = 0;
                        while (await reader.ReadAsync())
                        {
                            Contribuente contribuente = new();
                            contribuente.CodiceFiscale = !(await reader.IsDBNullAsync(0)) ? reader.GetString(0) : string.Empty;
                            contribuente.Denominazione = !(await reader.IsDBNullAsync(1)) ? reader.GetString(1) : string.Empty;
                            contribuente.Natura = !(await reader.IsDBNullAsync(2)) ? reader.GetInt16(2) : 0;
                            ret.Add(contribuente);
                            r++;
                            Console.WriteLine($"riga {r}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return ret;
        }
    }
}
