using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace tlgbt
{
    class Program
    {
        private MainWindow window;
            
        public ObservableCollection<MessageLog> BotMessageLog { get; set; }

        static ITelegramBotClient bot = new TelegramBotClient("@C:/Users/pyatn/Desktop/sklbx_bot");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Debug.WriteLine("-----");

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать" );
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        public void TelegramMessageClient(MainWindow Window , string pathToken = @"C:\Users\pyatn\Desktop\sklbx_bot\token.txt")
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);
            this.BotMessageLog = new ObservableCollection<MessageLog>();
            this.window = Window;

            

            bot = new TelegramBotClient(System.IO.File.ReadAllText(pathToken));

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, 
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );

        }
        public void SendMessage(string Text, string Id)
        {
            long id = Convert.ToInt64(Id);
            bot.SendTextMessageAsync(id, Text);
        }
    }
}