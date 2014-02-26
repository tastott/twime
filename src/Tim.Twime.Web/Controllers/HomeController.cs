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

        public IDictionary<Guid, RideAnalysis> AnalysisStore
        {
            get
            {
                var store = Session["AnalysisStore"] as IDictionary<Guid, RideAnalysis>;
                if (store == null)
                {
                    store = new Dictionary<Guid, RideAnalysis>();
                    Session["AnalysisStore"] = store;
                }

                return store;
            }
        }

        public HomeController(AnalysisService analysisService, UploadService uploadService)
        {
            _analysisService = analysisService;
            _uploadService = uploadService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AllRides()
        {
            var rides = _uploadService.GetAllRides().Values;

            return Json(rides, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadAndAnalyse(RideAnalysisInput input)
        {
            _UploadAndAnalyse(input);

            return RedirectToAction("Analysis");
        }

        public ActionResult _Upload(HttpPostedFileBase file)
        {
            var uploadedFile = new UploadedFile(file.FileName, file.InputStream);
            var guid = _uploadService.UploadGpx(uploadedFile);

            return Json(new { success = true, guid = guid, filename = uploadedFile.Filename });
        }

        public ActionResult Analysis()
        {
            return View();
        }

        private void _UploadAndAnalyse(RideAnalysisInput input)
        {
            var uploadedFile = new UploadedFile(input.File.FileName, input.File.InputStream);
            var guid = _uploadService.UploadGpx(uploadedFile);

            var ride = _uploadService.GetRide(guid);
            var analysisRequest = new RideAnalysisRequest(ride, new Wind(input.WindSpeedMph, input.WindBearingDegrees), input.Mass);

            var analysis = _analysisService.Analyse(analysisRequest);
            AnalysisStore[guid] = analysis;
        }

        public JsonResult GetAnalysis(Guid id)
        {
            var analysis = AnalysisStore[id];

            return Json(analysis, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetAnalysis(Guid id)
        {
            var analysis = AnalysisStore[id];
            ViewBag.AnalysisId = id;

            return PartialView("_RideAnalysis", analysis);
        }
    }
}
