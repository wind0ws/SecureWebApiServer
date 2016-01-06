using System.Net.Http;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public interface IBuildMessageRepresentation
    {
        string BuildRequestRepresentation(HttpRequestMessage requestMessage);
    }
}