using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Newtonsoft.Json;
using System.Text;
using Mongo.Services.Payment.Messages;
using PaymentProcessor;

namespace Mongo.Services.Payment.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnection;
        private readonly string topicNamePayment;
        private readonly string subscriptionNamePayment;
        private readonly string topicNamePaymentCompleted;
        private readonly IProcessPayment _processPayment;
        private readonly IMessageBus _messageBus;

        private ServiceBusProcessor paymentProcessor;

        public AzureServiceBusConsumer(IProcessPayment processPayment,
            IConfiguration configuration,
            IMessageBus messageBus)
        {
            _processPayment = processPayment;
            _messageBus = messageBus;
            serviceBusConnection = configuration.GetValue<string>("ServiceBusConnectionString");
            topicNamePayment = configuration.GetValue<string>("TopicNameProcess");
            subscriptionNamePayment = configuration.GetValue<string>("SubscriptionNameProcess");
            topicNamePaymentCompleted = configuration.GetValue<string>("TopicNameProcessed");

            var client = new ServiceBusClient(serviceBusConnection);
            paymentProcessor = client.CreateProcessor(topicNamePayment, subscriptionNamePayment);
        }

        public async Task Start()
        {
            paymentProcessor.ProcessMessageAsync += OnPaymentMessageReceived;
            paymentProcessor.ProcessErrorAsync += OnErrorMessageHandler;
            await paymentProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await paymentProcessor.StopProcessingAsync();
            await paymentProcessor.DisposeAsync();
        }

        private Task OnErrorMessageHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnPaymentMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = await _processPayment.Pay();

            UpdatePaymentResultMessage updatePaymentResultMessage = new UpdatePaymentResultMessage()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            await _messageBus.PublishMessage(updatePaymentResultMessage, topicNamePaymentCompleted);
        }
    }
}
