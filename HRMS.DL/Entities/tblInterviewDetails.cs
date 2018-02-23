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
    [Table("tblInterviewDetails")]
    public class tblInterviewDetails : BaseClass
    {
        [Key]
        public Int64 Id { get; set; }
        public DateTime ScheduleDateTime { get; set; }

        [ForeignKey("tblInterviewMaster")]
        public Int64 tblInterviewMasterId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string InterviewerId { get; set; }

        [ForeignKey("tblMaInterviewFeedbackStatus")]
        public Int64 tblMaInterviewFeedbackStatusId { get; set; }

        public string FeedbackDetails { get; set; }
        public string Others { get; set; }
        public bool IsInterviewTaken { get; set; }

        [ForeignKey("tblMaInterviewType")]
        public Int64 tblMaInterviewTypeId { get; set; }
        public virtual tblMaInterviewType tblMaInterviewType { get; set; }
        public virtual tblInterviewMaster tblInterviewMaster { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual tblMaInterviewFeedbackStatus tblMaInterviewFeedbackStatus { get; set; }
    }
}
