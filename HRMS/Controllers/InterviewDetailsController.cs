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
using HRMS.DL.Entities;

namespace HRMS.Controllers
{
    [Authorize]
    public class InterviewDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InterviewDetails
        public async Task<ActionResult> Index()
        {
            var tblInterviewDetails = db.tblInterviewDetails.Include(t => t.ApplicationUser).Include(t => t.tblMaInterviewFeedbackStatus).Include(t => t.tblMaInterviewType);
            return View(await tblInterviewDetails.ToListAsync());
        }

        // GET: InterviewDetails/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewDetails tblInterviewDetails = await db.tblInterviewDetails.FindAsync(id);
            if (tblInterviewDetails == null)
            {
                return HttpNotFound();
            }
            return View(tblInterviewDetails);
        }

        // GET: InterviewDetails/Create
        public ActionResult Create()
        {
            ViewBag.InterviewerId = new SelectList(GetUsers(), "Id", "FirstName");
            ViewBag.tblMaInterviewFeedbackStatusId = new SelectList(db.tblMaInterviewFeedbackStatus, "Id", "Status");
            ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type");
            return View();
        }

        // POST: InterviewDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ScheduleDateTime,InterviewerId,tblMaInterviewFeedbackStatusId,FeedbackDetails,IsInterviewTaken,tblMaInterviewTypeId,IsDeleted,IsActive")] tblInterviewDetails tblInterviewDetails)
        {
            if (ModelState.IsValid)
            {
                db.tblInterviewDetails.Add(tblInterviewDetails);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InterviewerId = new SelectList(GetUsers(), "Id", "FirstName", tblInterviewDetails.InterviewerId);
            ViewBag.tblMaInterviewFeedbackStatusId = new SelectList(db.tblMaInterviewFeedbackStatus, "Id", "Status", tblInterviewDetails.tblMaInterviewFeedbackStatusId);
            ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type", tblInterviewDetails.tblMaInterviewTypeId);
            return View(tblInterviewDetails);
        }

        // GET: InterviewDetails/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewDetails tblInterviewDetails = await db.tblInterviewDetails.FindAsync(id);
            if (tblInterviewDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.InterviewerId = new SelectList(GetUsers(), "Id", "FirstName", tblInterviewDetails.InterviewerId);
            ViewBag.tblMaInterviewFeedbackStatusId = new SelectList(db.tblMaInterviewFeedbackStatus, "Id", "Status", tblInterviewDetails.tblMaInterviewFeedbackStatusId);
            ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type", tblInterviewDetails.tblMaInterviewTypeId);
            return View(tblInterviewDetails);
        }

        // POST: InterviewDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ScheduleDateTime,InterviewerId,tblMaInterviewFeedbackStatusId,FeedbackDetails,IsInterviewTaken,tblMaInterviewTypeId,IsDeleted,IsActive")] tblInterviewDetails tblInterviewDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblInterviewDetails).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.InterviewerId = new SelectList(GetUsers(), "Id", "FirstName", tblInterviewDetails.InterviewerId);
            ViewBag.tblMaInterviewFeedbackStatusId = new SelectList(db.tblMaInterviewFeedbackStatus, "Id", "Status", tblInterviewDetails.tblMaInterviewFeedbackStatusId);
            ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type", tblInterviewDetails.tblMaInterviewTypeId);
            return View(tblInterviewDetails);
        }
        public List<ApplicationUser> GetUsers()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            try
            {
                users = db.Users.ToList();
                users.ForEach(s => s.FirstName = s.FirstName + " " + s.LastName);
                return users;
            }
            catch (Exception)
            {
            }
            return users;
        }

        // GET: InterviewDetails/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewDetails tblInterviewDetails = await db.tblInterviewDetails.FindAsync(id);
            if (tblInterviewDetails == null)
            {
                return HttpNotFound();
            }
            return View(tblInterviewDetails);
        }

        // POST: InterviewDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tblInterviewDetails tblInterviewDetails = await db.tblInterviewDetails.FindAsync(id);
            db.tblInterviewDetails.Remove(tblInterviewDetails);
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
