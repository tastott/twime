using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tim.Twime.Web.Controllers
{
    using Services;
    using Models;
    using ViewModels;

    public class HomeController : Controller
    {
        private AnalysisService _analysisService;
        private UploadService _uploadService;

        public HomeController(AnalysisService analysisService, UploadService uploadService)
        {
            _analysisService = analysisService;
            _uploadService = uploadService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(RideAnalysisInput input)
        {
            var uploadedFile = new UploadedFile(input.File.FileName, input.File.InputStream);
            var guid = _uploadService.UploadGpx(uploadedFile);

            var ride = _uploadService.RetrieveGpx(guid);
            var analysisRequest = new RideAnalysisRequest(ride, input.WindSpeedMph, input.WindBearingDegrees, input.Mass);
            var analysis = _analysisService.Analyse(analysisRequest);

            ViewBag.Thingy = analysis.Thingy;

            return View("Index");
        }
    }
}
