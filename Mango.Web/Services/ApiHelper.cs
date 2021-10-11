namespace Mango.Web.Services
{
    public static class ApiHelper
    {
        public static string MangoApiBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
