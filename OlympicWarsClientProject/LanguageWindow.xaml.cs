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
    /// Interaction logic for LanguageWindow.xaml
    /// </summary>
    public partial class LanguageWindow : Window
    {
        private ServerPlayerProxy _serverDeckProxy;
        private IPlayerService _playerDeckChannel;
        public LanguageWindow()
        {
            
            InitializeComponent();
        }

        private void comboBoxLanguege_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxLanguege.SelectedIndex == 0)
            {
                Properties.Settings.Default.languageCode = "en-US";
            }
            else
            {
                Properties.Settings.Default.languageCode = "es-MX";
            }
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Please introduce a user Name");
                return;
            }


            string name = textBoxName.Text;
            var decksss = new PlayerServiceCallback();
            decksss.PlayersListUpdatedEvent += OpenWindow;
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            //_userId = Guid.NewGuid();

            //var player = new PlayerDeckService();
            ////player.PlayersListUpdatedEvent += UpdateListOfConnectedClients;

            //_playerServiceProxy = new PlayerServiceProxy(player);
            //_playerChannel = _playerServiceProxy.ChannelFactory.CreateChannel();

            try
            {

                _playerDeckChannel.Logon(Guid.NewGuid(), name);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to login. Make sure the server application is running.");
                //UserName.IsEnabled = true;
                //loginButton.IsEnabled = true;
                return;
            }
            
            //sendButton.IsEnabled = true;
            //messageTextbox.IsEnabled = true;
        }

        private void OpenWindow(bool access, string player)
        {
            if (access)
            {
                //SendGameInvitation sendGameInvitation = new SendGameInvitation();
                //sendGameInvitation.FillPlayerName(player);
                //sendGameInvitation.loadProfile();
                //sendGameInvitation.Show();


                //History history = new History();
                //history.getUserName(player);
                //history.loadGameHistoryData();
                //history.Show();

                DeckWindow mainWindow = new DeckWindow();
                mainWindow.loadDataGridDecks(player);
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Access denied " + player + " invalid");
            }
            
        }

    }
}
