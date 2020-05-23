namespace BarFinder.Models
{
    public partial class MyLocation
    {
        public long Result { get; set; }
        public Data Data { get; set; }
    }

    public partial class Data
    {
        public double Lat { get; set; }
        public double Range { get; set; }
        public double Lon { get; set; }
        public long Time { get; set; }
    }
}
