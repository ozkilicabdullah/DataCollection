namespace DataCollection.Providers
{
    public interface IServicesProvider<TInterface>
    {
        TInterface GetInstance(string key);
    }
}
