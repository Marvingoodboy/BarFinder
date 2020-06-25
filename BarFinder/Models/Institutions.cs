using System.Collections.Generic;

namespace BarFinder.Models
{

    public partial class Institutions
    {
        public long GlobalId { get; set; }
        public string LatitudeWgs84 { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsNetObject { get; set; }
        public string TypeObject { get; set; }
        public string AdmArea { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public List<PublicPhone> PublicPhone { get; set; }
        public long SeatsCount { get; set; }
        public string SocialPrivileges { get; set; }
        public string LongitudeWgs84 { get; set; }
        public GeoData GeoData { get; set; }
        public string OperatingCompany { get; set; }
    }

    public partial class GeoData
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public partial class PublicPhone
    {
        public string PublicPhonePublicPhone { get; set; }
    }
}

