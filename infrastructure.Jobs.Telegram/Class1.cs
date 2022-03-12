using System.Threading.Tasks;
using System;
using Infrastructure.UI.TelegramBot.IOInstances;
using Quartz;

namespace Infrastructure.Jobs.Telegram
{
    public class HelloJob : IJob
    {
        private readonly MessageConsumer _consumer;
        public HelloJob(MessageConsumer consumer)
        {
            _consumer = consumer;
        }

        public virtual Task Execute(IJobExecutionContext context)
        {
            // Say Hello to the World and display the date/time
            var timestamp = DateTime.Now;
            Console.WriteLine($"Hello World! - {timestamp:yyyy-MM-dd HH:mm:ss.fff}");
            return Task.CompletedTask;
        }
    }

}
