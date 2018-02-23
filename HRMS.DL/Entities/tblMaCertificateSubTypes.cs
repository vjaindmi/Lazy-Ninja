using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Entities
{
    [Table("tblMaCertificateSubTypes")]
    public class tblMaCertificateSubTypes : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        [ForeignKey("tblMaCertificateTypes")]
        public Int64 tblMaCertificateTypesId { get; set; }
        public string CertificateName { get; set; }
        public string CertificateTemplateUrl { get; set; }
        public virtual tblMaCertificateTypes tblMaCertificateTypes { get; set; }
    }
     

}
