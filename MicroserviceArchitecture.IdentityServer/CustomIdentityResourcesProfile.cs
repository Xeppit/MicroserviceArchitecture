using IdentityModel;
using IdentityServer4.Models;

namespace MicroserviceArchitecture.IdentityServer
{
    public class CustomIdentityResourcesProfile : IdentityResources.Profile
    {
        public CustomIdentityResourcesProfile()
        {
            this.UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
