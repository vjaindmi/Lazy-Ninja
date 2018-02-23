using HRMS.DL.AccountModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Entities
{
    [Table("tblMaInterviewType")]
    public class tblMaInterviewType : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public string Type { get; set; }
    }
}
