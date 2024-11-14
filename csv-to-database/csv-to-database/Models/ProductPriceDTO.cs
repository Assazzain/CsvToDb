namespace csv_to_database.Models
{
    public class ProductPriceDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTimeOffset PriceDate { get; set; }
        public double BasePrice { get; set; }
        public int? RpaId { get; set; }
        public double? PriceMonth1 { get; set; }
        public double? PriceMonth2 { get; set; }
        public double? PriceMonth3 { get; set; }
        public double? ChangeMonth1 { get; set; }
        public double? ChangeMonth2 { get; set; }
        public double? ChangeMonth3 { get; set; }
        public byte Choice { get; set; } // 1 or 2
        public string ConcurrencyStamp { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
