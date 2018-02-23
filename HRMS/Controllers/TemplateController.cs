using HRMS.DL.AccountModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSMind.PB.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        public string TechnologyResports()
        {
           List< JsonValues> var = new List<JsonValues>() {
                // new JsonValues(){ value=10, name="rose1" },
                //new JsonValues(){ value=5, name="rose2" },
                //new JsonValues(){ value=25, name="rose3" },
                //new JsonValues(){ value=20, name="rose4" },
                //new JsonValues(){ value=35, name="rose5" },
            };

            ApplicationDbContext db = new ApplicationDbContext();
            var allTechnology = db.tblMaInterviewTechnologies.ToList();
            var allInterview = db.tblInterviewMasters.ToList();

            foreach (var item in allTechnology)
            {
                var count = allInterview.Where(s=>s.tblMaInterviewTechnologyID==item.Id).ToList().Count();
                var.Add(new JsonValues() {
                    value = count, name=item.Technology
                });
            }
            var json = JsonConvert.SerializeObject(var);
            return json;
        }

        public string InterviewStatusReports()
        {
            List<JsonValues> var = new List<JsonValues>()
            {
            };

            ApplicationDbContext db = new ApplicationDbContext();
            var allKeys = db.tblMaInterviewResults.ToList();
            var allValues = db.tblInterviewMasters.ToList();

            foreach (var item in allKeys)
            {
                var count = allValues.Where(s => s.tblMaInterviewResultId == item.Id).ToList().Count();
                var.Add(new JsonValues()
                {
                    value = count,
                    name = item.Status
                });
            }
            var json = JsonConvert.SerializeObject(var);
            return json;
        }


        public ActionResult Dashboard()
        {
            return View();
        }
 

        public ActionResult Index()
        {
            return View("");
        } 
        public ActionResult calendar()
        {
            return View();
        }
        public ActionResult chartjs()
        {
            return View();
        }
        public ActionResult chartjs2()
        {
            return View();
        }
        public ActionResult contacts()
        {
            return View();
        }
        public ActionResult e_commerce()
        {
            return View();
        }
        public ActionResult echarts()
        {
            return View();
        }
        public ActionResult form()
        {
            return View();
        }
        public ActionResult form_advanced()
        {
            return View();
        }
        public ActionResult form_buttons()
        {
            return View();
        }
        public ActionResult form_upload()
        {
            return View();
        }
        public ActionResult form_validation()
        {
            return View();
        }
        public ActionResult form_wizards()
        {
            return View();
        }
        public ActionResult general_elements()
        {
            return View();
        }
        public ActionResult glyphicons()
        {
            return View();
        }
        public ActionResult icons()
        {
            return View();
        }
        public ActionResult inbox()
        {
            return View();
        }
        public ActionResult index2()
        {
            return View();
        }
        public ActionResult index3()
        {
            return View();
        }
        public ActionResult invoice()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }
        public ActionResult map()
        {
            return View();
        }
        public ActionResult media_gallery()
        {
            return View();
        }
        public ActionResult morisjs()
        {
            return View();
        }
        public ActionResult other_charts()
        {
            return View();
        }
        public ActionResult page_404()
        {
            return View();
        }
        public ActionResult page_500()
        {
            return View();
        }
        public ActionResult plain_page()
        {
            return View();
        }
        public ActionResult pricing_tables()
        {
            return View();
        }
        public ActionResult profile()
        {
            return View();
        }
        public ActionResult project_detail()
        {
            return View();
        }
        public ActionResult projects()
        {
            return View();
        }
        public ActionResult sign_up()
        {
            return View();
        }
        public ActionResult tables()
        {
            return View();
        }
        public ActionResult tables_dynamic()
        {
            return View();
        }
        public ActionResult typography()
        {
            return View();
        }
        public ActionResult widgets()
        {
            return View();
        }
    }

    public class JsonValues
    {
        //value: 10, name: 'rose1'
        public int value { get; set; }
        public string name { get; set; }
    }
}
