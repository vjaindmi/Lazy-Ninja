using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Entities
{
    
    [Table("tblMaCertificateTypes")]
    public class tblMaCertificateTypes : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public string CertificateName { get; set; }
    }

}
