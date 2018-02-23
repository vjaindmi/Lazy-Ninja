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
    public class inputdetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: inputdetails
        public async Task<ActionResult> Index()
        {
            var tblCertificateInputFieldDetails = db.tblCertificateInputFieldDetails.Include(t => t.tblMaCertificateSubTypes).Include(t => t.tblMaColors).Include(t => t.tblMaFontAttributes).Include(t => t.tblMaFonts).Include(t => t.tblMaInputFields);
            return PartialView(await tblCertificateInputFieldDetails.OrderBy(s=>s.tblMaCertificateSubTypesId).ToListAsync());
        }

        // GET: inputdetails/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCertificateInputFieldDetails tblCertificateInputFieldDetails = await db.tblCertificateInputFieldDetails.FindAsync(id);
            if (tblCertificateInputFieldDetails == null)
            {
                return HttpNotFound();
            }
            return View(tblCertificateInputFieldDetails);
        }

        // GET: inputdetails/Create
        public ActionResult Create()
        {
            var subCertificateList = db.tblMaCertificateSubTypes.ToList();
            foreach (var item in subCertificateList)
                item.CertificateName = item.CertificateName + " - " + item.tblMaCertificateTypes.CertificateName;

            ViewBag.tblMaCertificateSubTypesId = new SelectList(subCertificateList, "Id", "CertificateName");
            ViewBag.Color = new SelectList(db.tblMaColors, "Id", "ColorName");
            ViewBag.FontAttribute = new SelectList(db.tblMaFontAttributes, "Id", "FontAttribute");
            ViewBag.Font = new SelectList(db.tblMaFonts, "Id", "FontName");
            ViewBag.tblMaInputFieldsId = new SelectList(db.tblMaInputFields, "Id", "Field");
            return View();
        }

        // POST: inputdetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,tblMaCertificateSubTypesId,tblMaInputFieldsId,XLocation,YLocation,Color,Font,FontSize,FontAttribute,IsDeleted,IsActive")] tblCertificateInputFieldDetails tblCertificateInputFieldDetails)
        {
            if (ModelState.IsValid)
            {
                db.tblCertificateInputFieldDetails.Add(tblCertificateInputFieldDetails);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.tblMaCertificateSubTypesId = new SelectList(db.tblMaCertificateSubTypes, "Id", "CertificateName", tblCertificateInputFieldDetails.tblMaCertificateSubTypesId);
            ViewBag.Color = new SelectList(db.tblMaColors, "Id", "ColorName", tblCertificateInputFieldDetails.Color);
            ViewBag.FontAttribute = new SelectList(db.tblMaFontAttributes, "Id", "FontAttribute", tblCertificateInputFieldDetails.FontAttribute);
            ViewBag.Font = new SelectList(db.tblMaFonts, "Id", "FontName", tblCertificateInputFieldDetails.Font);
            ViewBag.tblMaInputFieldsId = new SelectList(db.tblMaInputFields, "Id", "Field", tblCertificateInputFieldDetails.tblMaInputFieldsId);
            return View(tblCertificateInputFieldDetails);
        }

        // GET: inputdetails/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCertificateInputFieldDetails tblCertificateInputFieldDetails = await db.tblCertificateInputFieldDetails.FindAsync(id);
            if (tblCertificateInputFieldDetails == null)
            {
                return HttpNotFound();
            }
            var subCertificateList = db.tblMaCertificateSubTypes.ToList();
            foreach (var item in subCertificateList)
                item.CertificateName = item.CertificateName + " - " + item.tblMaCertificateTypes.CertificateName;

            ViewBag.tblMaCertificateSubTypesId = new SelectList(subCertificateList, "Id", "CertificateName", tblCertificateInputFieldDetails.tblMaCertificateSubTypesId);
            ViewBag.Color = new SelectList(db.tblMaColors, "Id", "ColorName", tblCertificateInputFieldDetails.Color);
            ViewBag.FontAttribute = new SelectList(db.tblMaFontAttributes, "Id", "FontAttribute", tblCertificateInputFieldDetails.FontAttribute);
            ViewBag.Font = new SelectList(db.tblMaFonts, "Id", "FontName", tblCertificateInputFieldDetails.Font);
            ViewBag.tblMaInputFieldsId = new SelectList(db.tblMaInputFields, "Id", "Field", tblCertificateInputFieldDetails.tblMaInputFieldsId);
            return PartialView(tblCertificateInputFieldDetails);
        }

        // POST: inputdetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblCertificateInputFieldDetails tblCertificateInputFieldDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCertificateInputFieldDetails).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.tblMaCertificateSubTypesId = new SelectList(db.tblMaCertificateSubTypes, "Id", "CertificateName", tblCertificateInputFieldDetails.tblMaCertificateSubTypesId);
            ViewBag.Color = new SelectList(db.tblMaColors, "Id", "ColorName", tblCertificateInputFieldDetails.Color);
            ViewBag.FontAttribute = new SelectList(db.tblMaFontAttributes, "Id", "FontAttribute", tblCertificateInputFieldDetails.FontAttribute);
            ViewBag.Font = new SelectList(db.tblMaFonts, "Id", "FontName", tblCertificateInputFieldDetails.Font);
            ViewBag.tblMaInputFieldsId = new SelectList(db.tblMaInputFields, "Id", "Field", tblCertificateInputFieldDetails.tblMaInputFieldsId);
            return PartialView(tblCertificateInputFieldDetails);
        }

        // GET: inputdetails/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCertificateInputFieldDetails tblCertificateInputFieldDetails = await db.tblCertificateInputFieldDetails.FindAsync(id);
            if (tblCertificateInputFieldDetails == null)
            {
                return HttpNotFound();
            }
            return View(tblCertificateInputFieldDetails);
        }

        // POST: inputdetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tblCertificateInputFieldDetails tblCertificateInputFieldDetails = await db.tblCertificateInputFieldDetails.FindAsync(id);
            db.tblCertificateInputFieldDetails.Remove(tblCertificateInputFieldDetails);
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
