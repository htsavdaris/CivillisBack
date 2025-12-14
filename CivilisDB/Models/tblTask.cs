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
[Table("tbltask")]
public partial class tblTask : BaseEntity
{
    [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long fldtaskid { get; set; }

    public string fldtitle { get; set; }

    public long? fldimportance { get; set; }

    public long? fldassignor { get; set; }

    public long? fldassignee { get; set; }

    public bool? fldiscompleted { get; set; }

    public long? fldtasksecuritygroup { get; set; }

    public string flddescription { get; set; }

    public long? createdby { get; set; }

    public long? updatedby { get; set; }
    
    public DateTime? flddeadline { get; set; }

    public DateTime? createdat { get; set; }
    
    public DateTime? updatedat { get; set; }
}