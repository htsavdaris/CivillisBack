using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
namespace CivilisDB.Models;


[Table("tblcaserow")]
public partial class tblCaseRow :BaseEntity
{
    [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long fldcaserowid { get; set; }

    public long? fldcaseid { get; set; } = 0;
    
    public string fldactiondesc { get; set; } = "";


}


public class tblCaseRowDac : DapperBaseRepository<tblCaseRow>
{
    private const string tableName = "tblcaserow";
    private const string pkeyName = "fldcaserowid";

    public tblCaseRowDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
    {

    }

    public tblCaseRowDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
    {

    }

    public List<tblCaseRow> GetByCaseID(long caseid)
    {
        try
        {
            var obj = Connection.Query<tblCaseRow>(SqlSelectCommand + " WHERE fldcaseid = @fldcaseid ORDER BY fldcreatedat ASC ", new { fldcaseid = caseid }).AsList();
            return obj;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(1, "GetByCaseID: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
            return null;
        }

    }

    ~tblCaseRowDac()
    {
        if (Connection != null)
            Connection.Close();
    }
}