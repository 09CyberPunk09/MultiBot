using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.IOInstances;
using Telegram.Bot;

namespace Infrastructure.UI.TelegramBot
{
    public class MessageReceiver : IMessageReceiver
    {
        #region Injected members
        private readonly ITelegramBotClient _uiClient;
        private readonly MessageConsumer _consumer;
        #endregion


        static MessageReceiver()
        {

        }

        public MessageReceiver(MessageConsumer consumer, ITelegramBotClient uiClient)
        {

            (_consumer, _uiClient) = (consumer, uiClient);

        }
        public void ConsumeMessage(Message message)
        {
            _consumer.ConsumeMessage(message);
        }



        public void Start()
        {
            //MatchPipeline();
            _uiClient.StartReceiving<MessageUpdateHandler>();
        }

        public void Stop()
        {
        }

    }

}
