namespace Mango.Web.Services
{
    public static class ApiHelper
    {
        public static string ProductApiBase { get; set; }
        public static string ShoppingApiBase { get; set; }
        public static string CouponApiBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
