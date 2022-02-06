using log4net;
using MetrixPeBusiness.Contrato;
using System.Web.Http;

namespace ApiMetrixPe.Controllers
{
    [RoutePrefix("api/prueba")]
    public class PruebaController : ApiController
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PruebaController));
        private readonly IPruebaBO _pruebaBO;
        public PruebaController(IPruebaBO pruebaBO)
        {
            _pruebaBO = pruebaBO;
        }
    }
}