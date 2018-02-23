using HRMS.DL.AccountModels;
using HRMS.DL.Entities;
using HRMS.DL.Helpers;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TSMind.PB.CL.Repositories;

namespace HRMS.Controllers
{
    [Authorize]
    public class CertificatesController : Controller
    { 
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult IDCards()
        {
            return cards("ID Cards");
        }

        [HttpGet]
        public ActionResult YearCompletionCertificates()
        {
            return cards("Year Completion Certificates");
        }

        [HttpGet]
        public ActionResult SpotAwards()
        {
            return cards("Spot Awards");
        }

        [HttpGet]
        public ActionResult QuarterlyAwards()
        {
            return cards("Quarterly Awards");
        }

        [HttpGet]
        public ActionResult AnnualAwards()
        {
            return cards("Annual Awards");
        }
        [HttpGet]
        public ActionResult WhateverItTakesAward()
        {
            return cards("Whatever It Takes Award");
        }
        [HttpGet]
        public ActionResult BirthdayMail()
        {
            return cards("Birthday Mail");
        }
        [HttpGet]
        public ActionResult NewBornBabyMail()
        {
            return cards("New Born Baby Mail");
        }
        [HttpGet]
        public ActionResult WeddingMail()
        {
            return cards("Wedding Mail");
        }

