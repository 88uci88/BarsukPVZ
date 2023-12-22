using Microsoft.Data.Sqlite;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
string dbPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "database", "barsukDB.db"));
string connectionString = $"Data Source={dbPath};";
using (var connection = new SqliteConnection($"Data Source={dbPath};"))
{
    connection.Open();
    SqliteCommand command = new SqliteCommand();
    command.Connection = connection;
    command.CommandText = "CREATE TABLE IF NOT EXISTS Users (U_id INTEGER PRIMARY KEY, Name TEXT, Username TEXT); " +
     "CREATE TABLE IF NOT EXISTS ShopList(pr_id INTEGER PRIMARY KEY, ProductName TEXT, Photo TEXT, Description TEXT, Cost INTEGER, Condition TEXT); " +
     "CREATE TABLE IF NOT EXISTS Orders (O_id INTEGER PRIMARY KEY, pr_id INTEGER, U_id INTEGER, pr_name TEXT,pr_cost int, Descr TEXT, Status TEXT, username text);";
    command.ExecuteNonQuery();
    Console.WriteLine("Таблицы созданны");
    connection.Close();
}

var client = new TelegramBotClient("6803945377:AAE_O7N_Y8hI4zAuXsufMtVMVzXpLKouFPI");
client.StartReceiving(Update, Error);

