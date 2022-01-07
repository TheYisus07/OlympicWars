using ContractsOW;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Window
    {

        private ServerPlayerProxy _serverDeckProxy;
        private IPlayerService _playerDeckChannel;

        public History()
        {
            InitializeComponent();
        }

        public void getUserName(string userName)
        {
            labelUserName.Content = userName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //string userName = labelUserName.Content.ToString();
            //var service = new PlayerServiceCallback();
            //service.LoadGameHistoryEvent += loadGameHistory;
            //_serverDeckProxy = new ServerDeckProxy(service);
            //_playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            //_playerDeckChannel.getGameHistory(userName);
        }

        public void loadGameHistoryData()
        {
            string userName = labelUserName.Content.ToString();
            var service = new PlayerServiceCallback();
            service.LoadGameHistoryEvent += loadGameHistory;
            _serverDeckProxy = new ServerPlayerProxy(service);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.getGameHistory(userName);
        }

        private void loadGameHistory(List<GameContract> gameHistory)
        {
            listBoxGameHistory.ItemsSource = gameHistory;
        }
    }
}
