using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.RegularExpressions;

namespace CivilisDB.Models
{
    [Table("tblpersonattributesaggregate")]
    public partial class tblPersonAttributesAggregate : BaseEntity
    {
        [Column, Key]
        public long fldpersonid { get; set; }

        public string fldaggattributesarr { get; set; } = "";
    }

    public class tblPersonAttributesAggregateDac : DapperBaseRepository<tblPersonAttributesAggregate>
    {
        private const string tableName = "tblpersonattributesaggregate";
        private const string pkeyName = "fldpersonid";

        public tblPersonAttributesAggregateDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblPersonAttributesAggregateDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public tblPersonAttributesAggregate GetAggByFlduserId(long fldpersonid)
        {
            var sql = "SELECT vwpersonattribute.fldpersonid, " +
                        " array_to_string(array_agg(vwpersonattribute.fldattributedescription), ', '::text) AS fldaggattributesarr " +
                        " FROM vwpersonattribute " +
                        " where fldpersonid = " + fldpersonid.ToString() +
                        " GROUP BY vwpersonattribute.fldpersonid " +
                        " ORDER BY vwpersonattribute.fldpersonid; ";
            try
            {
                var obj = Connection.QueryFirstOrDefault<tblPersonAttributesAggregate>(sql);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByFlduserId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public tblPersonAttributesAggregate SavInsert (long fldpersonid)
        {
            tblPersonAttributesAggregate objDB, objAgg;

            objAgg = GetAggByFlduserId(fldpersonid);
            objDB = Get(fldpersonid);
            if (objDB == null)
            {
                Insert(objAgg);
            } else
            {
                Update(objAgg);
            }
            return objAgg;
        }


        public Boolean SavInsertAll()
        {
            List<tblPerson> personList;
            using (tblPersonDac dac = new tblPersonDac(Connection, _logger))
            {
                personList = dac.GetAllSortedDesc();
                foreach (tblPerson person in personList)
                {
                    SavInsert(person.fldpersonid);
                }
            }
            return true;
        }


        ~tblPersonAttributesAggregateDac()
        {
            if (Connection != null)
                Connection.Close();
        }


    }

}
