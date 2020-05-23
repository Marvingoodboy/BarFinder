using System.Collections.Generic;

namespace BarFinder.Models
{
    public partial class Institutions
    {
        public List<Feature> Features { get; set; }
    }

    public partial class Feature
    {
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
        public FeatureType Type { get; set; }
    }

    public partial class Geometry
    {
        public List<double> Coordinates { get; set; }
        public GeometryType Type { get; set; }
    }

    public partial class Properties
    {
        public long DatasetId { get; set; }
        public long VersionNumber { get; set; }
        public long ReleaseNumber { get; set; }
        public object RowId { get; set; }
        public Attributes Attributes { get; set; }
    }

    public partial class Attributes
    {
        public string Name { get; set; }
        public long GlobalId { get; set; }
        public IsNetObject IsNetObject { get; set; }
        public string OperatingCompany { get; set; }
        public string TypeObject { get; set; }
        public string AdmArea { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public List<PublicPhone> PublicPhone { get; set; }
        public long SeatsCount { get; set; }
        public IsNetObject SocialPrivileges { get; set; }
    }

    public partial class PublicPhone
    {
        public string PublicPhonePublicPhone { get; set; }
    }

    public enum GeometryType { Point };

    public enum IsNetObject { Да, Нет };

    public enum FeatureType { Feature };
}
