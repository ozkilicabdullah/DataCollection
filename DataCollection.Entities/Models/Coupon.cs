namespace DataCollection.Entities.Models
{
    public class Coupon
    {
        public string CouponName { get; set; }
        public string ExpiryDate { get; set; }
        public CalculationType CalculationType { get; set; }
        public double Discount { get; set; }
    }
}
