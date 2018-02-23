using HRMS.DL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DL.AccountModels
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("tblMaDesignation")]
        public Nullable<Int64> tblMaDesignationId { get; set; }
        [ForeignKey("tblMaCompanyBranch")]
        public Nullable<Int64> tblMaCompanyBranchId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public string EmployeeId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfBirthday { get; set; }
        public Nullable<DateTime> DateOfAnniversary { get; set; }
        public Nullable<DateTime> DateOfResign { get; set; }
        public bool IsResigned { get; set; }

        public virtual tblMaDesignation tblMaDesignation { get; set; }
        public virtual tblMaCompanyBranch tblMaCompanyBranch { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
