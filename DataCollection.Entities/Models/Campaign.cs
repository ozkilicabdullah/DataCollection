
namespace DataCollection.Entities
{
    public class Campaign
    {
        public string CampaignName { get; set; }
        public string ExpiryDate { get; set; }
        public CampaignType CampaignType { get; set; }
        public CalculationType CalculationType { get; set; }
        public double Discount { get; set; }
    }

}
