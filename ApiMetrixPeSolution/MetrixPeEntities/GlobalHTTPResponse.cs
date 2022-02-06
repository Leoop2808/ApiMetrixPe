using System.Net;

namespace MetrixPeEntities
{
    /// <summary>
    /// La clase no debe ser modifica. Solo puede ser heredada.
    /// </summary>
    public abstract class GlobalHTTPResponse
    {
        public HttpStatusCode codeRes { get; set; }
        public string messageRes { get; set; }
    }
}
