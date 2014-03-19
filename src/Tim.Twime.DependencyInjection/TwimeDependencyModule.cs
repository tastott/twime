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
    using Services.Weather;

    public class TwimeDependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<UploadService>().ToSelf().InSingletonScope();
            Bind<AnalysisService>().ToSelf().InSingletonScope();
            Bind<WeatherService>().ToSelf().InSingletonScope();

            Bind<Importer>().ToSelf();
            Bind<RideAnalyser>().ToSelf();
            //Bind<IWeatherDataProvider>().To<MetOfficeWeatherDataProvider>();
            Bind<IWeatherDataProvider>().To<MockWeatherDataProvider>();
        }
    }
}
