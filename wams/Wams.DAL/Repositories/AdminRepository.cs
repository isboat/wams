using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Wams.Common.Logging;
using Wams.DataObjects.Accounts;
using Wams.DataObjects.Admin;
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
                        string.Format("select * from administrators where email = '{0}' and password = '{1}' and deleted = 0 limit 1;",
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
                                FirstName = record["firstname"].ToString(),
                                LastName = record["lastname"] + "(Admin)",
                                MembershipType = "Administrator",
                                LoginRole = Convert.ToInt32(record["role"].ToString()),
                                CanInvest = false,
                                IsAdmin = true
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

        public int CreateAdmin(CreateAdmin admin)
        {
            try
            {
                this.logProvider.Info(string.Format("AdminRepository, CreateAdmin email:{0}, firstname: {1}, lastname: {2}, role: {3}",
                    admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query =
                        string.Format("insert into administrators(email, firstname, lastname, role, password) values('{0}', '{1}', '{2}', {3}, '{4}')",
                            admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role, admin.Password);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteNonQuery();

                        return record;

                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AdminRepository, CreateAdmin email:{0}, firstname: {1}, lastname: {2}, role: {3}",
                    admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role), ex);
                throw;
            }
        }

        public List<BaseUserInfo> GetAllAdmins()
        {
            try
            {
                this.logProvider.Info("AdminRepository, GetAllAdmins");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from administrators where deleted = 0;";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var users = new List<BaseUserInfo>();

                        while (record.Read())
                        {
                            users.Add(new BaseUserInfo
                            {
                                AccountId = Convert.ToInt32(record["id"].ToString()),
                                EmailAddress = record["email"].ToString(),
                                FirstName = record["firstname"].ToString(),
                                LastName = record["lastname"].ToString(),
                                LoginRole = Convert.ToInt32(record["role"].ToString()),
                            });
                        }

                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error("AdminRepository, GetAllAdmins", ex);
                throw;
            }
        }

        public AdminUserInfo GetAdmin(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AdminRepository, GetAdmins id: {0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from administrators where id = {0} and deleted = 0 limit 1;", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        if (record.Read())
                        {
                            return new AdminUserInfo
                            {
                                AccountId = Convert.ToInt32(record["id"].ToString()),
                                EmailAddress = record["email"].ToString(),
                                FirstName = record["firstname"].ToString(),
                                LastName = record["lastname"].ToString(),
                                LoginRole = Convert.ToInt32(record["role"].ToString()),
                                Password = record["password"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AdminRepository, GetAdmins id: {0}", id), ex);
                throw;
            }
        }

        public int EditAdmin(EditAdmin admin)
        {
            try
            {
                this.logProvider.Info(string.Format("AdminRepository, EditAdmin email:{0}, firstname: {1}, lastname: {2}, role: {3}, id: {4}",
                    admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role, admin.Id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query =
                        string.Format("update administrators set email='{0}', firstname='{1}', lastname='{2}', role={3}, password='{4}' where id = {5}",
                            admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role, admin.Password, admin.Id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteNonQuery();

                        return record;

                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AdminRepository, EditAdmin email:{0}, firstname: {1}, lastname: {2}, role: {3} id: {4}",
                    admin.EmailAddress, admin.FirstName, admin.LastName, admin.Role, admin.Id), ex);
                throw;
            }
        }

        public int DeleteAdmin(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AdminRepository, DeleteAdmin id: {0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("update administrators set deleted = 1 where id = {0}", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteNonQuery();

                        return record;

                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AdminRepository, DeleteAdmin id: {0}", id), ex);
                throw;
            }
        }
    }
}