        [HttpGet]
        public ActionResult cards(string certificateType)
        {
            CertificateViewModel _CertificateViewModel = new CertificateViewModel();

            _CertificateViewModel.CardTitle = certificateType;
            _CertificateViewModel.tblMaCompanyBranchs = db.tblMaCompanyBranch.ToList();
            _CertificateViewModel.ApplicationUser = db.Users.ToList();
            _CertificateViewModel.baseUrl = HttpContext.Request.Url.ToString().Replace(HttpContext.Request.Url.LocalPath == "/" ? "Ignore" : HttpContext.Request.Url.LocalPath, "");

            //retirve certificate id
            var mainCertificate = db.tblMaCertificateTypes.Where(s=>s.CertificateName == certificateType).FirstOrDefault();

            if (mainCertificate != null)
            {
                //retirve sub certificates
                var subCertificates = db.tblMaCertificateSubTypes.Where(s => s.tblMaCertificateTypesId == mainCertificate.Id).ToList();

                if (subCertificates != null && subCertificates.Count > 0)
                {
                    //save all subcertificates in viewbag
                    var data = new SelectList(subCertificates, "CertificateTemplateUrl", "CertificateName");
                    var a=data.Count();//.FirstOrDefault().Value;

                    ViewBag.subCertificates = new SelectList(subCertificates, "CertificateTemplateUrl", "CertificateName");

                    //string path = Server.MapPath("~/Content/Templates/YCC Certificates/");
                    //string path = HttpContext.Request.Url.ToString().Replace(HttpContext.Request.Url.LocalPath == "/" ? "Ignore" : HttpContext.Request.Url.LocalPath, "");
                    string path = "/Content/Templates/YCC Certificates/";
                    string imgPrefix = "p";
                    int i = 1;
                    string extention = ".jpg"; 

                    ViewBag.CompletedYear = new SelectList(
    new List<SelectListItem>
    {
        new SelectListItem { Text = "1", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "2", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "3", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "4", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "5", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "6", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "7", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "8", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "9", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "10", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "11", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "12", Value = Path.Combine(path,imgPrefix+i+++extention)},
        new SelectListItem { Text = "13", Value = Path.Combine(path,imgPrefix+i+++extention)},
    }, "Value", "Text");

                    //set default fields data
                    var firstId = subCertificates.First().Id;
                    _CertificateViewModel.CardTemplate = subCertificates.First().CertificateTemplateUrl;

                    var defaultfields = db.tblCertificateInputFieldDetails.Where(s => s.tblMaCertificateSubTypesId == firstId).ToList();
                    _CertificateViewModel.FieldDetails = defaultfields;

                    return View("cards", _CertificateViewModel);

                }
                else
                {
                    //no sub certificates found
                }
            }
            else
            {
                //no main certificate found
            }
            return View("certficates");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult cards(CertificateViewModel _CertificateViewModel, HttpPostedFileBase Photo)
        {
            var result= ReturnImage(_CertificateViewModel, Photo);
            if (result.File != null)
                return result.File;
            else return RedirectToAction("IDCards");
        }

        public FileCombination ReturnImage(CertificateViewModel _CertificateViewModel, HttpPostedFileBase Photo)
        {
            FileCombination ff = new FileCombination();
            try
            {
                //retirve certificate id
                var mainCertificate = db.tblMaCertificateTypes.Where(s => s.CertificateName == _CertificateViewModel.CardTitle).FirstOrDefault();
                if (mainCertificate != null)
                {
                    //retirve sub certificates
                    string templateFullUrl = _CertificateViewModel.subCertificates;

                    var subCertificates = db.tblMaCertificateSubTypes.Where(s => s.tblMaCertificateTypesId == mainCertificate.Id && s.CertificateTemplateUrl == templateFullUrl).ToList().FirstOrDefault();
                    if (subCertificates != null)
                    {
                        string templateUrl = subCertificates.CertificateTemplateUrl;

                        if (!string.IsNullOrEmpty(_CertificateViewModel.CompletedYear))
                            templateUrl = _CertificateViewModel.CompletedYear;

                        string fullTemplateName = Server.MapPath("~" + templateUrl);
                        var imageFile = Path.Combine(Server.MapPath("~" + templateUrl));

                        using (var srcImage = Image.FromFile(imageFile))
                        {
                            using (Bitmap bmp = new Bitmap(srcImage))
                            {
                                using (Graphics grp = Graphics.FromImage(bmp))
                                {
                                    grp.SmoothingMode = SmoothingMode.AntiAlias;
                                    grp.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    grp.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    var defaultfields = db.tblCertificateInputFieldDetails.Where(s => s.tblMaCertificateSubTypesId == subCertificates.Id).ToList();
                                    var type = _CertificateViewModel.GetType();

                                    foreach (var item in defaultfields)
                                    {
                                        try
                                        {

                                            var property = type.GetProperty(item.tblMaInputFields.UniqueId);
                                            var value = property.GetValue(_CertificateViewModel);
                                            if (value != null)
                                            {
                                                string fieldValue = value.ToString();
                                                string FieldFont = item.tblMaFonts.FontName;
                                                string FieldFontSize = item.FontSize;
                                                FontStyle FieldFontAttribute = GetFontAttribute(item.tblMaFontAttributes.FontAttribute);
                                                Color FieldColor = GetFontColor(item.tblMaColors.ColorName);
                                                int FieldLocationX = item.XLocation;
                                                int FieldLocationY = item.YLocation;
                                                
                                                string[] stringDatatypes = new string[] { "string", "Blood Group", "Month Year", "Year", "Employee Name" };
                                                if (stringDatatypes.Contains(item.tblMaInputFields.Datatype))
                                                {
                                                    Brush brush = new SolidBrush(FieldColor);
                                                    Font font = new System.Drawing.Font(FieldFont, Convert.ToInt16(FieldFontSize), FieldFontAttribute);
                                                    SizeF textSize = new SizeF();
                                                    Point position = new Point(FieldLocationX, FieldLocationY);

                                                    if (!string.IsNullOrEmpty(item.tblMaFieldAlignmentId) && item.tblMaFieldAlignmentId.ToLower() != "none")
                                                    {
                                                        StringFormat format = new StringFormat();
                                                        StringAlignment alignment =
                                                            item.tblMaFieldAlignmentId.ToLower() == "center" ? StringAlignment.Center :
                                                            item.tblMaFieldAlignmentId.ToLower() == "far" ? StringAlignment.Far :
                                                            StringAlignment.Near;
                                                        format.Alignment = alignment;
                                                        textSize = grp.MeasureString(fieldValue, font, position, format);
                                                        grp.DrawString(fieldValue, font, brush, position, format);
                                                    }
                                                    else
                                                    {
                                                        textSize = grp.MeasureString(fieldValue, font, new SizeF(position));
                                                        grp.DrawString(fieldValue, font, brush, position);
                                                    }
                                                    //StringFormat format = new StringFormat();
                                                    //format.Alignment = StringAlignment.Center;
                                                    //format.Alignment = StringAlignment.Far;
                                                    //format.Alignment = StringAlignment.Near;

                                                    //Brush brush = new SolidBrush(FieldColor);
                                                    //Font font = new System.Drawing.Font(FieldFont, Convert.ToInt16(FieldFontSize), FieldFontAttribute);
                                                    //SizeF textSize = new SizeF();
                                                    //Point position = new Point(FieldLocationX, FieldLocationY);
                                                    //textSize = grp.MeasureString(fieldValue, font,position,format);
                                                    //grp.DrawString(fieldValue, font, brush, position, format);
                                                     

                                                }
                                                else if (item.tblMaInputFields.Datatype == "Photo")
                                                {
                                                    using (var photoImage = Image.FromStream(Photo.InputStream))
                                                    {
                                                        string[] allSizes = FieldFontSize.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                                                        grp.DrawImage(photoImage, new Rectangle(Convert.ToInt32(allSizes[0]), Convert.ToInt32(allSizes[1]), Convert.ToInt32(allSizes[2]), Convert.ToInt32(allSizes[3])));
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        //Save the Watermarked image to the MemoryStream.
                                        bmp.Save(memoryStream, ImageFormat.Png);
                                        memoryStream.Position = 0;
                                        if (_CertificateViewModel.DownloadFormat == "pdf")
                                            _CertificateViewModel.DownloadFormat = "tif";
                                        else if (string.IsNullOrEmpty(_CertificateViewModel.DownloadFormat))
                                        {
                                            _CertificateViewModel.DownloadFormat = "tif";
                                        }
                                        FileContentResult f = File(memoryStream.ToArray(), "image/png", string.IsNullOrEmpty(_CertificateViewModel.EmployeeName)?"Card": 
                                            _CertificateViewModel.EmployeeName.Replace(" ","") + "."+ _CertificateViewModel.DownloadFormat + "");

                                        string filename = "Priview" + Guid.NewGuid() + ".jpg";
                                        string path = Server.MapPath("~/Content/Priview/");
                                        path = Path.Combine(path, filename);
                                        bmp.Save(path, ImageFormat.Jpeg);

                                        string basePath = HttpContext.Request.Url.ToString().Replace(HttpContext.Request.Url.LocalPath == "/" ? "Ignore" : HttpContext.Request.Url.LocalPath, "");
                                        path = basePath+ "/Content/Priview/" + filename;

                                        ff.File = f;
                                        ff.FilePath = path;
                                        return ff;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
         
        [HttpPost]
        public ActionResult templatePreview(CertificateViewModel _CertificateViewMode, HttpPostedFileBase Photo)
        {
            try
            {
                try
                {
                    HttpPostedFileBase photo = Request.Files["Photo"];

                    if (photo != null && photo.ContentLength > 0)
                    {
                        //var fileName = Path.GetFileName(photo.FileName);
                        //photo.SaveAs(Path.Combine(directory, fileName));
                    }
                }
                catch (Exception) { }

                var result = ReturnImage(_CertificateViewMode, null);
                return Json(new
                {
                    msg = result.FilePath
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    msg = ex.Message
                });
            } 
        }

        public void generatePdf()
        {
            try
            {
                //destination
                string filename = "Priview" + Guid.NewGuid() + ".pdf";

                string path = Server.MapPath("~/Content/Priview");
                path = Path.Combine(path, filename);

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Size = PdfSharp.PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                //source
                string templateDir = Server.MapPath("~/Content/Templates/Annual Awards");
                string imagePath = Path.Combine(templateDir, "Champion_AA_2017.tif");
                XImage image = XImage.FromFile(imagePath);
                gfx.DrawImage(image, 0, 0);
                document.Save(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }


        }

        private FontStyle GetFontAttribute(string fontAttribute)
        {
            try
            {
                switch (fontAttribute)
                {
                    case "Bold":
                        return FontStyle.Bold;
                    case "Regular":
                        return FontStyle.Regular;
                    case "Italic":
                        return FontStyle.Italic;
                    case "Underline":
                        return FontStyle.Underline;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return FontStyle.Regular;
        }

        private Color GetFontColor(string color)
        {
            try
            {
                switch (color)
                {
                    case "Black":
                        return Color.Black;
                    case "Red":
                        return Color.Red;
                    case "Yellow":
                        return Color.Yellow;
                    case "Gray":
                        return Color.Gray;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return Color.Black;
        }
    }

    public class FileCombination
    {
        public string FilePath { get; set; }
        public FileContentResult File { get; set; }
    }
}