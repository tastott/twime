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

        public Guid UploadGpx(UploadedFile file)
        {
            var guid = Guid.NewGuid();

            _rides[guid] = new Ride(_importer.GetWaypointsFromGpxFile(file.Content).ToArray(), file.Filename);

            return guid;
        }

        public Ride RetrieveGpx(Guid guid)
        {
            return _rides[guid];
        }
    }
}
