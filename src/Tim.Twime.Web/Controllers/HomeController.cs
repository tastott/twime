using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tim.Twime.Web.Controllers
{
    using Twime.Services;
    using Twime.Services.Weather;
    using Models;
    using ViewModels;

    public class HomeController : Controller
    {
        private AnalysisService _analysisService;
        private UploadService _uploadService;
        private WeatherService _weatherService;

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

        public HomeController(AnalysisService analysisService, 
            UploadService uploadService,
            WeatherService weatherService)
        {
            _analysisService = analysisService;
            _uploadService = uploadService;
            _weatherService = weatherService;
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

        public ActionResult _Upload(HttpPostedFileBase file)
        {
            var uploadedFile = new UploadedFile(file.FileName, file.InputStream);
            var ride = _uploadService.UploadGpx(uploadedFile);
            var windDataTokens = _weatherService.GenerateWindDataRequestsForRide(ride);

            var rideReceipt = new
            {
                success = true,
                guid = ride.Guid,
                filename = uploadedFile.Filename,
                windDataTokens = windDataTokens
            };

            return Json(rideReceipt);
        }

        public ActionResult _GetWindData(Guid id)
        {
            var windDataResult = _weatherService.GetWindData(id);
            string status = windDataResult.Result == AssertionResult.Pass ? "success" : "unknown";

            return Json(new { status = status, observation = windDataResult.Value}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Analysis()
        {
            return View();
        }

        public ActionResult Analyse(RideAnalysisInput input)
        {
            var ride = _uploadService.GetRide(input.RideId);
            var analysisRequest = new RideAnalysisRequest(ride, new Wind(input.WindSpeed, input.WindBearing), 70);

            var analysis = _analysisService.Analyse(analysisRequest);
            AnalysisStore[ride.Guid] = analysis;

            return RedirectToAction("Analysis");
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
