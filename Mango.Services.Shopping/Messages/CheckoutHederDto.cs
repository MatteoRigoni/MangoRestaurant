using Mango.MessageBus;
using Mango.Services.Shopping.Models.Dtos;

namespace Mango.Services.Shopping.Messages
{
    public class CheckoutHederDto: BaseMessage
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string? CouponCode { get; set; } = String.Empty;
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PickupDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CartNumber { get; set; }
        public int CartTotalItems { get; set; }
        public IEnumerable<CartDetailDto> CartDetails { get; set; }
    }
}
