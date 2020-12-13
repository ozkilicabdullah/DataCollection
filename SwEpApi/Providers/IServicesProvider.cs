namespace SwEpApi.Providers
{
    public interface IServicesProvider<TInterface>
    {
        TInterface GetInstance(string key);
    }
}
