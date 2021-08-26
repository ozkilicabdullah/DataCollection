namespace DataCollection.Entities
{
    public enum PaymentType
    {
        CreditCart = 1,
        Transfer = 2,
        Mobile = 3,
        Paypal = 4

    }

    public enum CampaignType
    {
        cartDiscount = 1

    }

    public enum CalculationType
    {
        cash = 1,
        percent = 2
    }

    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Famale = 2
    }
    public enum ContactType
    {
        Mail = 1,
        Sms = 2,
        Call = 3 
    }
}
