using System;
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

    public class AccountRepository :BaseRepository, IAccountRepository
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
                                       DateOfBirth = Convert.ToDateTime(record["date_of_birth"].ToString()),
                                       EmailAddress = record["email_address"].ToString(),
                                       FirstName = record["first_name"].ToString(),
                                       LastName = record["last_name"].ToString(),
                                       Gender = record["gender"].ToString(),
                                       Biography = record["biography"].ToString(),
                                       Telephone = record["phone_number"].ToString(),
                                       EmergencyTel = record["emergency_contact_number"].ToString(),
                                       MembershipType = record["membershiptype"].ToString(),
                                       UserLoginRole = Convert.ToInt32(record["loginrole"].ToString()),
                                       ProfilePicUrl = record["picurl"].ToString()
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
                this.logProvider.Error("AccountRepository, GetAllUserAccounts", ex);
                throw;
            }
        }

        public bool SetPasscode(string accountkey, string passcodeKey)
        {
            throw new NotImplementedException();
        }

        public int CreateApplication(string firstname, string lastname, string gender, DateTime dob,
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

                        cmd.Parameters.AddWithValue("@p_loginrole", userAccount.UserLoginRole);
                        cmd.Parameters["@p_loginrole"].Direction = ParameterDirection.Input;

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

                        cmd.Parameters.AddWithValue("@p_added_date", dues.AddedDate.ToString());
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
                                AddedDate = DateTime.Parse(record["added_date"].ToString()),
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

                        if(record.Read())
                        {
                            return new MemberDues
                            {
                                DuesId = Convert.ToInt32(record["duesid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Amount = Convert.ToDecimal(record["amount"].ToString()),
                                DuesMonth = record["dues_month"].ToString(),
                                DuesYear = Convert.ToInt32(record["dues_year"].ToString()),
                                AddedDate = Convert.ToDateTime(record["added_date"].ToString()),
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

                        cmd.Parameters.AddWithValue("@p_added_date", dues.AddedDate.ToString());
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
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.PendingLoanId, pending.Reason));

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
                    pending.MemberId, pending.Amount, pending.Granted, pending.MemberName, pending.PendingLoanId, pending.Reason), ex);

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
                                PendingLoanId = Convert.ToInt32(record["loanid"].ToString()),
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
                    pending.MemberId, pending.BenefitDate, pending.Granted, pending.BenefitType, pending.MemberName));

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

                        cmd.Parameters.AddWithValue("@benefitdate", pending.BenefitDate);
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
                    pending.MemberId, pending.BenefitDate, pending.Granted, pending.BenefitType, pending.MemberName), ex);

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
                                BenefitId = Convert.ToInt32(record["benefitid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Message = record["message"].ToString(),
                                BenefitType = record["benefittype"].ToString(),
                                BenefitDate = record["benefitdate"].ToString(),
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
                                BenefitId = Convert.ToInt32(record["benefitid"].ToString()),
                                MemberId = Convert.ToInt32(record["member_id"].ToString()),
                                MemberName = record["member_name"].ToString(),
                                Message = record["message"].ToString(),
                                BenefitType = record["benefittype"].ToString(),
                                BenefitDate = record["benefitdate"].ToString(),
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
                    request.MemberId, request.BenefitDate, request.Granted, request.BenefitType, request.MemberName));

                using (var connection = new MySqlConnection(this.ConString))
                {
                    using (var cmd = new MySqlCommand("updatebenefit", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        cmd.Parameters.AddWithValue("@id", request.BenefitId);
                        cmd.Parameters["@id"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@benefitdate", request.BenefitDate);
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
                    request.MemberId, request.BenefitDate, request.Granted, request.BenefitType, request.MemberName), ex);

                return -1;
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
                                PendingLoanId = Convert.ToInt32(record["loanid"].ToString()),
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

                        cmd.Parameters.AddWithValue("@id", request.PendingLoanId);
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
