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
    public class AccountingRepository : BaseRepository, IAccountingRepository
    {
        private readonly ILogProvider logProvider;

        public AccountingRepository(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }


        public List<MemberDues> GetAll_AnnualDues(int year)
        {
            try
            {
                this.logProvider.Info("AccountingRepository, GetAll_AnnualDues");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    var query = "select duesid, d.member_id, member_name, dues_month, dues_year, added_date, added_by, added_by_id, amount " +
                                "from dues d, member_information m " +
                                "where d.dues_year = " + year +" and m.member_id = d.member_id and m.deleted = 0;";
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
                this.logProvider.Error("AccountingRepository, GetAll_AnnualDues", ex);
                throw;
            }
        }

        public List<MemberInvmt> GetAll_Investments(int year)
        {
            try
            {
                this.logProvider.Info("AccountingRepository, GetAll_Investments");

                using (var connection = new MySqlConnection(this.ConString))
                {
                    //var query = string.Format("select * from investments where invmt_year = {0} and invmt_type = 'investment'", year);
                    var query = "select invmtid, i.member_id, member_name, invmt_month, invmt_year, added_date, added_by, added_by_id, amount " +
                                "from investments i, member_information m where i.invmt_year = " + year +
                                " and i.invmt_type = 'investment' and m.member_id = i.member_id and m.deleted = 0;";
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
                this.logProvider.Error("AccountingRepository, GetAll_Investments", ex);
                throw;
            } 
        }
    }
}
