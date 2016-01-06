namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public interface ISecretRepository
    {
        string GetSecretForAppKey(string appKey);
    }
}