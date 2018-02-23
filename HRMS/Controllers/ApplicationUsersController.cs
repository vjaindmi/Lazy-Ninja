using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HRMS.DL.AccountModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace HRMS.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            var applicationUsers = db.Users.Include(a => a.tblMaCompanyBranch).Include(a => a.tblMaDesignation);
            return PartialView(await applicationUsers.ToListAsync());
        }

        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Where(s => s.Id == id).ToList().FirstOrDefault();
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return PartialView(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            ViewBag.tblMaCompanyBranchId = new SelectList(db.tblMaCompanyBranch, "Id", "Location");
            ViewBag.tblMaDesignationId = new SelectList(db.tblMaDesignation.ToList().OrderBy(s=>s.Designation), "Id", "Designation");
            return PartialView();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,MiddleName,LastName,ImagePath,tblMaDesignationId,tblMaCompanyBranchId,IsDeleted,IsActive,EmployeeId,DateOfJoining,DateOfBirthday,DateOfAnniversary,DateOfResign,IsResigned,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            try
            {
                if (!db.Users.Any(u => u.UserName == applicationUser.Email))
                {
                    applicationUser.UserName = applicationUser.Email;
                    // add user in role
                    var store = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(store);
                    await manager.CreateAsync(applicationUser, "password#01");

                    if (!await manager.IsInRoleAsync(applicationUser.Id, "Employee"))
                        await manager.AddToRoleAsync(applicationUser.Id, "Employee");
                    //db.ApplicationUsers.Add(applicationUser);
                    //await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.tblMaCompanyBranchId = new SelectList(db.tblMaCompanyBranch, "Id", "Location", applicationUser.tblMaCompanyBranchId);
                ViewBag.tblMaDesignationId = new SelectList(db.tblMaDesignation.ToList().OrderBy(s => s.Designation), "Id", "Designation", applicationUser.tblMaDesignationId);
                return PartialView(applicationUser);
            }
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Where(s => s.Id == id).ToList().FirstOrDefault();
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.tblMaCompanyBranchId = new SelectList(db.tblMaCompanyBranch, "Id", "Location", applicationUser.tblMaCompanyBranchId);
            ViewBag.tblMaDesignationId = new SelectList(db.tblMaDesignation, "Id", "Designation", applicationUser.tblMaDesignationId);
            return PartialView(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,ImagePath,tblMaDesignationId,tblMaCompanyBranchId,IsDeleted,IsActive,EmployeeId,DateOfJoining,DateOfBirthday,DateOfAnniversary,DateOfResign,IsResigned,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.tblMaCompanyBranchId = new SelectList(db.tblMaCompanyBranch, "Id", "Location", applicationUser.tblMaCompanyBranchId);
            ViewBag.tblMaDesignationId = new SelectList(db.tblMaDesignation, "Id", "Designation", applicationUser.tblMaDesignationId);
            return PartialView(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Where(s => s.Id == id).ToList().FirstOrDefault();
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return PartialView(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Where(s => s.Id == id).ToList().FirstOrDefault();
            db.Users.Remove(applicationUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
