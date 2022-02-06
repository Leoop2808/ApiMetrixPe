using MetrixPeDataAccess.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Win32.SafeHandles;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ApiMetrixPe.Providers
{
    public class UserStoreApiMetrixPe :
    IUserStore<usuario>, IUserPasswordStore<usuario>,
    IUserSecurityStampStore<usuario>, IUserEmailStore<usuario>
    {
        readonly MetrixPeBDEntities context = new MetrixPeBDEntities();
        bool disposed = false;
        readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);


        public Task CreateAsync(usuario user)
        {
            user.fecha_creacion = DateTime.Now;
            user.fecha_modificacion = DateTime.Now;
            context.usuario.Add(user);
            return context.SaveChangesAsync();
        }


        public Task DeleteAsync(usuario user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }
        public Task<usuario> FindByIdAsync(string userId)
        {
            Task<usuario> task =
            context.usuario.Where(apu => apu.cod_usuario == userId).FirstOrDefaultAsync();

            return task;
        }

        public Task<usuario> FindByNameAsync(string userName)
        {
            Task<usuario> task =
            context.usuario.Where(apu => apu.username == userName)
                           .FirstOrDefaultAsync();

            return task;
        }

        public Task<string> GetPasswordHashAsync(usuario user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.password);
        }

        public Task<string> GetSecurityStampAsync(usuario user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.username);

            //return Task.FromResult(user.API_SECURITYSTAMP);
        }

        public Task<bool> HasPasswordAsync(usuario user)
        {
            return Task.FromResult(user.password != null);
        }

        public Task SetPasswordHashAsync(usuario user, string passwordHash)
        {
            user.password = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetSecurityStampAsync(usuario user, string stamp)
        {
            //user.API_SECURITYSTAMP = stamp;
            return Task.FromResult(0);
        }

        public Task UpdateAsync(usuario user)
        {
            if (context.usuario.Any(x => x.id_usuario == user.id_usuario))
            {
                var usuario = context.usuario.FirstOrDefault(x => x.id_usuario == user.id_usuario);
                usuario.password = user.password;
                return context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException("user");
            }

        }

        public Task SetEmailAsync(usuario user, string email)
        {
            //user.ema = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(usuario user)
        {
            //var persona = context.Persona.FirstOrDefault(x => x.IdPersona == user.IdPersona);
            if (user == null/* || persona == null*/)
                throw new ArgumentNullException("user");

            return Task.FromResult("email@null.com"); //Task.FromResult(String.IsNullOrEmpty(user.correo) ? "email@null.com" : user.correo);
            //return Task.FromResult(String.IsNullOrEmpty(persona.email) ? "email@null.com" : persona.email);
        }

        public Task<bool> GetEmailConfirmedAsync(usuario user)
        {
            if (user == null /*|| user.Persona == null*/)
                throw new ArgumentNullException("user");
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(usuario user, bool confirmed)
        {
            return Task.FromResult(0);
        }

        public Task<usuario> FindByEmailAsync(string email)
        {
            Task<usuario> task = null;
            //context.usuario.Where(apu => apu.correo == email).FirstOrDefaultAsync();

            return task;
        }
    }
}