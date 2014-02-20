using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace Tim.Twime.DependencyInjection
{
    using Services;
    using Cycling;
    using ImportExport;

    public class TwimeDependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<UploadService>().ToSelf().InSingletonScope();
            Bind<AnalysisService>().ToSelf().InSingletonScope();
            Bind<Importer>().ToSelf();
            Bind<RideAnalyser>().ToSelf();
        }
    }
}
