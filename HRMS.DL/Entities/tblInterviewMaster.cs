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
    [Table("tblInterviewMaster")]
    public class tblInterviewMaster : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public string StudentName { get; set; }

        public string TotalExperience { get; set; }
        [ForeignKey("tblMaInterviewNoticePeriod")]
        public Int64 NoticePeriod { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentCTC { get; set; }
        public string ExpectedCTC { get; set; }

        public string OtherTechnology { get; set; }
        public string ResumePath { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }

        [ForeignKey("tblMaInterviewTechnology")]
        public Int64 tblMaInterviewTechnologyID { get; set; }

        [ForeignKey("tblMaInterviewResult")]
        public Int64 tblMaInterviewResultId { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string WhoScheduledId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual tblMaInterviewTechnology tblMaInterviewTechnology { get; set; }
        public virtual tblMaInterviewResult tblMaInterviewResult { get; set; }
        public virtual tblMaInterviewNoticePeriod tblMaInterviewNoticePeriod { get; set; }

    }

   
}
