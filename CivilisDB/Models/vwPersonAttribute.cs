using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using static Dapper.SqlMapper;


namespace CivilisDB.Models;

[Table("vwpersonattribute")]
public partial class vwPersonAttribute : BaseEntity
{
    [Column, Key]
    public long? fldpersonattrid { get; set; }

    public long? fldpersonid { get; set; }

    public string fldpersonlastname { get; set; }

    public string fldpersonfirstname { get; set; }


    public long? fldattributeid { get; set; }

    public string fldattributedescription { get; set; }
}

public class vwPersonAttributeDac : DapperBaseRepository<vwPersonAttribute>
{

    private const string tableName = "vwpersonattribute";
    private const string pkeyName = "fldpersonattrid";

    public vwPersonAttributeDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
    {

    }

    public vwPersonAttributeDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
    {
    }

    public List<vwPersonAttribute> GetAllForPersonId(long personid)
    {
        _logger.LogTrace(1, "GetAllForId is called");
        try
        {
            var obj = Connection.Query<vwPersonAttribute>(SqlSelectCommand + " WHERE fldpersonid = @personid ORDER BY " + pkeyName, new { personid = personid }).ToList();
            return obj;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(3, typeof(tblEntity).FullName + " GetAllForPersonId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
            return null;
        }
    }

    public List<vwPersonAttribute> GetAllForAttrId(long attrId)
    {
        _logger.LogTrace(1, "GetAllForId is called");
        try
        {
            var obj = Connection.Query<vwPersonAttribute>(SqlSelectCommand + " WHERE fldattributeid = @attrId ORDER BY " + pkeyName, new { attrId = attrId }).ToList();
            return obj;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(3, typeof(tblEntity).FullName + " GetAllForAttrId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
            return null;
        }
    }


    ~vwPersonAttributeDac()
    {
        if (Connection != null)
            Connection.Close();
    }

}

