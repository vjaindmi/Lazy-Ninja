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
using HRMS.DL.ViewModels;
using Microsoft.AspNet.Identity;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;

namespace HRMS.Controllers
{
    [Authorize]
    public class InterviewMasterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InterviewMaster
        public async Task<ActionResult> Index()
        {
            var tblInterviewMasters = db.tblInterviewMasters.Include(t => t.tblMaInterviewNoticePeriod).Include(t => t.tblMaInterviewResult).Include(t => t.tblMaInterviewTechnology);
            return View(await tblInterviewMasters.OrderByDescending(s=>s.Id).ToListAsync());
        }

        // GET: InterviewMaster/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewMaster tblInterviewMaster = await db.tblInterviewMasters.FindAsync(id);
            if (tblInterviewMaster == null)
            {
                return HttpNotFound();
            }
            return View(tblInterviewMaster);
        }

        // GET: InterviewMaster/Create
        public ActionResult Create()
        {
            var model = new tblInterviewMasterViewModel();
            ViewBag.NoticePeriod = new SelectList(db.tblMaInterviewNoticePeriods, "Id", "NoticePeriod");
            ViewBag.tblMaInterviewResultId = new SelectList(db.tblMaInterviewResults, "Id", "Status");
            ViewBag.tblMaInterviewTechnologyID = new SelectList(db.tblMaInterviewTechnologies, "Id", "Technology");

            var allUsers = GetUsers();
            ViewBag.InterviewerId = new SelectList(allUsers, "Id", "FirstName");
            ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type");

            model.AllUsers = allUsers;
            model.AllBranches = db.tblMaCompanyBranch.ToList();

            return View(model);
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

        // POST: InterviewMaster/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tblInterviewMasterViewModel tblInterviewMasterViewModel, HttpPostedFileBase ResumePath)
        {
            try
            {
                //save in master
                var master = new tblInterviewMaster()
                {
                    CurrentCTC = tblInterviewMasterViewModel.CurrentCTC,
                    IsDeleted = false,
                    IsActive = true,
                    CurrentLocation = tblInterviewMasterViewModel.CurrentLocation,
                    ExpectedCTC = tblInterviewMasterViewModel.ExpectedCTC,
                    NoticePeriod = tblInterviewMasterViewModel.NoticePeriod,
                    OtherTechnology = tblInterviewMasterViewModel.OtherTechnology,
                    ResumePath = string.Empty,
                    StudentName = tblInterviewMasterViewModel.StudentName,
                    tblMaInterviewTechnologyID = tblInterviewMasterViewModel.tblMaInterviewTechnologyID,
                    tblMaInterviewResultId = 1,
                    TotalExperience = tblInterviewMasterViewModel.TotalExperience,

                    WhoScheduledId = GetUserId(),
                    ScheduledDateTime= tblInterviewMasterViewModel.ScheduleDateTime.AddDays(-1),
                    EmailId=tblInterviewMasterViewModel.EmailId,
                    PhoneNumber = tblInterviewMasterViewModel.PhoneNumber
                };

                db.tblInterviewMasters.Add(master);
                await db.SaveChangesAsync();

                //update resume path
                string finlePath = string.Empty;
                if (ResumePath != null)
                {
                    string resumePath = Server.MapPath( @"\Content\Resume\");
                    string[] ext = ResumePath.FileName.Split('.');
                    string extt = ext[(ext.Count() - 1)];
                    string fileName = master.StudentName.Replace(" ","_")+ "_Resume_" + master.Id+ "." + extt;
                    finlePath = resumePath + fileName;
                    ResumePath.SaveAs(finlePath);

                    master.ResumePath = @"\Content\Resume\"+ fileName;
                    db.Entry(master).State = EntityState.Modified;
                    db.SaveChanges();
                }

                //save in details
                var details = new tblInterviewDetails()
                {
                    InterviewerId = tblInterviewMasterViewModel.InterviewerId,
                    IsActive = true,
                    IsDeleted = false,
                    IsInterviewTaken = false,
                    ScheduleDateTime = tblInterviewMasterViewModel.ScheduleDateTime,
                    tblInterviewMasterId = master.Id,
                    tblMaInterviewFeedbackStatusId = 4,
                    tblMaInterviewTypeId = tblInterviewMasterViewModel.tblMaInterviewTypeId,
                };
                db.tblInterviewDetails.Add(details);
                await db.SaveChangesAsync();

                //save in notification
                tblMaNotifications notifications = new tblMaNotifications() {
                    Details = "New interview has been scheduled to you by "+GetUserName()+". click here for more details...",
                    DetailsId = details.Id,
                     IsActive=true,
                     IsDeleted=false,
                      IsOpened=false,
                       NotificationType="InterviewScheduled",
                        UserId= tblInterviewMasterViewModel.InterviewerId,
                    SenderName = GetUserName(),
                    DateTime = DateTime.Now
                };
                db.tblMaNotifications.Add(notifications);
                await db.SaveChangesAsync();

                TempData["Y"] = "Interview has been scheduled successfully and Email/Notification has been sent to respective interviewer.";

                //send mail
                var userDetails = db.Users.Where(s=>s.Id== tblInterviewMasterViewModel.InterviewerId).ToList().FirstOrDefault();
                var technolgyDetails = db.tblMaInterviewTechnologies.Where(s=>s.Id== tblInterviewMasterViewModel.tblMaInterviewTechnologyID).FirstOrDefault();
                if (userDetails != null)
                {
                    Appointment appointment = new Appointment();
                    appointment.Name = "Inteview Schedule for " + technolgyDetails.Technology;
                    appointment.Location = "DMI Pune";
                    appointment.Subject = "Inteview Schedule for " + technolgyDetails.Technology;
                    appointment.StartDate = tblInterviewMasterViewModel.ScheduleDateTime;
                    appointment.EndDate = tblInterviewMasterViewModel.ScheduleDateTime.AddHours(2);
                    appointment.Email = "tjagdale@dminc.com";
                    //appointment.Email = userDetails.Email;
                    appointment.Attachment = finlePath;
                    StringBuilder body = new StringBuilder();
                    body.AppendFormat("Hi {0}", userDetails.FirstName);
                    appointment.Body = "Hi " + userDetails.FirstName;

                    sendOutlookInvitationViaICSFile(appointment);
                }
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                ViewBag.NoticePeriod = new SelectList(db.tblMaInterviewNoticePeriods, "Id", "NoticePeriod", tblInterviewMasterViewModel.tblInterviewMaster.NoticePeriod);
                ViewBag.tblMaInterviewResultId = new SelectList(db.tblMaInterviewResults, "Id", "Status", tblInterviewMasterViewModel.tblInterviewMaster.tblMaInterviewResultId);
                ViewBag.tblMaInterviewTechnologyID = new SelectList(db.tblMaInterviewTechnologies, "Id", "Technology", tblInterviewMasterViewModel.tblInterviewMaster.tblMaInterviewTechnologyID);

                ViewBag.InterviewerId = new SelectList(GetUsers(), "Id", "FirstName",tblInterviewMasterViewModel.InterviewerId);
                ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type",tblInterviewMasterViewModel.tblMaInterviewTypeId);
                TempData["N"] = ex.Message;
                return View(tblInterviewMasterViewModel);
            }
        }

        public string sendOutlookInvitationViaICSFile(Appointment objApptEmail)
        {
            try
            {

                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                SmtpClient sc = new SmtpClient("smtp.gmail.com");

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

                msg.From = new MailAddress(settings.Smtp.Network.UserName);
                msg.To.Add(new MailAddress(objApptEmail.Email, objApptEmail.Name));
                msg.Subject = objApptEmail.Subject;
                msg.IsBodyHtml = true;
                msg.Body = objApptEmail.Body;
                //try
                //{
                //    if (!string.IsNullOrEmpty(objApptEmail.Attachment))
                //        msg.Attachments.Add(new Attachment(objApptEmail.Attachment));
                //}
                //catch (Exception ex)
                //{ }

                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");
                str.AppendLine("PRODID:-//" + "http://www.dmi-hrms.com");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");
                str.AppendLine("BEGIN:VEVENT");

                str.AppendLine(string.Format("UID:http://www.dmi-hrms.com/event/{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", objApptEmail.StartDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", objApptEmail.EndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z")));
                //str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", objApptEmail.StartDate.ToString()));
                //str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                //str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", objApptEmail.EndDate.ToString()));
                str.AppendLine("LOCATION:" + objApptEmail.Location);
                str.AppendLine(string.Format("DESCRIPTION:{0}", objApptEmail.Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", objApptEmail.Body));
                str.AppendLine(string.Format("SUMMARY:{0}", objApptEmail.Subject));
                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", objApptEmail.Email));

                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));
                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");

                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
                ct.Parameters.Add("method", "REQUEST");
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
                msg.AlternateViews.Add(avCal);
                NetworkCredential nc = new NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
                sc.Port = settings.Smtp.Network.Port;
                sc.EnableSsl = true;
                sc.Credentials = nc;
                try
                {
                    sc.Send(msg);
                    return "Success";
                }
                catch
                {
                    return "Fail";
                }
            }
            catch { }
            return string.Empty;
        }

        // GET: InterviewMaster/Edit/5


        public FileResult DownloadResume(string path)
        {
            var finlePath = Server.MapPath(path);
            byte[] fileBytes = System.IO.File.ReadAllBytes(finlePath);
            string[] allPath = path.Split('/');
            string fileName = allPath.LastOrDefault();
            string contentType = "application/pdf";
            string[] ext = path.Split('.');
            string extt = ext[(ext.Count() - 1)];if (extt == "doc")
            contentType = "application/doc";
            else if (extt == "docx")
                contentType = "application/docx";
            return File(fileBytes, contentType, fileName);

            //var finlePath = Server.MapPath(path);

            //string[] ext = path.Split('.');
            //string extt = ext[(ext.Count() - 1)];

            //string contentType = "application/pdf";

            //if (extt == "doc")
            //    contentType = "application/doc";
            //else if (extt == "docx")
            //    contentType = "application/docx";

            //return new FilePathResult(finlePath, contentType);
            ////FilePathResult f = File(finlePath, contentType);// "image/png", string.IsNullOrEmpty(_CertificateViewModel.EmployeeName)?"Card": _CertificateViewModel.EmployeeName.Replace(" ","") + ".tif");
            ////return f;
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewMaster tblInterviewMaster = await db.tblInterviewMasters.FindAsync(id);
            if (tblInterviewMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.NoticePeriod = new SelectList(db.tblMaInterviewNoticePeriods, "Id", "NoticePeriod", tblInterviewMaster.NoticePeriod);
            ViewBag.tblMaInterviewResultId = new SelectList(db.tblMaInterviewResults, "Id", "Status", tblInterviewMaster.tblMaInterviewResultId);
            ViewBag.tblMaInterviewTechnologyID = new SelectList(db.tblMaInterviewTechnologies, "Id", "Technology", tblInterviewMaster.tblMaInterviewTechnologyID);
             
            return View(tblInterviewMaster);
        }

        // POST: InterviewMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblInterviewMaster tblInterviewMaster)
        {
            try
            {
                db.Entry(tblInterviewMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();

                TempData["Y"] = "Interview details updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["N"] = ex.Message;
                ViewBag.NoticePeriod = new SelectList(db.tblMaInterviewNoticePeriods, "Id", "NoticePeriod", tblInterviewMaster.NoticePeriod);
                ViewBag.tblMaInterviewResultId = new SelectList(db.tblMaInterviewResults, "Id", "Status", tblInterviewMaster.tblMaInterviewResultId);
                ViewBag.tblMaInterviewTechnologyID = new SelectList(db.tblMaInterviewTechnologies, "Id", "Technology", tblInterviewMaster.tblMaInterviewTechnologyID);
                return View(tblInterviewMaster);
            }
        }

        // GET: InterviewMaster/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblInterviewMaster tblInterviewMaster = await db.tblInterviewMasters.FindAsync(id);
            if (tblInterviewMaster == null)
            {
                return HttpNotFound();
            }
            return View(tblInterviewMaster);
        }

        // POST: InterviewMaster/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            tblInterviewMaster tblInterviewMaster = await db.tblInterviewMasters.FindAsync(id);
            db.tblInterviewMasters.Remove(tblInterviewMaster);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: InterviewMaster/Delete/5
        public async Task<ActionResult> SubmitFeedback()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var allPendingInterviews = await db.tblInterviewDetails.Where(s => !s.IsInterviewTaken && s.InterviewerId.Equals(userId)).ToListAsync();

                if (allPendingInterviews == null || allPendingInterviews.Count() == 0)
                    TempData["W"] = "No any interview has assigned to you.";
                else
                {
                    List<InterviewedStudent> interviewed = new List<InterviewedStudent>();
                    var allList = db.tblInterviewMasters.ToList();
                    foreach (var item in allList)
                    {
                        var idd = item.Id;
                        var count = allPendingInterviews.Where(s => s.tblInterviewMasterId == idd).ToList().FirstOrDefault();
                        if (count!=null)
                        {
                            string combinedName = item.StudentName + " for " + item.tblMaInterviewTechnology.Technology;
                            Int64 Id = count.Id;
                            var data = new InterviewedStudent()
                            {
                                Name = combinedName,
                                Id = Id
                            };
                            interviewed.Add(data);

                            
                        }
                    }
                    ViewBag.InterviewedStudentName = new SelectList(interviewed, "Id", "Name" );
                    ViewBag.tblMaInterviewFeedbackStatusId = new SelectList(db.tblMaInterviewFeedbackStatus.Where(s=>s.Status!= "Yet To Decide"), "Id", "Status");
                    ViewBag.tblMaInterviewTypeId = new SelectList(db.tblMaInterviewTypes, "Id", "Type");
                    ViewBag.tblMaInterviewResultsId = new SelectList(db.tblMaInterviewResults.Where(s=>s.Status!= "Scheduled"), "Id", "Status");

                    //make notification off
                    var notificationDetail = db.tblMaNotifications.Where(s=>s.UserId== userId).ToList();
                    foreach (var item in notificationDetail)
                    { 
                        item.IsOpened = true;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitFeedback(tblInterviewDetails _tblInterviewDetails,Int64 InterviewedStudentName, Int64 tblMaInterviewResultsId,Int16 NextRoundScheduled)
        {
            try
            {
                //update first round
                var interviewDetails = db.tblInterviewDetails.Where(s => s.Id == InterviewedStudentName).FirstOrDefault();
                interviewDetails.FeedbackDetails = _tblInterviewDetails.FeedbackDetails;
                interviewDetails.tblMaInterviewFeedbackStatusId = _tblInterviewDetails.tblMaInterviewFeedbackStatusId;
                interviewDetails.Others = _tblInterviewDetails.Others;
                interviewDetails.IsInterviewTaken = true;
                db.Entry(interviewDetails).State = EntityState.Modified;
                db.SaveChanges();

                //update feedback in  master table
                var master = db.tblInterviewMasters.Where(s => s.Id == interviewDetails.tblInterviewMasterId).FirstOrDefault();
                master.tblMaInterviewResultId = tblMaInterviewResultsId;
                db.Entry(master).State = EntityState.Modified;
                db.SaveChanges();

                //send notification to hr

                //arrange second round
                if (NextRoundScheduled ==1)
                {
                    var secondRoundDetails = new tblInterviewDetails()
                    {
                        InterviewerId = _tblInterviewDetails.InterviewerId,
                        IsActive = true,
                        IsDeleted = false,
                        IsInterviewTaken = false,
                        ScheduleDateTime = _tblInterviewDetails.ScheduleDateTime,
                        tblInterviewMasterId = master.Id,
                        tblMaInterviewFeedbackStatusId = 4,
                        tblMaInterviewTypeId = _tblInterviewDetails.tblMaInterviewTypeId,
                    };
                    db.tblInterviewDetails.Add(secondRoundDetails);
                    db.SaveChanges();

                    //send notification 
                    //save in notification
                    tblMaNotifications notifications = new tblMaNotifications()
                    {
                        Details = "New interview has been scheduled to you. click here for more details...",
                        DetailsId = secondRoundDetails.Id,
                        IsActive = true,
                        IsDeleted = false,
                        IsOpened = false,
                        NotificationType = "InterviewScheduled",
                        UserId = _tblInterviewDetails.InterviewerId,
                        SenderName = GetUserName(),
                        DateTime = DateTime.Now
                    };
                    db.tblMaNotifications.Add(notifications);
                    await db.SaveChangesAsync();

                    //send mail
                    //send mail
                    var userDetails = db.Users.Where(s => s.Id == _tblInterviewDetails.InterviewerId).ToList().FirstOrDefault();
                    var technolgyDetails = db.tblMaInterviewTechnologies.Where(s => s.Id == master.tblMaInterviewTechnologyID).FirstOrDefault();
                    string resumePath = Server.MapPath( master.ResumePath);
                    if (userDetails != null)
                    {
                        Appointment appointment = new Appointment();
                        appointment.Name = "Inteview Schedule for " + technolgyDetails.Technology;
                        appointment.Location = "DMI Pune";
                        appointment.Subject = "Inteview Schedule for " + technolgyDetails.Technology;
                        appointment.StartDate = _tblInterviewDetails.ScheduleDateTime;
                        appointment.EndDate = _tblInterviewDetails.ScheduleDateTime.AddHours(2);
                        appointment.Email = "tjagdale@dminc.com";
                        //appointment.Email = userDetails.Email;
                        appointment.Attachment = resumePath;
                        StringBuilder body = new StringBuilder();
                        body.AppendFormat("Hi {0}", userDetails.FirstName);

                        appointment.Body = body.ToString();

                        sendOutlookInvitationViaICSFile(appointment);
                    }

                    TempData["Y"] = "Second round of interview has been scheduled successfully and email has been sent to respective interviewer.";
                    return RedirectToAction("SubmitFeedback");
                }
                else
                {
                    TempData["Y"] = "Feedback has been submitted successfully.";
                    return RedirectToAction("SubmitFeedback");
                }                
            }
            catch (Exception ex)
            {
                TempData["N"] = ex.Message;
                return RedirectToAction("SubmitFeedback");
            }
        }
        public string GetUserName()
        {
            try
            {
                string userId = User.Identity.GetUserId(); 
                var userData = db.Users.Where(s => s.Id.Equals(userId)).FirstOrDefault();
                return userData.FirstName;
            }
            catch (Exception ex)
            {
            }
            return "Sush";
        }

        public string GetUserId()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                return userId;
            }
            catch (Exception ex)
            {
            }
            return "Sush";
        }

        public class InterviewedStudent
        {
            public Int64 Id { get; set; }
            public string Name { get; set; }
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

    public class Appointment
    {
        public string Attachment { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Location { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Subject { set; get; }
        public string Body { set; get; }
    }

}
