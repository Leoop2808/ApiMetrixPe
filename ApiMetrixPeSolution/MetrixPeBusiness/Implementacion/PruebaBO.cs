using log4net;
using MetrixPeBusiness.Contrato;
using MetrixPeDataAccess.Contrato;

namespace MetrixPeBusiness.Implementacion
{
    public class PruebaBO : IPruebaBO
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PruebaBO));
        readonly IPruebaDO _pruebaDO;
        public PruebaBO(IPruebaDO pruebaDO)
        {
            _pruebaDO = pruebaDO;
        }
    }
}
