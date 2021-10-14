using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnection;
        private readonly string topicNameProcessed;
        private readonly string subscriptionNameProcessed; // multi-subscribers on same topic

        private readonly EmailRepository _emailRepsitory;

        private ServiceBusProcessor processedProcessor;

        public AzureServiceBusConsumer(EmailRepository mailRepsitory,
            IConfiguration configuration)
        {
            _emailRepsitory = mailRepsitory;
            serviceBusConnection = configuration.GetValue<string>("ServiceBusConnectionString");
            topicNameProcessed = configuration.GetValue<string>("TopicNameProcessed");
            subscriptionNameProcessed = configuration.GetValue<string>("SubscriptionNameProcessed");

            var client = new ServiceBusClient(serviceBusConnection);
            processedProcessor = client.CreateProcessor(topicNameProcessed, subscriptionNameProcessed);
        }

        public async Task Start()
        {
            processedProcessor.ProcessMessageAsync += OnPaymentProcessedMessageReceived;
            processedProcessor.ProcessErrorAsync += OnErrorMessageHandler;
            await processedProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await processedProcessor.StopProcessingAsync();
            await processedProcessor.DisposeAsync();
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
            await _emailRepsitory.SendAndLogEmail(paymentResultMessage);
            await args.CompleteMessageAsync(args.Message);

        }
    }
}
