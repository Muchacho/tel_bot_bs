using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace tbot
{
    class Program
    {
        private static string token { get; set; } = "2016886472:AAEI6oLC9EpVXPv4EMZ2JJBFMtm3sRLj7KI";

        private static TelegramBotClient client;
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }

        static int num = 0;
        const string def_pass = "D:\\projects\\tbot\\tbot\\source\\";
        static string pic_pass = "D:\\projects\\tbot\\tbot\\source\\";
        static string ori_pic_pass = "D:\\projects\\tbot\\tbot\\source\\";

        private static int gen(ref string str1, ref string str2)
        {
            Random rndnum = new Random();
            int i = 0;
            while (i != 1) { 
                num = rndnum.Next(1, 7);
                if(num != num_pic)
                {
                    i = 1;
                }
            }
            str1 = pic_pass + num + "_t.jpg";
            str2 = ori_pic_pass + num + ".jpg";
            return num;
        }

        static bool st_Ga = false;
        static string que_pic = "", ans_pic = "";
        static int num_pic = 0;

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            string[] rian = { "эль примо", "эдгар", "леон", "генерал гавс", "мортис", "сэнди" };
            var mess = e.Message;
            string mess_str = "Задача игры - угадать персонажа";
            string mess_str_cl = "Игра окончена";
            if (mess.Text != null)
            {
                Console.WriteLine(mess.Text);
                if (!st_Ga)
                {
                    switch (mess.Text)
                    {
                        case "Начать игру":
                            {
                                st_Ga = true;
                                await client.SendTextMessageAsync(mess.Chat.Id, mess_str, replyMarkup: SomeButtons());
                                num_pic = gen(ref que_pic, ref ans_pic);
                                using (var fileStream = new FileStream(que_pic, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    await client.SendPhotoAsync(mess.Chat.Id, new InputOnlineFile(fileStream), replyMarkup: SomeButtons());
                                }
                                break;
                            }
                        case "/start": { await client.SendTextMessageAsync(mess.Chat.Id, "Привет, я бот по бравл старсу.", replyMarkup: SomeButtons()); break; }
                        default: Console.WriteLine("кручу косяк"); break;
                    }
                }
                else if (mess.Text == "Закончить")
                {
                    st_Ga = false;
                    await client.SendTextMessageAsync(mess.Chat.Id, mess_str_cl, replyMarkup: SomeButtons());
                }
                else
                {
                    string str_m = mess.Text;
                    if (rian[num_pic - 1] == str_m.ToLower())
                    {
                        using (var fileStream = new FileStream(ans_pic, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await client.SendPhotoAsync(mess.Chat.Id, new InputOnlineFile(fileStream), replyMarkup: SomeButtons());
                        }
                        await client.SendTextMessageAsync(mess.Chat.Id, "Все верно! Продолжим");
                        num_pic = gen(ref que_pic, ref ans_pic);
                        using (var fileStream = new FileStream(que_pic, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await client.SendPhotoAsync(mess.Chat.Id, new InputOnlineFile(fileStream), replyMarkup: SomeButtons());
                        }

                    }
                    else
                    {
                        await client.SendTextMessageAsync(mess.Chat.Id, "Ответ, к сожалению, не верен. Пробуй еще", replyMarkup: SomeButtons());
                    }
                }
            }
        }
        private static IReplyMarkup SomeButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> {new KeyboardButton { Text = "Начать игру"}, new KeyboardButton { Text = "Закончить" } }
                }
            };
        }
    }
}
