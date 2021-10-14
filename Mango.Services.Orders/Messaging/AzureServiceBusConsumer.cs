using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.Orders.Messages;
using Mango.Services.Orders.Models;
using Mango.Services.Orders.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Orders.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnection;
        private readonly string topicNameCheckout;
        private readonly string subscriptionNameCheckout;
        private readonly string topicNameProcess;
        private readonly string subscriptionNameProcess;
        private readonly string topicNameProcessed;
        private readonly string subscriptionNameProcessed;

        private readonly OrderRepository _orderRepsitory;
        private readonly IMessageBus _messageBus;

        private ServiceBusProcessor checkoutProcessor;
        private ServiceBusProcessor processedProcessor;

        public AzureServiceBusConsumer(OrderRepository orderRepsitory,
            IConfiguration configuration,
            IMessageBus messageBus)
        {
            _orderRepsitory = orderRepsitory;
            _messageBus = messageBus;
            serviceBusConnection = configuration.GetValue<string>("ServiceBusConnectionString");
            topicNameCheckout = configuration.GetValue<string>("TopicNameCheckout");
            subscriptionNameCheckout = configuration.GetValue<string>("SubscriptionNameCheckout");
            topicNameProcess = configuration.GetValue<string>("TopicNameProcess");
            subscriptionNameProcess = configuration.GetValue<string>("SubscriptionNameProcess");
            topicNameProcessed = configuration.GetValue<string>("TopicNameProcessed");
            subscriptionNameProcessed = configuration.GetValue<string>("SubscriptionNameProcessed");

            var client = new ServiceBusClient(serviceBusConnection);
            checkoutProcessor = client.CreateProcessor(topicNameCheckout, subscriptionNameCheckout);
            processedProcessor = client.CreateProcessor(topicNameProcessed, subscriptionNameProcessed);
        }

        public async Task Start()
        {
            checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            checkoutProcessor.ProcessErrorAsync += OnErrorMessageHandler;
            await checkoutProcessor.StartProcessingAsync();

            processedProcessor.ProcessMessageAsync += OnPaymentProcessedMessageReceived;
            processedProcessor.ProcessErrorAsync += OnErrorMessageHandler;
            await processedProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await checkoutProcessor.StopProcessingAsync();
            await checkoutProcessor.DisposeAsync();
        }

        private Task OnErrorMessageHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnPaymentProcessedMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);
            await _orderRepsitory.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);
            await args.CompleteMessageAsync(args.Message);

        }
        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader order = new OrderHeader()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetail>(),
                CouponCode = checkoutHeaderDto.CouponCode,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                PickupDate = checkoutHeaderDto.PickupDate
            };
            foreach (var detail in checkoutHeaderDto.CartDetails)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = detail.ProductId,
                    ProductName = detail.Product.Name,
                    Price = detail.Product.Price,
                    Count = detail.Count
                };
                order.OrderDetails.Add(orderDetail);
            }

            await _orderRepsitory.AddOrder(order);

            PaymentRequestMessage paymentRequestMessage = new()
            {
                Name = order.FirstName,
                CardNumber = order.CartNumber,
                OrderTotal = order.OrderTotal,
                Email = order.Email
            };

            try
            {
                await _messageBus.PublishMessage(paymentRequestMessage, topicNameProcess);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                // ...
                throw;
            }
        }
    }
}
