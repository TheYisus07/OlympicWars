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
    /// Interaction logic for SendGameInvitation.xaml
    /// </summary>
    public partial class SendGameInvitation : Window
    {
        private Guid _userId;
        private ServerPlayerProxy _serverDeckProxy;
        private IPlayerService _playerDeckChannel;
        private bool _playerHost = true;
        public SendGameInvitation()
        {
            InitializeComponent();
        }

        public void FillPlayerName(string playerName)
        {
            labelPlayer.Content = playerName;
        }

        public void loadProfile()
        {
            var playerService = new PlayerServiceCallback();
            playerService.LoadGameEvent += LoadGame;
            playerService.LoadFriendListEvent += LoadFriendList;
            playerService.ReceiveInvitationEvent += ReceiveInvitation;
            playerService.ReceiveConfirmationEvent += ReceiveConfirmation;
            playerService.StartTurnEvent += StartTurn;
            _serverDeckProxy = new ServerPlayerProxy(playerService);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _userId = Guid.NewGuid();
            _playerDeckChannel.GetFriendList(_userId, labelPlayer.Content.ToString());
        }

        private void buttonSendInvitation_Click(object sender, RoutedEventArgs e)
        {
            var selectedFriend = friendsListBox.SelectedItem as PlayerContract;

            if (selectedFriend == null)
            {
                MessageBox.Show("Please select a user first.");
                return;
            }

            _playerDeckChannel.SendInvitation(labelPlayer.Content.ToString(), selectedFriend.nickName);
            gridGameRoom.Visibility = Visibility.Visible;
            labelMyNam.Content = labelPlayer.Content;
        }

        public void LoadFriendList(List<PlayerContract> playerList)
        {
            friendsListBox.ItemsSource = playerList;
        }

        private void ReceiveInvitation(string userInvitationName, PlayerContract hostfriend)
        {
            var visible = Visibility.Visible;
            borderInvitation.Visibility = visible;
            buttonAcceptInvitation.Visibility = visible;
            iconAccept.Visibility = visible;
            labelInvitation.Content = userInvitationName;
            labelMyNam.Content = hostfriend.nickName;
            waitAceptInvitation.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var playerService = new PlayerServiceCallback();

            _serverDeckProxy = new ServerPlayerProxy(playerService);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.AceptInvitation(labelPlayer.Content.ToString(), labelMyNam.Content.ToString());

            labelNameInvitationAcepted.Content = labelPlayer.Content;
            gridGameRoom.Visibility = Visibility.Visible;
            buttonStartGame.IsEnabled = true;
            _playerHost = false;
        }

        private void ReceiveConfirmation(string playerConfirmation)
        {
            labelNameInvitationAcepted.Content = playerConfirmation;
            waitAceptInvitation.Visibility = Visibility.Hidden;
            buttonStartGame.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var playerService = new PlayerServiceCallback();
            _serverDeckProxy = new ServerPlayerProxy(playerService);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            if (_playerHost)
            {
                _playerDeckChannel.StarGame(labelMyNam.Content.ToString(), labelNameInvitationAcepted.Content.ToString());
                _playerDeckChannel.SendPlayerToStartTurns(labelMyNam.Content.ToString());
            }
            else
            {
                _playerDeckChannel.StarGame(labelNameInvitationAcepted.Content.ToString(), labelMyNam.Content.ToString());
            }
                
        }

        private void LoadGame(PlayerContract player1, PlayerContract player2)
        {
            PlayGame playGame = new PlayGame();
            playGame.StartGame(player1, player2);
            //playGame.StartTurnsPlayer1(player1.nickName);
            playGame.Show();
            this.Close();
        }

        private void StartTurn(bool startAttack, string player1)
        {
            if (startAttack)
            {
                PlayGame playGame = new PlayGame();
                
                playGame.StartTurnsPlayer1(player1);
            }
        }
    }
}
