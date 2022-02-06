using log4net;
using MetrixPeDataAccess.Contrato;

namespace MetrixPeDataAccess.Implementacion
{
    public class PruebaDO : IPruebaDO
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PruebaDO));
    }
}
