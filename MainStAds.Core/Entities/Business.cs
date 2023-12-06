using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainStAds.Core.Entities
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Link { get; set; }
        public string ImageType { get; set; }

        public byte[] ImageData { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
