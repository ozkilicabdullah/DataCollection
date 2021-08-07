using DataCollection.Contracts.Entites;

namespace DataCollection.Contracts
{
    public class BasketParams : ModelActivityBase
    {
        public string ProductId { get; set; }
        public BasketInfo BasketInfo { get; set; }
        public string Type { get; set; }
    }
}
