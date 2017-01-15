using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNAuthentication.Controllers
{
    
    public class HomeController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        public HomeController()
        {
        }

        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            
            //var user = (Models.ApplicationUser)Session["userObject"];
            return View(user);
        }

        public ActionResult About()
        {
            var underCons = ConfigurationManager.AppSettings["websiteUnderConstruction"]; 
            if(Convert.ToBoolean(underCons) == true)
            {
                return View("UnderConst");
            }
            var siteAddress = ConfigurationManager.AppSettings["siteAddress"];
            return Redirect(siteAddress);
            //ViewBag.Message = "Your application description page.";

            //return View();
        }

        public ActionResult Disclaimer()
        {
            //return Redirect("Privacy");
            return View("Privacy");
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult FAQ()
        {
            ViewBag.Message = "Frequently asked Questions";

            return View();
        }

        public JsonResult UpdateTerms(string termsType)
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            switch (termsType)
            {
                case "surya":
                    user.SuryaTermsAccept = true;
                    break;
                case "goldenlotus":
                    user.GoldenLotusTermsAccept = true;
                    break;
                case "earthpeace":
                    user.EarthPeceTermsAccept = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(termsType);
                    
            }
            db.SaveChanges();
            return new JsonResult() { Data = new { success = true }, ContentType = "application/json", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return Json(new { success = true });
        }

        public ActionResult Download(string file)
        {
            var path = Server.MapPath("~/videos/");
            var filePath = path + file;
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            
            var response = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = file
            };
            
            return response;
        }

    }
}