async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
{
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
        new KeyboardButton[] { "Каталог товаров🛒", "Мои заказы🛍" },
        new KeyboardButton[] { "Мой аккаунт👤", "Связь с поддержкой🆘" }
    })
    {
        ResizeKeyboard = true
    };
    var message = update.Message;
    var callbackQuery = update.CallbackQuery;
    string name = string.Empty;

    if (message != null)
    {
        name = message.Chat.FirstName + message.Chat.LastName;
        var connection = new SqliteConnection($"Data Source={Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "database", "barsukDB.db"))};");

        connection.Open();
        string checkDuplicateUserQuery = $"SELECT COUNT(*) FROM Users WHERE U_id = {message.Chat.Id};";
        var checkDuplicateUserCommand = new SqliteCommand(checkDuplicateUserQuery, connection);
        using (checkDuplicateUserCommand)
        {
            int existingRecordsCount = Convert.ToInt32(checkDuplicateUserCommand.ExecuteScalar());
            if (existingRecordsCount == 0)
            {
                string insertDataQuery = $"INSERT INTO Users (U_id, Name, Username) VALUES" +
                $" ({message.Chat.Id}, '{name}', '{message.Chat.Username}');";
                using (var insertCommand = new SqliteCommand(insertDataQuery, connection))
                {
                    insertCommand.ExecuteNonQuery();
                    Console.WriteLine("Пользлватель добавлен");
                    connection.Close();
                }
            }
        }

        Console.WriteLine($"{message?.Chat.Id} | {message?.Chat.FirstName} | {message?.Text}");

        if (message.Text == "/start" || message.Text == "/restart")
        {
            string imagePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "images", "BarsukStart.png"));
            using (var stream = new FileStream(imagePath, FileMode.Open))
            {
                await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromStream(stream: stream, "BarsukStart.png"),
                    caption: "Здравствуйте! Вы зашли в Barsuk Shop\nУдачных покупок!", replyMarkup: replyKeyboardMarkup);
            }
        }
        if (message.Text.ToLower().Contains("каталог товаров"))
        {
            string countQuery = "SELECT COUNT(*) FROM ShopList;";
            string sqlExpression = "SELECT pr_id, ProductName, Description, Cost, Condition, Photo FROM ShopList;";
            using (var outpr = new SqliteConnection("Data Source=usersdata.db"))
            {
                connection.Open();
                using (var readCommand = new SqliteCommand(sqlExpression, connection))
                {
                    using (var reader = readCommand.ExecuteReader())
                    {
                        while (reader.Read())                    
                        {
                            var pr_id = reader.GetValue(0);
                            var pr_name = reader.GetValue(1);
                            var pr_descr = reader.GetValue(2);
                            var pr_cost = reader.GetValue(3);
                            var pr_cond = reader.GetValue(4);
                            var pr_photo = reader.GetValue(5);
                            string photo = pr_id + ".jpg";
                            string imagePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "images", photo));
                            string pr_caption = $"{pr_name} {pr_cost}.руб\n\n{pr_descr}\n\nСостояние: {pr_cond}";

                            using (var stream = new FileStream(imagePath, FileMode.Open))
                            {                              
                                await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromStream(stream: stream, photo), caption: pr_caption, replyMarkup: new InlineKeyboardMarkup(
                                InlineKeyboardButton.WithCallbackData(
                                text: "Купить",
                                callbackData: $"buy_{pr_id}_{pr_name}")));
                            }
                        }
                    }
                }
                connection.Close();
            }
        }
        if (message.Text.ToLower().Contains("мой аккаунт"))
        {
            long userId = message.Chat.Id;
            string countOrdersQuery = $"SELECT COUNT(*) FROM Orders WHERE U_id = {userId};";
            using (var countOrdersCommand = new SqliteCommand(countOrdersQuery, connection))
            {
                int orderCount = Convert.ToInt32(countOrdersCommand.ExecuteScalar());
                Console.WriteLine($"Количество заказов: {orderCount}");
                string sumOrdersQuery = $"SELECT SUM(pr_cost) FROM Orders WHERE U_id = {userId} AND Status = 'выдан';";
                using (var sumOrdersCommand = new SqliteCommand(sumOrdersQuery, connection))
                {
                    object sumObject = sumOrdersCommand.ExecuteScalar();
                    int sum = sumObject != DBNull.Value ? Convert.ToInt32(sumObject) : 0;
                    Console.WriteLine($"Сумма выкупа: {sum} руб.");
                    string accountInfo = $"{name}\nКол-во заказов: {orderCount}\nСумма выкупа: {sum} руб.";
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: accountInfo, replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                }
            }
        }


        if (message.Text.ToLower().Contains("связь с поддержкой"))
        {
            await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"{name}, у вас возникли вопросы по работе нашего сервиса?\nВот наши контакты для связи:\nТелеграмм: @barsuksupport\nЭлектронная почта: barsukSupport@gmail.com\nНомер телефона: +79123456789 ",
                replyMarkup: replyKeyboardMarkup, cancellationToken: token);
        }
        if (message.Text.ToLower().Contains("мои заказы"))
        {
            long userId = message.Chat.Id;

            string countQueryOrders = $"SELECT COUNT(*) FROM Orders WHERE U_id = {userId};";
            string sqlExpressionOrders = $"SELECT O_id, pr_id, U_id, pr_name, Descr, Status FROM Orders WHERE U_id = {userId};";

            using (var connectionOrders = new SqliteConnection($"Data Source={Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "database", "barsukDB.db"))};"))
            {
                connectionOrders.Open();

                using (var readCommandOrders = new SqliteCommand(sqlExpressionOrders, connectionOrders))
                {
                    using (var readerOrders = readCommandOrders.ExecuteReader())
                    {
                        while (readerOrders.Read())
                        {
                            var o_id = readerOrders.GetValue(0);
                            var pr_id = readerOrders.GetValue(1);
                            var u_id = readerOrders.GetValue(2);
                            var pr_name = readerOrders.GetValue(3);
                            var pr_descr = readerOrders.GetValue(4);
                            var status = readerOrders.GetValue(5);

                            string photo = pr_id + ".jpg";
                            string imagePath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "images", photo));
                            string orderInfo = $"ID: {o_id}\n{pr_name}\n{pr_descr}\nСтатус: {status}";

                            using (var stream = new FileStream(imagePath, FileMode.Open))
                            {
                                await botClient.SendPhotoAsync(message.Chat.Id, InputFile.FromStream(stream: stream, photo), caption: orderInfo);
                            }
                        }
                    }
                }

                connectionOrders.Close();
            }
        }

    }
    else if (callbackQuery != null)
    {
        var callbackData = callbackQuery.Data;

        if (callbackData.StartsWith("buy_"))
        {
            var pr_id = callbackData.Split('_')[1];
            var pr_name = callbackData.Split('_')[2];
            
            string pr_descr = "";
            string username = $"{callbackQuery.Message.Chat.FirstName} {callbackQuery.Message.Chat.LastName}";
            int cost = 0;
            int o_id;
            bool isUniqueId = false;

            using (var connection = new SqliteConnection($"Data Source={Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "BARSUK", "database", "barsukDB.db"))};"))
            {
                connection.Open();
                string checkProductQuery = $"SELECT COUNT(*) FROM ShopList WHERE pr_id = {pr_id};";
                using (var checkProductCommand = new SqliteCommand(checkProductQuery, connection))
                {
                    int productCount = Convert.ToInt32(checkProductCommand.ExecuteScalar());

                    if (productCount == 0)
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: callbackQuery.Message.Chat.Id,
                            text: $"Извините, товар {pr_name} не найден в каталоге. Возможно, он уже продан или удален.");
                        connection.Close();
                        return;
                    }
                }


                string selectDescriptionQuery = $"SELECT Description, Cost FROM ShopList WHERE pr_id = {pr_id};";             

                using (var selectDescriptionCommand = new SqliteCommand(selectDescriptionQuery, connection))
                {
                    using (var reader = selectDescriptionCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pr_descr = reader.GetString(0);  
                            cost = reader.GetInt32(1);   
                            pr_descr += $"\n{cost}руб"; 
                            
                        }
                    }
                }
         
                Random random = new Random();
                do
                {
                    o_id = random.Next(1000, 10000);
                    string checkUniqueIdQuery = $"SELECT COUNT(*) FROM Orders WHERE O_id = {o_id};";

                    using (var checkUniqueIdCommand = new SqliteCommand(checkUniqueIdQuery, connection))
                    {
                        int existingRecordsCount = Convert.ToInt32(checkUniqueIdCommand.ExecuteScalar());
                        isUniqueId = existingRecordsCount == 0;
                    }

                } while (!isUniqueId);

                string insertOrderQuery = $"INSERT INTO Orders (O_id, pr_id, U_id, pr_name, Descr, Status, pr_cost,username) VALUES" +
                    $" ({o_id}, {pr_id}, {callbackQuery.Message.Chat.Id}, '{pr_name}', '{pr_descr}', 'обработка', {cost}, '{username}');";

                using (var insertOrderCommand = new SqliteCommand(insertOrderQuery, connection))
                {
                    insertOrderCommand.ExecuteNonQuery();
                }

                string deleteShopListItemQuery = $"DELETE FROM ShopList WHERE pr_id = {pr_id};";
                using (var deleteShopListItemCommand = new SqliteCommand(deleteShopListItemQuery, connection))
                {
                    deleteShopListItemCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Вы купили {pr_name}");
        }
    }
}


Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
{
    Console.WriteLine($"Error: {arg2.Message}");
    return Task.CompletedTask;
}

Console.ReadLine();