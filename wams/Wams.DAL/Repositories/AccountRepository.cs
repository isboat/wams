using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.DAL.Repositories
{
    using System.ComponentModel;
    using System.Data;

    using MySql.Data.MySqlClient;

    using Wams.DataObjects.Accounts;
    using Wams.DAL.Interfaces;
    using Wams.DataObjects;

    public class AccountRepository :BaseRepository, IAccountRepository
    {
        public BaseUserInfo Login(string email, string password)
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("login", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_email_in", email);
                        cmd.Parameters["@p_email_in"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_passwd_in", password);
                        cmd.Parameters["@p_passwd_in"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@id_out", MySqlDbType.Int32);
                        cmd.Parameters["@id_out"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new MySqlParameter("@fn_out", MySqlDbType.VarChar));
                        cmd.Parameters["@fn_out"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new MySqlParameter("@ln_out", MySqlDbType.VarChar));
                        cmd.Parameters["@ln_out"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new MySqlParameter("@memtype_out", MySqlDbType.VarChar));
                        cmd.Parameters["@memtype_out"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new MySqlParameter("@loginrole_out", MySqlDbType.Int32));
                        cmd.Parameters["@loginrole_out"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        var id = cmd.Parameters["@id_out"].Value.ToString();
                        if (!string.IsNullOrEmpty(id))
                        {
                            return new BaseUserInfo
                                   {
                                       AccountId = Convert.ToInt32(id),
                                       EmailAddress = email,
                                       FirstName = cmd.Parameters["@fn_out"].Value.ToString(),
                                       LastName = cmd.Parameters["@ln_out"].Value.ToString(),
                                       MembershipType = cmd.Parameters["@memtype_out"].Value.ToString(),
                                       UserLoginRole = Convert.ToInt32(cmd.Parameters["@loginrole_out"].Value.ToString())
                                   };
                        }

                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserAccount GetAccountInfo(int accountid)
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("getaccountinfo", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_memberid_in", accountid);
                        cmd.Parameters["@p_memberid_in"].Direction = ParameterDirection.Input;

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new UserAccount
                                   {
                                       AccountId = accountid,
                                       DateOfBirth = Convert.ToDateTime(record["date_of_birth"].ToString()),
                                       EmailAddress = record["email_address"].ToString(),
                                       FirstName = record["first_name"].ToString(),
                                       LastName = record["last_name"].ToString(),
                                       Gender = record["gender"].ToString(),
                                       Biography = record["biography"].ToString(),
                                       Telephone = record["phone_number"].ToString(),
                                       EmergencyTel = record["emergency_contact_number"].ToString(),
                                       MembershipType = record["membershiptype"].ToString(),
                                       UserLoginRole = Convert.ToInt32(record["loginrole"].ToString())
                                   };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserAccount> GetAllUserAccounts()
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from member_information";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<UserAccount>();
                        while (record.Read())
                        {
                            records.Add(new UserAccount
                            {
                                AccountId = Convert.ToInt32(record["member_id"].ToString()),
                                DateOfBirth = Convert.ToDateTime(record["date_of_birth"].ToString()),
                                EmailAddress = record["email_address"].ToString(),
                                FirstName = record["first_name"].ToString(),
                                LastName = record["last_name"].ToString(),
                                Gender = record["gender"].ToString(),
                                Biography = record["biography"].ToString(),
                                Telephone = record["phone_number"].ToString(),
                                EmergencyTel = record["emergency_contact_number"].ToString(),
                                MembershipType = record["membershiptype"].ToString()
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetPasscode(string accountkey, string passcodeKey)
        {
            throw new NotImplementedException();
        }

        public int CreateApplication(
            string firstname, 
            string lastname, 
            string gender, 
            DateTime dob, 
            string email, 
            string password, 
            string membershipType,
            int userLoginRole)
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("createaccount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_fn", firstname);
                        cmd.Parameters["@p_fn"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_ln", lastname);
                        cmd.Parameters["@p_ln"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_gender", gender);
                        cmd.Parameters["@p_gender"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_dob", dob);
                        cmd.Parameters["@p_dob"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_email", email);
                        cmd.Parameters["@p_email"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_password", password);
                        cmd.Parameters["@p_password"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_memtype", membershipType);
                        cmd.Parameters["@p_memtype"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_loginrole", userLoginRole);
                        cmd.Parameters["@p_loginrole"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@out_id", MySqlDbType.Int32);
                        cmd.Parameters["@out_id"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        var id = cmd.Parameters["@out_id"].Value.ToString();

                        if (!string.IsNullOrEmpty(id))
                        {
                            return Convert.ToInt32(id);
                        }

                        return -1;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int UpdateAccountInfo(UserAccount userAccount)
        {
            if (userAccount == null)
            {
                return -1;
            }

            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updateaccount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_id", userAccount.AccountId);
                        cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_fn", userAccount.FirstName);
                        cmd.Parameters["@p_fn"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_ln", userAccount.LastName);
                        cmd.Parameters["@p_ln"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_gender", userAccount.Gender);
                        cmd.Parameters["@p_gender"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_dob", userAccount.DateOfBirth);
                        cmd.Parameters["@p_dob"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_email", userAccount.EmailAddress);
                        cmd.Parameters["@p_email"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_tel", userAccount.Telephone);
                        cmd.Parameters["@p_tel"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_emergency", userAccount.EmergencyTel);
                        cmd.Parameters["@p_emergency"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_bio", userAccount.Biography);
                        cmd.Parameters["@p_bio"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_memtype", userAccount.MembershipType);
                        cmd.Parameters["@p_memtype"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_loginrole", userAccount.UserLoginRole);
                        cmd.Parameters["@p_loginrole"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public int ChangePassword(string accountKey, string newPassword)
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updatepassword", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_id", accountKey);
                        cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_pwd", newPassword);
                        cmd.Parameters["@p_pwd"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetPassword(string accountKey)
        {
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("getpassword", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_id", accountKey);
                        cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        return record.Read() ? record["password"].ToString() : string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Private methods
        
        private UserAccount CreateUserAccount(IDataRecord row)
        {
            return new UserAccount
                   {
                       AccountId = DataAccessHelper.ToInt(row, "member_id"),
                       DateOfBirth = DataAccessHelper.ToDateTime(row, "date_of_birth"),
                       EmailAddress = DataAccessHelper.ToStr(row, "email_address"),
                       FirstName = DataAccessHelper.ToStr(row, "first_name"),
                       LastName = DataAccessHelper.ToStr(row, "last_name")
                   };
        }

        #endregion
    }
}
