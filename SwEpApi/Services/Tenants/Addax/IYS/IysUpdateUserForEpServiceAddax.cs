using SwEpApi.Services.Tenants.Base.IYS;
using SwEpApi.Services.Tenants.Base.IYS.RequestParams;

namespace SwEpApi.Services.Tenants.Addax
{
    public class IysUpdateUserForEpServiceAddax : IysUpdateUserForEpServiceBase
    {
        public IysUpdateUserForEpServiceAddax(IConnectionService connectionService) : base(connectionService) { }
    }
}
