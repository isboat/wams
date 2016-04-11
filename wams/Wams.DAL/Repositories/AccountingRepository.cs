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
                    var query = string.Format("select * from dues where dues_year = {0}", year);

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
    }
}
