using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Entities
{
    [Table("tblCertificateInputFieldDetails")]
    public class tblCertificateInputFieldDetails : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        [ForeignKey("tblMaCertificateSubTypes")]
        public Int64 tblMaCertificateSubTypesId { get; set; }
        [ForeignKey("tblMaInputFields")]
        public Int64 tblMaInputFieldsId { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        [ForeignKey("tblMaColors")]
        public Int64 Color { get; set; }
        [ForeignKey("tblMaFonts")]
        public Int64 Font { get; set; }
        public string FontSize { get; set; }
        [ForeignKey("tblMaFontAttributes")]
        public Int64 FontAttribute { get; set; } 
        //[ForeignKey("tblMaFieldAlignment")]
        public string tblMaFieldAlignmentId { get; set; } 
       // public virtual tblMaFieldAlignment tblMaFieldAlignment { get; set; }
        public virtual tblMaCertificateSubTypes tblMaCertificateSubTypes { get; set; }
        public virtual tblMaInputFields tblMaInputFields { get; set; }
        public virtual tblMaColors tblMaColors { get; set; }
        public virtual tblMaFonts tblMaFonts { get; set; }
        public virtual tblMaFontAttributes tblMaFontAttributes { get; set; }
    }
}
