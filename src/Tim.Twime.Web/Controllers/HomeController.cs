using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tim.Twime.Web.Controllers
{
    using Services;
    using Models;

    public class HomeController : Controller
    {
        private AnalysisService _analysisService;

        public HomeController()
        {
            _analysisService = new AnalysisService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            var uploadedFile = new UploadedFile(file.FileName, file.InputStream);
            var analysis = _analysisService.UploadAndAnalyse(uploadedFile);

            ViewBag.Thingy = analysis.Thingy;

            return View("Index");
        }
    }
}
