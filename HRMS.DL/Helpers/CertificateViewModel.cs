using HRMS.DL.AccountModels;
using HRMS.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.Helpers
{
    public class CertificateViewModel
    {
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public string ContactNumber { get; set; }
        public string BloodGroup { get; set; }
        public string Photo { get; set; }
        public string CompletedYear { get; set; }
        public string CurrentMonthYear { get; set; }
        public string Year { get; set; }
        public string TeamProjectName { get; set; }
        public string TeamName { get; set; }
        public string ProjectName { get; set; }

        public string CardTitle { get; set; }
        public string CardTemplate { get; set; }
        public string subCertificates { get; set; }
        public string baseUrl { get; set; }
        public string EmailText { get; set; }
        public string DownloadFormat { get; set; }

        public List<ApplicationUser> ApplicationUser { get; set; }
        public List<tblMaCompanyBranch> tblMaCompanyBranchs { get; set; }
        public List<tblCertificateInputFieldDetails> FieldDetails { get; set; }
    }
}
