using HRMS.DL.AccountModels;
using HRMS.DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.ViewModels
{
    public class tblInterviewMasterViewModel
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

        [ForeignKey("tblMaInterviewTechnology")]
        public Int64 tblMaInterviewTechnologyID { get; set; }

        [ForeignKey("tblMaInterviewResult")]
        public Int64 tblMaInterviewResultId { get; set; }


        public virtual tblMaInterviewTechnology tblMaInterviewTechnology { get; set; }
        public virtual tblMaInterviewResult tblMaInterviewResult { get; set; }
        public virtual tblMaInterviewNoticePeriod tblMaInterviewNoticePeriod { get; set; }


        public Int64 DetailsId { get; set; }
        public DateTime ScheduleDateTime { get; set; }

        [ForeignKey("tblInterviewMaster")]
        public Int64 tblInterviewMasterId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string InterviewerId { get; set; }

        [ForeignKey("tblMaInterviewFeedbackStatus")]
        public Int64 tblMaInterviewFeedbackStatusId { get; set; }

        public string FeedbackDetails { get; set; }
        public bool IsInterviewTaken { get; set; }

        [ForeignKey("tblMaInterviewType")]
        public Int64 tblMaInterviewTypeId { get; set; }
        public virtual tblMaInterviewType tblMaInterviewType { get; set; }
        public virtual tblInterviewMaster tblInterviewMaster { get; set; }

        public List<ApplicationUser> AllUsers { get; set; }
        public List<tblMaCompanyBranch> AllBranches { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual tblMaInterviewFeedbackStatus tblMaInterviewFeedbackStatus { get; set; }

        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }

    }
}
