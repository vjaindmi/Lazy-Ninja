using HRMS.DL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.AccountModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<tblCertificateInputFieldDetails> tblCertificateInputFieldDetails { get; set; }
        public DbSet<tblMaColors> tblMaColors { get; set; }
        public DbSet<tblMaFonts> tblMaFonts { get; set; }
        public DbSet<tblMaFontAttributes> tblMaFontAttributes { get; set; }
        public DbSet<tblMaCertificateTypes> tblMaCertificateTypes { get; set; }
        public DbSet<tblMaCertificateSubTypes> tblMaCertificateSubTypes { get; set; }
        public DbSet<tblMaInputFields> tblMaInputFields { get; set; }
        public DbSet<tblMaDesignation> tblMaDesignation { get; set; }
        public DbSet<tblMaCompanyBranch> tblMaCompanyBranch { get; set; }
        public DbSet<tblMaNotifications> tblMaNotifications { get; set; }
        public DbSet<tblInterviewMaster> tblInterviewMasters { get; set; } 
        public DbSet<tblMaInterviewNoticePeriod> tblMaInterviewNoticePeriods { get; set; } 
        public DbSet<tblMaInterviewResult> tblMaInterviewResults { get; set; } 
        public DbSet<tblMaInterviewTechnology> tblMaInterviewTechnologies { get; set; } 
        public DbSet<tblInterviewDetails> tblInterviewDetails { get; set; } 
        public DbSet<tblMaInterviewFeedbackStatus> tblMaInterviewFeedbackStatus { get; set; }
        public DbSet<tblMaInterviewType> tblMaInterviewTypes { get; set; }
    }
}
