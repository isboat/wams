﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wams.Common.Logging;

namespace Wams.DAL.Repositories
{
    using System.ComponentModel;
    using System.Data;

    using MySql.Data.MySqlClient;

    using Wams.DataObjects.Accounts;
    using Wams.DAL.Interfaces;
    using Wams.DataObjects;
    using System.Globalization;

    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly ILogProvider logProvider;

        public AccountRepository(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }

        public BaseUserInfo Login(string email, string password)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, Login email:{0}", email));

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

                        cmd.Parameters.Add(new MySqlParameter("@caninvest_out", MySqlDbType.Int32));
                        cmd.Parameters["@caninvest_out"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new MySqlParameter("@canDoChildBenefit_out", MySqlDbType.Int32));
                        cmd.Parameters["@canDoChildBenefit_out"].Direction = ParameterDirection.Output;

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
                                LoginRole = Convert.ToInt32(cmd.Parameters["@loginrole_out"].Value.ToString()),
                                CanInvest = Convert.ToInt32(cmd.Parameters["@caninvest_out"].Value.ToString()) == 1,
                                CanDoChildBenefit = Convert.ToInt32(cmd.Parameters["@canDoChildBenefit_out"].Value.ToString()) == 1,
                                IsAdmin = false
                            };
                        }

                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, Login email:{0}", email), ex);
                throw;
            }
        }

        public UserAccount GetAccountInfo(int accountid)
        {
            this.logProvider.Info(string.Format("AccountRepository, GetAccountInfo accountid:{0}", accountid));

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
                                DateOfBirth = record["date_of_birth"].ToString(),
                                EmailAddress = record["email_address"].ToString(),
                                FirstName = record["first_name"].ToString(),
                                LastName = record["last_name"].ToString(),
                                Gender = record["gender"].ToString(),
                                Biography = record["biography"].ToString(),
                                Telephone = record["phone_number"].ToString(),
                                EmergencyTel = record["emergency_contact_number"].ToString(),
                                MembershipType = record["membershiptype"].ToString(),
                                LoginRole = Convert.ToInt32(record["loginrole"].ToString()),
                                ProfilePicUrl = record["picurl"].ToString(),
                                CanInvest = Convert.ToInt32(record["canInvest"].ToString()) == 1,
                                CanDoChildBenefit = Convert.ToInt32(record["canDoChildBenefit"].ToString()) == 1,
                                Address = record["address"].ToString()
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetAccountInfo accountid:{0}", accountid), ex);
                throw;
            }
        }

        public List<UserAccount> GetAllUserAccounts()
        {
            this.logProvider.Info("AccountRepository, GetAllUserAccounts");
            try
            {
                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from member_information where deleted = 0 AND loginrole < 2";

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
                                DateOfBirth = record["date_of_birth"].ToString(),
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
                this.logProvider.Error("AccountRepository, GetAllUserAccounts", ex);
                throw;
            }
        }

        public bool SetPasscode(string accountkey, string passcodeKey)
        {
            throw new NotImplementedException();
        }

        public int CreateApplication(string firstname, string lastname, string gender, string dob,
            string email, string address, string occupation, string password, string membershipType,
            int userLoginRole)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, CreateApplication firstname:{0}, lastname:{1}, dob:{2}, email:{3}, address:{4}",
                    firstname, lastname, dob, email, address));

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

                        cmd.Parameters.AddWithValue("@p_address", address);
                        cmd.Parameters["@p_address"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_occupation", occupation);
                        cmd.Parameters["@p_occupation"].Direction = ParameterDirection.Input;

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
                this.logProvider.Error(string.Format("AccountRepository, CreateApplication firstname:{0}, lastname:{1}, dob:{2}, email:{3}, address:{4}",
                    firstname, lastname, dob, email, address), ex);
                throw;
            }
        }

        public int UpdateAccountInfo(UserAccount userAccount)
        {
            if (userAccount == null)
            {
                return -1;
            }

            this.logProvider.Info(string.Format("AccountRepository, UpdateAccountInfo firstname:{0}, lastname:{1}, dob:{2}, email:{3}, membershipType:{4}",
                userAccount.FirstName, userAccount.LastName, userAccount.DateOfBirth, userAccount.EmailAddress, userAccount.MembershipType));

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

                        cmd.Parameters.AddWithValue("@p_loginrole", userAccount.LoginRole);
                        cmd.Parameters["@p_loginrole"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_caninvest", userAccount.CanInvest ? 1 : 0);
                        cmd.Parameters["@p_caninvest"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_canDoChildBenefit", userAccount.CanDoChildBenefit ? 1 : 0);
                        cmd.Parameters["@p_canDoChildBenefit"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateAccountInfo firstname:{0}, lastname:{1}, dob:{2}, email:{3}, membershipType:{4}",
                    userAccount.FirstName, userAccount.LastName, userAccount.DateOfBirth, userAccount.EmailAddress, userAccount.MembershipType), ex);

                throw;
            }
        }

        public int UpdateProfilePicUrl(int accountId, string url)
        {
            try
            {

                this.logProvider.Info(string.Format("AccountRepository, UpdateProfilePicUrl accountId:{0}, url:{1}", accountId, url));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updateprofileurl", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_id", accountId);
                        cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_url", url);
                        cmd.Parameters["@p_url"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateProfilePicUrl accountId:{0}, url:{1}", accountId, url), ex);
                throw;
            }
        }

        public int ChangePassword(string accountKey, string newPassword)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, ChangePassword accountId:{0}, newPassword:{1}", accountKey, newPassword));

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
                this.logProvider.Error(string.Format("AccountRepository, ChangePassword accountId:{0}, newPassword:{1}", accountKey, newPassword), ex);
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

        public int DeleteMember(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, DeleteMember id:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("update member_information set deleted = 1 where member_id = {0}", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, DeleteMember id:{0}", id), ex);
                throw;
            }
        }

        public int SetMemberPassword(int id, string newPassword)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, SetMemberPassword id:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("update member_information set password = '{0}' where member_id = {1}", newPassword, id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, SetMemberPassword id:{0}, newPassword={1}", id, newPassword), ex);
                throw;
            }
        }

        #region Dues

        public int AddMemberDues(MemberDues dues)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, AddMemberDues memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, duesMonth:{5}, duesYear:{6}, duesMemberName:{7}",
                    dues.MemberId, dues.AddedBy, dues.AddedById, dues.AddedDate, dues.Amount, dues.DuesMonth, dues.DuesYear, dues.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("adddues", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_mem_id", dues.MemberId);
                        cmd.Parameters["@p_mem_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_mem_name", dues.MemberName);
                        cmd.Parameters["@p_mem_name"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", dues.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", dues.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", dues.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", dues.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", dues.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", dues.Amount.ToString());
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, AddMemberDues memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, duesMonth:{5}, duesYear:{6}, duesMemberName:{7}",
                    dues.MemberId, dues.AddedBy, dues.AddedById, dues.AddedDate, dues.Amount, dues.DuesMonth, dues.DuesYear, dues.MemberName), ex);

                return -1;
            }
        }

        public List<MemberDues> ViewAllMemberDues(int accountId)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, ViewAllMemberDues memberId:{0}", accountId));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from dues where member_id = {0}", accountId);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<MemberDues>();
                        while (record.Read())
                        {
                            records.Add(new MemberDues
                            {
                                DuesId = Convert.ToInt32(record["duesid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["dues_month"].ToString(),
                                DuesYear = Convert.ToInt32(record["dues_year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, ViewAllMemberDues memberId:{0}", accountId), ex);
                throw;
            }
        }

        public MemberDues GetMemberDues(int duesid)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetMemberDues duesId:{0}", duesid));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from dues where duesid = {0} limit 1", duesid);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new MemberDues
                            {
                                DuesId = Convert.ToInt32(record["duesid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["dues_month"].ToString(),
                                DuesYear = Convert.ToInt32(record["dues_year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetMemberDues duesId:{0}", duesid), ex);
                throw;
            }
        }

        public int UpdateMemberDues(MemberDues dues)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateMemberDues memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, duesMonth:{5}, duesYear:{6}, duesMemberName:{7}",
                    dues.MemberId, dues.AddedBy, dues.AddedById, dues.AddedDate, dues.Amount, dues.DuesMonth, dues.DuesYear, dues.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updatedues", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_dues_id", dues.DuesId);
                        cmd.Parameters["@p_dues_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", dues.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", dues.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", dues.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", dues.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", dues.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", dues.Amount.ToString());
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateMemberDues memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, duesMonth:{5}, duesYear:{6}, duesMemberName:{7}",
                    dues.MemberId, dues.AddedBy, dues.AddedById, dues.AddedDate, dues.Amount, dues.DuesMonth, dues.DuesYear, dues.MemberName), ex);
                return -1;
            }
        }

        #endregion

        #region Benefits

        public int AddMemberSupport(MemberInvmt memberInvmt)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, AddMemberSupport memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    memberInvmt.MemberId, memberInvmt.AddedBy, memberInvmt.AddedById, memberInvmt.AddedDate, memberInvmt.Amount, memberInvmt.DuesMonth, memberInvmt.DuesYear, memberInvmt.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("addsupport", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_mem_id", memberInvmt.MemberId);
                        cmd.Parameters["@p_mem_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_mem_name", memberInvmt.MemberName);
                        cmd.Parameters["@p_mem_name"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", memberInvmt.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", memberInvmt.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", memberInvmt.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", memberInvmt.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", memberInvmt.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", memberInvmt.Amount.ToString());
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_stype", "child");
                        cmd.Parameters["@p_stype"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, AddMemberSupport memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    memberInvmt.MemberId, memberInvmt.AddedBy, memberInvmt.AddedById, memberInvmt.AddedDate, memberInvmt.Amount, memberInvmt.DuesMonth, memberInvmt.DuesYear, memberInvmt.MemberName), ex);

                return -1;
            }
        }

        public List<ChildBenefit> ViewAllMemberChildSupport(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, ViewAllMemberChildBenefits memberId:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from supports where member_id = {0} and support_type='child'", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<ChildBenefit>();
                        while (record.Read())
                        {
                            records.Add(new ChildBenefit
                            {
                                Id = Convert.ToInt32(record["sid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                Month = record["month"].ToString(),
                                Year = Convert.ToInt32(record["year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, ViewAllMemberChildBenefits memberId:{0}", id), ex);
                throw;
            }
        }

        public MemberInvmt GetMemberSupport(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetMemberSupport id:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from supports where sid = {0} limit 1", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new MemberInvmt
                            {
                                Id = Convert.ToInt32(record["sid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["month"].ToString(),
                                DuesYear = Convert.ToInt32(record["year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetMemberSupport id:{0}", id), ex);
                throw;
            }
        }

        public int UpdateMemberSupport(MemberInvmt investment)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateMemberSupport memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updatesupport", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_id", investment.Id);
                        cmd.Parameters["@p_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", investment.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", investment.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", investment.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", investment.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", investment.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", investment.Amount.ToString(CultureInfo.InvariantCulture));
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateMemberSupport memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName), ex);
                return -1;
            }
        }

        #endregion

        #region Investments

        public int AddMemberInvmt(MemberInvmt investment)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, AddMemberInvmt memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("addinvmt", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_mem_id", investment.MemberId);
                        cmd.Parameters["@p_mem_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_mem_name", investment.MemberName);
                        cmd.Parameters["@p_mem_name"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", investment.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", investment.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", investment.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", investment.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", investment.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", investment.Amount.ToString());
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, AddMemberInvmt memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName), ex);

                return -1;
            }
        }

        public List<MemberInvmt> ViewAllMemberInvestments(int accountId)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, ViewAllMemberInvestments memberId:{0}", accountId));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from investments where member_id = {0}", accountId);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<MemberInvmt>();
                        while (record.Read())
                        {
                            records.Add(new MemberInvmt
                            {
                                Id = Convert.ToInt32(record["invmtid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["invmt_month"].ToString(),
                                DuesYear = Convert.ToInt32(record["invmt_year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, ViewAllMemberInvestments memberId:{0}", accountId), ex);
                throw;
            }
        }

        public MemberInvmt GetMemberInvmt(int invmtid)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetMemberInvmt invmtId:{0}", invmtid));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from investments where invmtid = {0} limit 1", invmtid);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new MemberInvmt
                            {
                                Id = Convert.ToInt32(record["invmtid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["invmt_month"].ToString(),
                                DuesYear = Convert.ToInt32(record["invmt_year"].ToString()),
                                AddedDate = record["added_date"].ToString(),
                                AddedBy = record["added_by"].ToString(),
                                AddedById = Convert.ToInt32(record["added_by_id"].ToString())
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetMemberInvmt invmtId:{0}", invmtid), ex);
                throw;
            }
        }

        public int UpdateMemberInvmt(MemberInvmt investment)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateMemberInvmt memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updateinvmt", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@p_invmt_id", investment.Id);
                        cmd.Parameters["@p_invmt_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_month", investment.DuesMonth);
                        cmd.Parameters["@p_month"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_year", investment.DuesYear);
                        cmd.Parameters["@p_year"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_date", investment.AddedDate);
                        cmd.Parameters["@p_added_date"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by", investment.AddedBy);
                        cmd.Parameters["@p_added_by"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_added_by_id", investment.AddedById);
                        cmd.Parameters["@p_added_by_id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@p_amount", investment.Amount.ToString(CultureInfo.InvariantCulture));
                        cmd.Parameters["@p_amount"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateMemberInvmt memberId:{0}, addedBy:{1}, addedById:{2}, addedDate:{3}, amount:{4}, invmtMonth:{5}, invmtYear:{6}, invmtMemberName:{7}",
                    investment.MemberId, investment.AddedBy, investment.AddedById, investment.AddedDate, investment.Amount, investment.DuesMonth, investment.DuesYear, investment.MemberName), ex);
                return -1;
            }
        }

        public int RequestInvestmentWithdrawal(InvestmentWithdrawal pending)
        {
            if (pending == null)
            {
                return -1;
            }

            try
            {
                this.logProvider.Info(string.Format("AccountRepository, RequestInvestmentWithdrawal memberId:{0}, amount:{1}, granted:{2}, memberName:{3}, howtoPayYou: {4}",
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.HowToPayYou));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var cmdText = string.Format("INSERT INTO investmentrequest(member_id, member_name, amount, request_date, howtoPayYou, granted) VALUES({0}, '{1}', {2}, '{3}', '{4}', {5})",
                        pending.MemberId, pending.MemberName, pending.Amount, pending.RequestDate, pending.HowToPayYou, 0);

                    using (var cmd = new MySqlCommand(cmdText, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, RequestInvestmentWithdrawal memberId:{0}, amount:{1}, granted:{2}, memberName:{3}, howToPayYou: {4}",
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.HowToPayYou), ex);

                throw;
            }
        }

        public List<InvestmentWithdrawal> GetGrantedMemberInvestmentReqs(int memberId)
        {
            try
            {
                this.logProvider.Info("AccountRepository, GetGrantedMemberInvestmentReqs member_id: " + memberId);

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from investmentrequest where granted = 1 and member_id = {0};", memberId);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<InvestmentWithdrawal>();
                        while (record.Read())
                        {
                            records.Add(new InvestmentWithdrawal
                            {
                                PendingId = Convert.ToInt32(record["id"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                RequestDate = record["request_date"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                                HowToPayYou = record["howtoPayYou"].ToString()
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error("AccountRepository, GetGrantedMemberInvestmentReqs member_id: " + memberId, ex);
                throw;
            }
        }

        public List<PendingBase> GetAllInvestmentRequests()
        {
            try
            {
                this.logProvider.Info("AccountRepository, GetAllInvestmentRequests");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from investmentrequest where granted = 0;";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<PendingBase>();
                        while (record.Read())
                        {
                            records.Add(new PendingBase
                            {
                                PendingId = Convert.ToInt32(record["id"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                RequestDate = record["request_date"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error("AccountRepository, GetAllInvestmentRequests", ex);
                throw;
            }
        }

        public InvestmentWithdrawal GetInvestmentWithdrawRequest(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetInvestmentWithdrawRequest id:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from investmentrequest where granted = 0 and id = {0} limit 1", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new InvestmentWithdrawal
                            {
                                PendingId = Convert.ToInt32(record["id"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                RequestDate = record["request_date"].ToString(),
                                HowToPayYou = record["howtoPayYou"].ToString(),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetInvestmentWithdrawRequest id:{0}", id), ex);
                throw;
            }
        }

        public int UpdateInvestmentRequest(InvestmentWithdrawal request)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateInvestmentRequest memberId:{0}, amount:{1}, granted:{2}, memberid:{3}, MemberName:{4}, howtoPayYou: {5}",
                    request.MemberId, request.Amount, request.Granted, request.MemberId, request.MemberName, request.HowToPayYou));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var updateStatement = string.Format("update investmentrequest set amount = {0}, granted = {1}, request_date = '{2}', howtoPayYou = '{3}' where id = {4};",
                        request.Amount, request.Granted ? 1 : 0, request.RequestDate, request.HowToPayYou, request.PendingId);

                    using (var cmd = new MySqlCommand(updateStatement, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        connection.Open();

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateInvestmentRequest memberId:{0}, amount:{1}, granted:{2}, memberid:{3}, MemberName:{4}, howtoPayYou:{5}",
                    request.MemberId, request.Amount, request.Granted, request.MemberId, request.MemberName, request.HowToPayYou), ex);

                return -1;
            }
        }

        #endregion

        #region Request Loan

        public int RequestLoan(PendingLoan pending)
        {
            if (pending == null)
            {
                return -1;
            }

            try
            {
                this.logProvider.Info(string.Format("AccountRepository, RequestLoan memberId:{0}, amount:{1}, granted:{2}, memberName:{3}, pendingLoanId:{4}, reason:{5}",
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.PendingId, pending.Reason));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("addloanrequest", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@memid", pending.MemberId);
                        cmd.Parameters["@memid"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memname", pending.MemberName);
                        cmd.Parameters["@memname"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@amount", pending.Amount);
                        cmd.Parameters["@amount"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@reason", pending.Reason);
                        cmd.Parameters["@reason"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, RequestLoan memberId:{0}, amount:{1}, granted:{2}, memberName:{3}, pendingLoanId:{4}, reason:{5}",
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.PendingId, pending.Reason), ex);

                throw;
            }
        }

        public List<PendingLoan> GetAllPendingdLoans()
        {
            try
            {
                this.logProvider.Info("AccountRepository, GetAllPendingdLoans");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from loans where granted = 0;";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<PendingLoan>();
                        while (record.Read())
                        {
                            records.Add(new PendingLoan
                            {
                                PendingId = Convert.ToInt32(record["loanid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                Reason = record["reason"].ToString(),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error("AccountRepository, GetAllPendingdLoans", ex);
                throw;
            }
        }

        public PendingLoan GetPendingdLoan(int loanid)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetPendingdLoan id:{0}", loanid));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from loans where granted = 0 and loanid = {0} limit 1", loanid);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new PendingLoan
                            {
                                PendingId = Convert.ToInt32(record["loanid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                Reason = record["reason"].ToString(),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetPendingdLoan id:{0}", loanid), ex);
                throw;
            }
        }

        public int UpdateLoan(PendingLoan request)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateLoan memberId:{0}, amount:{1}, granted:{2}, memberid:{3}, MemberName:{4}",
                    request.MemberId, request.Amount, request.Granted, request.MemberId, request.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updateloan", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@id", request.PendingId);
                        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@amount", request.Amount);
                        cmd.Parameters["@amount"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memname", request.MemberName);
                        cmd.Parameters["@memname"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@reason", request.Reason);
                        cmd.Parameters["@reason"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memid", request.MemberId);
                        cmd.Parameters["@memid"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@granted", request.Granted ? 1 : 0);
                        cmd.Parameters["@granted"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateLoan memberId:{0}, amount:{1}, granted:{2}, memberid:{3}, MemberName:{4}",
                    request.MemberId, request.Amount, request.Granted, request.MemberId, request.MemberName), ex);

                return -1;
            }
        }

        #endregion

        #region Benefit

        public int BenefitRequest(PendingBenefitRequest pending)
        {
            if (pending == null)
            {
                return -1;
            }

            try
            {
                this.logProvider.Info(string.Format("AccountRepository, BenefitRequest memberId:{0}, BenefitDate:{1}, granted:{2}, BenefitType:{3}, MemberName:{4}",
                    pending.MemberId, pending.RequestDate, pending.Granted, pending.BenefitType, pending.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("addbenefitrequest", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@memid", pending.MemberId);
                        cmd.Parameters["@memid"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memname", pending.MemberName);
                        cmd.Parameters["@memname"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@benefitdate", pending.RequestDate);
                        cmd.Parameters["@benefitdate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@benefittype", pending.BenefitType);
                        cmd.Parameters["@benefittype"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@message", pending.Message);
                        cmd.Parameters["@message"].Direction = ParameterDirection.Input;

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, BenefitRequest memberId:{0}, BenefitDate:{1}, granted:{2}, BenefitType:{3}, MemberName:{4}",
                    pending.MemberId, pending.RequestDate, pending.Granted, pending.BenefitType, pending.MemberName), ex);

                throw;
            }
        }

        public List<PendingBenefitRequest> GetAllPendingdBenefits()
        {
            try
            {
                this.logProvider.Info("AccountRepository, GetAllPendingdBenefits");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select * from benefits where granted = 0;";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader();

                        var records = new List<PendingBenefitRequest>();
                        while (record.Read())
                        {
                            records.Add(new PendingBenefitRequest
                            {
                                PendingId = Convert.ToInt32(record["benefitid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Message = record["message"].ToString(),
                                BenefitType = record["benefittype"].ToString(),
                                RequestDate = record["benefitdate"].ToString(),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            });
                        }

                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error("AccountRepository, GetAllPendingdBenefits", ex);

                throw;
            }
        }

        public PendingBenefitRequest GetPendingdBenefits(int id)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, GetPendingdBenefits id:{0}", id));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = string.Format("select * from benefits where benefitid = {0} limit 1", id);

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        connection.Open();

                        var record = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        if (record.Read())
                        {
                            return new PendingBenefitRequest
                            {
                                PendingId = Convert.ToInt32(record["benefitid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Message = record["message"].ToString(),
                                BenefitType = record["benefittype"].ToString(),
                                RequestDate = record["benefitdate"].ToString(),
                                Granted = Convert.ToInt32(record["granted"].ToString()) == 1,
                            };
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, GetPendingdBenefits id:{0}", id), ex);
                throw;
            }
        }

        public int UpdateBenefit(PendingBenefitRequest request)
        {
            try
            {
                this.logProvider.Info(string.Format("AccountRepository, UpdateBenefit memberId:{0}, BenefitDate:{1}, granted:{2}, BenefitType:{3}, MemberName:{4}",
                    request.MemberId, request.RequestDate, request.Granted, request.BenefitType, request.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updatebenefit", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@id", request.PendingId);
                        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@benefitdate", request.RequestDate);
                        cmd.Parameters["@benefitdate"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@benefittype", request.BenefitType);
                        cmd.Parameters["@benefittype"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memname", request.MemberName);
                        cmd.Parameters["@memname"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@message", request.Message);
                        cmd.Parameters["@message"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@memid", request.MemberId);
                        cmd.Parameters["@memid"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@granted", request.Granted ? 1 : 0);
                        cmd.Parameters["@granted"].Direction = ParameterDirection.Input;

                        var results = cmd.ExecuteNonQuery();

                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logProvider.Error(string.Format("AccountRepository, UpdateBenefit memberId:{0}, BenefitDate:{1}, granted:{2}, BenefitType:{3}, MemberName:{4}",
                    request.MemberId, request.RequestDate, request.Granted, request.BenefitType, request.MemberName), ex);

                return -1;
            }
        }

        #endregion

        #region Private methods

        private UserAccount CreateUserAccount(IDataRecord row)
        {
            return new UserAccount
            {
                AccountId = DataAccessHelper.ToInt(row, "member_id"),
                DateOfBirth = DataAccessHelper.ToStr(row, "date_of_birth"),
                EmailAddress = DataAccessHelper.ToStr(row, "email_address"),
                FirstName = DataAccessHelper.ToStr(row, "first_name"),
                LastName = DataAccessHelper.ToStr(row, "last_name")
            };
        }

        #endregion
    }
}
