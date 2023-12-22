using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using mvvmsample.model;
using Telegram.Bot;
using System.Threading.Tasks;

namespace mvvmsample.viewwmodel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<ProductModel> allOrders;
        private ObservableCollection<ProductModel> filteredOrders;

        public ObservableCollection<ProductModel> Orders
        {
            get => filteredOrders;
            set => Set(ref filteredOrders, value);
        }

        private int? searchOrderId;

        public int? SearchOrderId
        {
            get => searchOrderId;
            set
            {
                if (Set(ref searchOrderId, value))
                {
                    FilterOrders();
                }
            }
        }

        public ICommand SearchOrderCommand { get; set; }
        public ICommand IssueCommand { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public MainViewModel()
        {
            using (var dbContext = new AppDbContext())
            {
                allOrders = new ObservableCollection<ProductModel>(dbContext.Orders);
                Orders = allOrders;
            }

            SearchOrderId = null;

            SearchOrderCommand = new RelayCommand(SearchOrder);
            IssueCommand = new RelayCommand<int>(IssueOrder);
            ReturnCommand = new RelayCommand<int>(ReturnOrder);
            RefreshCommand = new RelayCommand<object>(parameter => RefreshOrders());
        }

        private void SearchOrder(object parameter)
        {
            FilterOrders();
        }

        private void FilterOrders()
        {
            ObservableCollection<ProductModel> temporaryCollection;

            if (!string.IsNullOrEmpty(SearchOrderId?.ToString()) && SearchOrderId.ToString().Length > 1)
            {
                temporaryCollection = new ObservableCollection<ProductModel>(
                    allOrders.Where(order => order.O_id.ToString().Contains(SearchOrderId.ToString()))
                );
            }
            else
            {
                temporaryCollection = new ObservableCollection<ProductModel>(allOrders);
            }

            Orders = temporaryCollection;
        }

        private void IssueOrder(int orderId)
        {
            ProductModel selectedOrder = Orders.FirstOrDefault(order => order.O_id == orderId);

            if (selectedOrder != null && selectedOrder.Status == "готово к выдаче")
            {
                Random random = new Random();
                int generatedNumber = random.Next(1000, 10000);
                TelegramNotifier telegramNotifier = new TelegramNotifier("6803945377:AAE_O7N_Y8hI4zAuXsufMtVMVzXpLKouFPI", (long)selectedOrder.U_id);
                telegramNotifier.SendMessageAsync($"Код получения:{generatedNumber}");

                string userInput = Microsoft.VisualBasic.Interaction.InputBox("Введите четырёхзначное число:", "Подтверждение выдачи", "", -1, -1);

                if (!string.IsNullOrEmpty(userInput) && int.TryParse(userInput, out int enteredNumber) && enteredNumber == generatedNumber)
                {
                    selectedOrder.Status = "выдан";

                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Orders.First(o => o.O_id == orderId).Status = "выдан";
                        dbContext.SaveChanges();
                        telegramNotifier.SendMessageAsync($"Товар {selectedOrder.pr_name} выдан");
                    }

                    LoadOrders();
                    MessageBox.Show($"ID:{selectedOrder.O_id}. Выдача подтверждена");
                }
                else
                {
                    MessageBox.Show("Невозможно выдать заказ. Товар не готов к выдаче или введенное число не совпадает.");
                }
            }
            else
            {
                MessageBox.Show("Невозможно выдать заказ. Товар не готов к выдаче.");
            }
        }

        private void ReturnOrder(int orderId)
        {
            ProductModel selectedOrder = Orders.FirstOrDefault(order => order.O_id == orderId);
            Random random = new Random();
            int generatedNumber = random.Next(1000, 10000);
            TelegramNotifier telegramNotifier = new TelegramNotifier("6803945377:AAE_O7N_Y8hI4zAuXsufMtVMVzXpLKouFPI", (long)selectedOrder.U_id);
            telegramNotifier.SendMessageAsync($"Код подтверждения возврата:{generatedNumber}");

            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Введите четырёхзначное число:", "Подтверждение возврата", "", -1, -1);

            if (!string.IsNullOrEmpty(userInput) && int.TryParse(userInput, out int enteredNumber) && enteredNumber == generatedNumber)
            {
                if (selectedOrder != null)
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Orders.Remove(selectedOrder);
                        dbContext.SaveChanges();
                    }

                    LoadOrders();
                }
            }
        }

        private void LoadOrders()
        {
            using (var dbContext = new AppDbContext())
            {
                allOrders = new ObservableCollection<ProductModel>(dbContext.Orders);
                Orders = allOrders;
            }
        }

        private void RefreshOrders()
        {
            LoadOrders();
        }

        public class TelegramNotifier
        {
            private readonly TelegramBotClient _botClient;
            private readonly long _chatId;

            public TelegramNotifier(string botToken, long chatId)
            {
                _botClient = new TelegramBotClient(botToken);
                _chatId = chatId;
            }

            public async Task SendMessageAsync(string message)
            {
                await _botClient.SendTextMessageAsync(_chatId, message);
            }
        }
    }
}
