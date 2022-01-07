using ContractsOW;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServerPlayerProxy _serverPlayerProxy;
        private IPlayerService _playerChannel;
        private OlympicWarsContext _context = new OlympicWarsContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetName(string name)
        {
            labelPlayer.Content = name;
            GetRequest(name);
            var playerService = new PlayerServiceCallback();

            playerService.ReceiveConfirmationEvent += ReceiveConfirmaction;
            playerService.ReceiveInvitationEvent += ReceiveInvitation;

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();

        }

        public void ReceiveRequest()
        {
            var playerService = new PlayerServiceCallback();


            playerService.SendInvitationEvent += SendRequest;
            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();

        }

        private void GetRequest(string name)
        {
            var playerService = new PlayerServiceCallback();


            playerService.SeeRequestsEvent += SeeRequest;
            playerService.SendInvitationEvent += SendRequest;
            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.SearchOfRequests(name);
        }

        private void SeeRequest(List<FriendContract> friends)
        {
            labelNotifications.Content = friends.Count();
            listBoxFriendRequest.ItemsSource = friends;
        }

        public void LoadData(PlayerContract player)
        {
            var playerService = new PlayerServiceCallback();
            imagePlayer.Source = LoadImage(player.imageProfile);
            playerService.LoadGameEvent += LoadGame;
            playerService.StartTurnEvent += StartTurn;
            playerService.LoadFriendListEvent += LoadFriendList;
            playerService.ReceiveInvitationEvent += ReceiveInvitation;
            playerService.ReceiveConfirmationEvent += ReceiveConfirmaction;
            playerService.SendInvitationEvent += SendRequest;
            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.GetPlayers(player);
        }

        private void LoadFriendList(List<PlayerContract> _usersOnline)
        {
            listBoxFriends.Items.Clear();
            listBoxFriendsGame.Items.Clear();

            //listBox_FriendsGame.ItemsSource = _usersOnline;
            for (int i = 0; i < _usersOnline.Count(); i++)
            {
                if (_usersOnline[i].nickName != labelPlayer.Content.ToString())
                {
                    listBoxFriendsGame.Items.Add(_usersOnline[i].nickName);
                    listBoxFriends.Items.Add(_usersOnline[i].nickName);
                }
            }
        }

        public static BitmapImage LoadImage(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();
            if (imageData == null || imageData.Length == 0)
            {
                image = null;
            }
            using (MemoryStream memoryStream = new MemoryStream(imageData))
            {
                memoryStream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memoryStream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void Button_Modify_Click(object sender, RoutedEventArgs e)
        {
            buttonSaveProfile.Visibility = Visibility.Visible;
        }

        private void Button_Profile_Click(object sender, RoutedEventArgs e)
        {
            borderProfile.Visibility = Visibility.Visible;
            borderRoomGame.Visibility = Visibility.Hidden;
            borderFriends.Visibility = Visibility.Hidden;

            Player player = _context.Players.Single(u => u.nickName == labelPlayer.Content.ToString());

            textBlockNicknamePlayer.Text = player.nickName;
            textBlockState.Text = player.state;
            imageProfile.Source = LoadImage(player.imageProfile);
        }

        private void Button_Room_Game_Click(object sender, RoutedEventArgs e)
        {
            borderRoomGame.Visibility = Visibility.Visible;
            borderProfile.Visibility = Visibility.Hidden;
            borderFriends.Visibility = Visibility.Hidden;
        }

        private void Button_Friends_Click(object sender, RoutedEventArgs e)
        {
            borderFriends.Visibility = Visibility.Visible;
            borderRoomGame.Visibility = Visibility.Hidden;
            borderProfile.Visibility = Visibility.Hidden;
        }

        private void Button_Add_Friend_Click(object sender, RoutedEventArgs e)
        {
            borderSearchPlayer.Visibility = Visibility.Visible;
        }

        private void Button_Remove_Friend_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Search_Player_Click(object sender, RoutedEventArgs e)
        {

            if (textBoxSearch.Text == "")
            {
                MessageBox.Show("The search field is empty", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (labelPlayer.Content.ToString() == textBoxSearch.Text)
            {
                MessageBox.Show("Are you", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (labelPlayer.Content.ToString() != textBoxSearch.Text)
            {
                var playerService = new PlayerServiceCallback();

                playerService.SearchPlayerEvent += SendPlayer;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.SearchPlayer(textBoxSearch.Text);
            }

        }

        private void SendPlayer(PlayerContract player, bool isFound)
        {
            if (isFound)
            {
                Border_Send_Invitation.Visibility = Visibility.Visible;
                imageSend.Source = LoadImage(player.imageProfile);
                labelSendNick.Content = player.nickName;

            }
            else
            {
                MessageBox.Show("Player not found, try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSendInvitationClick(object sender, RoutedEventArgs e)
        {
            var playerService = new PlayerServiceCallback();
            string player = labelPlayer.Content.ToString();
            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.SendInvitation(labelPlayer.Content.ToString(), textBoxSearch.Text);
        }

        private void SendRequest(bool confirmInvitation)
        {
            GetRequest(labelPlayer.Content.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string friend = (string)listBoxFriendsGame.SelectedItem;

            var playerService = new PlayerServiceCallback();

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.SendInvitationGame(labelPlayer.Content.ToString(), friend);
        }

        private void ReceiveInvitation(string invitationName, PlayerContract friend)
        {
            List<PlayerContract> invitationList = new List<PlayerContract>();
            List<string> invitationListNickName = new List<string>();
            if (listBoxInvitations.ItemsSource != null)
            {
                invitationList = (List<PlayerContract>)listBoxInvitations.ItemsSource;
                for (int i = 0; i < invitationList.Count(); i++)
                {
                    invitationListNickName.Add(invitationList[i].nickName);
                }
            }
            
            if (!invitationListNickName.Contains(friend.nickName))
            {
                invitationList.Add(friend);
            }
            listBoxInvitations.ItemsSource = invitationList;
            listBoxInvitations.Items.Refresh();
        }

        private void ReceiveConfirmaction(string playerConfirmation)
        {
            var playerService = new PlayerServiceCallback();

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.StarGame(labelPlayer.Content.ToString(), playerConfirmation);
            _playerChannel.SendPlayerToStartTurns(labelPlayer.Content.ToString());
        }

        private void buttonAcceptGame_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxInvitations.Items.CurrentPosition;
            PlayerContract playerFriend = (PlayerContract)listBoxInvitations.Items[i];
            var playerService = new PlayerServiceCallback();

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.AceptInvitation(labelPlayer.Content.ToString(), playerFriend.nickName);
        }

        private void buttonDenyGame_Click(object sender, RoutedEventArgs e)
        {
            List<PlayerContract> invitationList = new List<PlayerContract>();
            invitationList = (List<PlayerContract>)listBoxInvitations.ItemsSource;
            int i = listBoxInvitations.Items.CurrentPosition;
            PlayerContract playerFriend = (PlayerContract)listBoxInvitations.Items[i];
            invitationList.Remove(playerFriend);
            if(invitationList.Count() > 0)
            {
                listBoxInvitations.ItemsSource = invitationList;
            }
            else
            {
                listBoxInvitations.ItemsSource = null;
            }
            
            listBoxInvitations.Items.Refresh();
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

        private void buttonDecks_Click(object sender, RoutedEventArgs e)
        {
            gridDecks.Visibility = Visibility.Visible;
            loadDataGridDecks();
        }

        private void buttonHistory_Click(object sender, RoutedEventArgs e)
        {
            gridHistory.Visibility = Visibility.Visible;
        }

        public void loadDataGridDecks()
        {
            var decksss = new PlayerServiceCallback();
            decksss.DecksListUpdatedEvent += getListOfDecks;
            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();

            try
            {
                _playerChannel.chargeDecks(labelPlayer.Content.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to login. Make sure the server application is running.");
                return;
            }
        }

        private void getListOfDecks(List<DeckContract> decks)
        {
            listBoxDecks.ItemsSource = decks;
        }

        private void ListBoxDecksClicked(object sender, MouseButtonEventArgs e)
        {
            if (listBoxDecks.SelectedItem != null)
            {
                this.ShowCards();
            }
        }

        private void ShowCards()
        {
            DeckContract deck = (DeckContract)listBoxDecks.SelectedItem;
            string cellvalue = deck.deckName;
            var decksss = new PlayerServiceCallback();
            decksss.CardListUpdatedEvent += LoadListOfCards;
            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.LoadCards(cellvalue);
        }

        private void LoadListOfCards(List<CardContract> cardContractList)
        {
            listBoxCardFromDeck.ItemsSource = cardContractList;
            buttonModifyDeck.Visibility = Visibility.Visible;
        }

        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            gridAddDeck.Visibility = Visibility.Visible;
            buttonSaveDeck.Visibility = Visibility.Visible;
            gridCards.Visibility = Visibility.Visible;
            var decksss = new PlayerServiceCallback();
            decksss.CardListGotEvent += LoadAllCards;

            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.GetAllCards();
        }

        private void LoadAllCards(List<CardContract> cardContractList)
        {
            gridCards.Visibility = Visibility.Visible;
            listBoxCards.ItemsSource = cardContractList;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gridDeckUpdate.Visibility = Visibility.Hidden;
            gridAddDeck.Visibility = Visibility.Hidden;
            buttonSaveDeck.Visibility = Visibility.Hidden;
            buttonUpdateDeck.Visibility = Visibility.Hidden;
            gridCards.Visibility = Visibility.Hidden;
            gridDeckUpdate.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string deckName = textBoxDeckName.Text;
            System.Collections.IList cartIList = listBoxCards.SelectedItems;
            List<string> cardCollection = cartIList.Cast<CardContract>().Select(cl => cl.nameCard).ToList();
            var decksss = new PlayerServiceCallback();
            decksss.DeckSavedEvent += ConfirmDeckSaved;
            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            if (cardCollection.Count() > 12)
            {
                _playerChannel.SaveDeck(cardCollection, deckName, labelPlayer.Content.ToString());
            }
            else
            {
                MessageBox.Show("he deck must contain 13 cards or more");
            }
            this.loadDataGridDecks();
            gridAddDeck.Visibility = Visibility.Hidden;
            buttonSaveDeck.Visibility = Visibility.Hidden;
            gridCards.Visibility = Visibility.Hidden;
        }

        private void ConfirmDeckSaved(int numberEntries)
        {
            MessageBox.Show("The number of entries in the databas is -> " + numberEntries + "Deck Saved");
        }

        private void buttonModifyDeck_Click(object sender, RoutedEventArgs e)
        {
            List<CardContract> cardList = (List<CardContract>)listBoxCardFromDeck.ItemsSource;
            ListBoxCardsAdded.ItemsSource = cardList;
            DeckContract deck = (DeckContract)listBoxDecks.SelectedItem;
            string deckName = deck.deckName;
            textBoxDeckName.Text = deckName;
            List<string> cardNameList = cardList.Select(c => c.nameCard).ToList();
            var decksss = new PlayerServiceCallback();
            decksss.CardListFiltredToUpdateEvent += CardListFiltredToUpdateEvent;
            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.GetCardsToModify(cardNameList);
            textBoxDeckName.Text = deckName;
        }

        private void CardListFiltredToUpdateEvent(List<CardContract> cardList)
        {

            listBoxCardsToUpdate.ItemsSource = cardList;
            listBoxCardsToUpdate.Items.Refresh();
            gridAddDeck.Visibility = Visibility.Visible;
            buttonUpdateDeck.Visibility = Visibility.Visible;
            gridDeckUpdate.Visibility = Visibility.Visible;
            scrollViewerCardsToModify.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            List<CardContract> cardList = (List<CardContract>)ListBoxCardsAdded.ItemsSource;
            DeckContract deck = (DeckContract)listBoxDecks.SelectedItem;
            string deckName = deck.deckName;
            var decksss = new PlayerServiceCallback();
            decksss.DeckUpdatedEvent += ConfirmDeckUpdated;
            _serverPlayerProxy = new ServerPlayerProxy(decksss);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            if (cardList.Count() > 12)
            {
                _playerChannel.UpdateDeck(cardList, deckName);
            }
            else
            {
                MessageBox.Show("he deck must contain 13 cards or more");
            }

        }

        private void ConfirmDeckUpdated(int numerEntries)
        {
            gridAddDeck.Visibility = Visibility.Hidden;
            buttonUpdateDeck.Visibility = Visibility.Hidden;
            gridDeckUpdate.Visibility = Visibility.Hidden;
            scrollViewerCardsToModify.Visibility = Visibility.Hidden;
            this.ShowCards();
            listBoxDecks.Items.Refresh();
            MessageBox.Show("The number of entries in the databas is -> " + numerEntries + "Deck Updated");

        }

        private void ListBoxCardsAdded_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CardContract card = (CardContract)ListBoxCardsAdded.SelectedItem;
            List<CardContract> cardList = (List<CardContract>)ListBoxCardsAdded.ItemsSource;
            List<CardContract> cardListToRemove = (List<CardContract>)listBoxCardsToUpdate.ItemsSource;
            cardList.Remove(card);
            cardListToRemove.Add(card);
            listBoxCardsToUpdate.ItemsSource = cardListToRemove;
            ListBoxCardsAdded.ItemsSource = cardList;
            listBoxCardsToUpdate.Items.Refresh();
            ListBoxCardsAdded.Items.Refresh();
        }

        private void gridDeckUpdate_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CardContract card = (CardContract)listBoxCardsToUpdate.SelectedItem;
            List<CardContract> cardList = (List<CardContract>)ListBoxCardsAdded.ItemsSource;
            List<CardContract> cardListToRemove = (List<CardContract>)listBoxCardsToUpdate.ItemsSource;
            cardList.Add(card);
            cardListToRemove.Remove(card);
            listBoxCardsToUpdate.ItemsSource = cardListToRemove;
            ListBoxCardsAdded.ItemsSource = cardList;
            listBoxCardsToUpdate.Items.Refresh();
            ListBoxCardsAdded.Items.Refresh();
        }
        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            gridHistory.Visibility = Visibility.Hidden;
        }

        public void loadGameHistoryData()
        {
            string userName = labelPlayer.Content.ToString();
            var service = new PlayerServiceCallback();
            service.LoadGameHistoryEvent += loadGameHistory;
            _serverPlayerProxy = new ServerPlayerProxy(service);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.getGameHistory(userName);
        }

        private void loadGameHistory(List<GameContract> gameHistory)
        {
            listBoxGameHistory.ItemsSource = gameHistory;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            gridDecks.Visibility = Visibility.Hidden;
        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridFriendRequest.Visibility = Visibility.Visible;
        }

        private void gridFriendRequest_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gridFriendRequest.Visibility = Visibility.Hidden;

        }

        private void buttonAcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxFriendRequest.Items.CurrentPosition;
            FriendContract playerFriend = (FriendContract)listBoxFriendRequest.Items[i];
            var playerService = new PlayerServiceCallback();

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.AcceptFriendRequest(labelPlayer.Content.ToString(), playerFriend.nicknameFriend);
        }

        private void buttonDenyRequest_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxFriendRequest.Items.CurrentPosition;
            FriendContract playerFriend = (FriendContract)listBoxFriendRequest.Items[i];
            var playerService = new PlayerServiceCallback();

            _serverPlayerProxy = new ServerPlayerProxy(playerService);
            _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            _playerChannel.DenyFriendRequest(labelPlayer.Content.ToString(), playerFriend.nicknameFriend);
        }
    }
}