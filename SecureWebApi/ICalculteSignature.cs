namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public interface ICalculteSignature
    {
        string Signature(string secret, string value);
    }
}