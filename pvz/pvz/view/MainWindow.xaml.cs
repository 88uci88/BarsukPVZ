using System.Windows;
using mvvmsample.viewwmodel;

namespace mvvmsample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = new MainViewModel();            
        }
        public MainViewModel ViewModel { get; set; }
    }
}
