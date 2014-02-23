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
            if (AnalysisStore.Any())
            {
                ViewBag.AnalysisId = AnalysisStore.Keys.First();
            }

            return View();
        }

        public ActionResult Upload(RideAnalysisInput input)
        {
            var uploadedFile = new UploadedFile(input.File.FileName, input.File.InputStream);
            var guid = _uploadService.UploadGpx(uploadedFile);

            var ride = _uploadService.RetrieveGpx(guid);
            var analysisRequest = new RideAnalysisRequest(ride, input.WindSpeedMph, input.WindBearingDegrees, input.Mass);
            
            var analysis = _analysisService.Analyse(analysisRequest);
            var analysisId = Guid.NewGuid();
            AnalysisStore[analysisId] = analysis;

            ViewBag.AnalysisId = analysisId;

            return View("Index");
        }

        public JsonResult GetAnalysis(Guid id)
        {
            var analysis = AnalysisStore[id];

            return Json(analysis);
        }

        public ActionResult GetAnalysisHtml(Guid id)
        {
            var analysis = AnalysisStore[id];
            ViewBag.AnalysisId = id;

            return PartialView("_RideAnalysis", analysis);
        }
    }
}
