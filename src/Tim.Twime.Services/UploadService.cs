using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tim.Twime.Services
{
    using ImportExport;
    using Models;

    public class UploadService
    {
        private IDictionary<Guid, Ride> _rides;
        private Importer _importer;

        public UploadService(Importer importer)
        {
            _importer = importer;
            _rides = new Dictionary<Guid, Ride>();
        }

        public Ride UploadGpx(UploadedFile file)
        {
            var ride = new Ride(Guid.NewGuid(), _importer.GetWaypointsFromGpxFile(file.Content).ToArray(), file.Filename);

            _rides[ride.Guid] = ride;

            return ride;
        }

        public Ride GetRide(Guid guid)
        {
            return _rides[guid];
        }

        public IDictionary<Guid, Ride> GetAllRides()
        {
            return _rides;
        }
    }
}
