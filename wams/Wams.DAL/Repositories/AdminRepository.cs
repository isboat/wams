using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Wams.Common.Logging;
using Wams.DataObjects.Accounts;
using Wams.DAL.Interfaces;

namespace Wams.DAL.Repositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        private readonly ILogProvider logProvider;

        public AdminRepository(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }

        public BaseUserInfo Login(string email, string password)
        {
            try
            {
                this.logProvider.Info(string.Format("AdminRepository, Login email:{0}", email));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query =
                        string.Format("select * from administrators where email = '{0}' and password = '{1}' limit 1;",
                            email, password);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        if (record.Read())
                        {
                            return new BaseUserInfo
                            {
                                AccountId = Convert.ToInt32(record["id"].ToString()),
                                EmailAddress = email,
                                FirstName = record["name"].ToString(),
                                LastName = "(Admin)",
                                MembershipType = "Administrator",
                                UserLoginRole = Convert.ToInt32(record["role"].ToString()),
                                CanInvest = false
                            };
                        }

                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AdminRepository, Login email:{0}", email), ex);
                throw;
            }
        }
    }
}
