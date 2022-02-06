using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MetrixPeDataAccess.Models
{
    public partial class usuario : IUser
    {
        [NotMapped]
        public string Id
        {
            get
            {
                return cod_usuario;
            }
        }
        [NotMapped]
        public string UserName
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<usuario> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
