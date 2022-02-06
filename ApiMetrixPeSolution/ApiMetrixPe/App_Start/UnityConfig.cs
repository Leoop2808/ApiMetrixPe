using MetrixPeBusiness.Contrato;
using MetrixPeBusiness.Implementacion;
using MetrixPeDataAccess.Contrato;
using MetrixPeDataAccess.Implementacion;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace ApiMetrixPe
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            #region Business
            container.RegisterType<IPruebaBO, PruebaBO>();
            #endregion

            #region DataAccess
            container.RegisterType<IPruebaDO, PruebaDO>();
            #endregion

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}