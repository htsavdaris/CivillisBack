using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivilisDB.Models
{
    public abstract class BaseEntity
    {
        public DateTime? fldcreatedat { get; set; } = DateTime.Now;

        public DateTime? fldupdatedat { get; set; } = DateTime.Now;

        public long? fldcreatedby { get; set; } = 0;

        public long? fldupdatedby { get; set; } = 0;
    }
}
