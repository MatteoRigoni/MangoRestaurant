using Mango.MessageBus;

namespace Mango.Services.Orders.Messages
{
    public class PaymentRequestMessage: BaseMessage
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public double OrderTotal { get; set; }
        public string Email { get; set; }
    }
}
