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
    [Table("tblMaInputFields")]
    public class tblMaInputFields : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public string Field { get; set; }
        public string UniqueId { get; set; }
        public string Datatype { get; set; }
    }

   

   

   






}
