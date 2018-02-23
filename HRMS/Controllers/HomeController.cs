using HRMS.DL.AccountModels;
using HRMS.DL.Entities;
using HRMS.DL.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HRMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //await InitilizeFirstUser();
            if (Request.IsAuthenticated)
            {
                //return RedirectToAction("pdfPage", "Template");
                return RedirectToAction("Dashboard", "Template");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public async Task<ActionResult> EmployeeList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var applicationUsers = db.Users.Include(a => a.tblMaCompanyBranch).Include(a => a.tblMaDesignation);
            return View(await applicationUsers.ToListAsync());
        }
        
        public async Task InitilizeFirstUser()
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                //var success = context.Database.CreateIfNotExists();
                //if (!success)
                //    return;

                //add branch
                //var existingBranch = context.tblMaCompanyBranch.ToList();
                //if (existingBranch.Count == 0)
                //{
                //    foreach (var item in ApplicationConstants.ListBranch)
                //        context.tblMaCompanyBranch.Add(new tblMaCompanyBranch() { Location = item, IsActive = true, IsDeleted = false });
                //    await context.SaveChangesAsync();
                //}

                //add designations
                //var existingDesignation = context.tblMaDesignation.ToList();
                //if (existingDesignation.Count == 0)
                //{
                //    foreach (var item in ApplicationConstants.ListDesignation)
                //        context.tblMaDesignation.Add(new tblMaDesignation() { Designation = item, IsActive = true, IsDeleted = false });
                //    await context.SaveChangesAsync();
                //}

                //add certificate types
                //var existingCertificates = context.tblMaCertificateTypes.ToList();
                //if (existingCertificates.Count == 0)
                //{
                //    try
                //    {
                //        foreach (var item in ApplicationConstants.ListCertificateType)
                //        {
                //            string[] CertificatesDetails = item.Split('-');
                //            var data = new tblMaCertificateTypes() { CertificateName = CertificatesDetails[0], IsActive = true, IsDeleted = false };
                //            var updatedData = context.tblMaCertificateTypes.Add(data);
                //            await context.SaveChangesAsync();

                //            string[] allCertificates = CertificatesDetails[1].Split(',');
                //            foreach (var subitem in allCertificates)
                //            {
                //                context.tblMaCertificateSubTypes.Add(new tblMaCertificateSubTypes() { CertificateName = subitem, tblMaCertificateTypesId = updatedData.Id, IsActive = true, IsDeleted = false });
                //            }
                //        }
                //        await context.SaveChangesAsync();
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}

                //add input keys 
                //var existingKeys = context.tblMaInputFields.ToList();
                //if (existingKeys.Count == 0)
                //{
                //    foreach (var item in ApplicationConstants.ListFilelds)
                //    {
                //        string[] fieldDetails = item.Split('-');
                //        context.tblMaInputFields.Add( new tblMaInputFields() { Field=fieldDetails[0], UniqueId= fieldDetails[0].Replace(" ","").Replace("/",""), Datatype =fieldDetails[1], IsActive=true, IsDeleted=false });
                //    }
                //    await context.SaveChangesAsync();
                //}

                //create roles
                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    try
                    {
                        foreach (var item in ApplicationConstants.ListRoles)
                        await manager.CreateAsync(new IdentityRole() { Name = item });
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //create default usere
                if (!context.Users.Any(u => u.UserName == "sroychoudhary@dminc.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "sroychoudhary@dminc.com", FirstName = "Sushmita", MiddleName = "", LastName = "Choudhary", ImagePath = "/Content/images/SRC.png", tblMaCompanyBranchId = 1, tblMaDesignationId = 1, IsActive = true, IsDeleted = false, DateOfJoining=DateTime.Now, DateOfBirthday=DateTime.Now,IsResigned=false,  };
                    await manager.CreateAsync(user, "password#01");
                    await manager.AddToRoleAsync(user.Id, "HR");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}