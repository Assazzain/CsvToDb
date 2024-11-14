namespace csv_to_database.Models
{
    public class BasePriceCSV
    {
        public DateTime Date { get; set; }
        public double MBP { get; set; }
        public double MBB { get; set; }
        public double CPAP { get; set; }
        public double CPAB { get; set; }
        public double AFP { get; set; }
        public double AFB { get; set; }
    }
}
