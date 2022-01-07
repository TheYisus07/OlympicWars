using ContractsOW;
using OlympicWarsClientProject.Validations;
using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
        private Log log = new Log();
        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetName(string name)
        {
            labelPlayer.Content = name;
            GetRequest(name);
            try
            {
                var playerService = new PlayerServiceCallback();

                playerService.ReceiveConfirmationEvent += ReceiveConfirmaction;
                playerService.ReceiveInvitationEvent += ReceiveInvitation;

                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }

        }

        public void ReceiveRequest()
        {
            try
            {
                var playerService = new PlayerServiceCallback();


                playerService.SendInvitationEvent += SendRequest;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void GetRequest(string name)
        {
            try
            {
                var playerService = new PlayerServiceCallback();

                playerService.SeeRequestsEvent += SeeRequest;
                playerService.SendInvitationEvent += SendRequest;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.SearchOfRequests(name);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        private void SeeRequest(List<FriendContract> friends)
        {
            labelNotifications.Content = friends.Count();
            listBoxFriendRequest.ItemsSource = friends;
        }

        public void LoadData(PlayerContract player)
        {
            try
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
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        private void LoadFriendList(List<PlayerContract> _usersOnline)
        {
            
            for (int i = 0; i < _usersOnline.Count(); i++)
            {
                if (_usersOnline[i].nickName == labelPlayer.Content.ToString())
                {
                    FillFriendList(_usersOnline[i]);
                }
            }
        }

        public void FillFriendList(PlayerContract playerContract)
        {
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ChargeListOfFriendsEvent += ChargeListOfFriends;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.GetPlayerList(playerContract);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        public void ChargeListOfFriends(List<PlayerContract> playerContracts)
        {
            listBoxFriends.Items.Clear();
            listBoxFriendsGame.Items.Clear();

            //listBox_FriendsGame.ItemsSource = _usersOnline;
            for (int i = 0; i < playerContracts.Count(); i++)
            {
                if (playerContracts[i].nickName != labelPlayer.Content.ToString())
                {
                    listBoxFriendsGame.Items.Add(playerContracts[i].nickName);
                    listBoxFriends.Items.Add(playerContracts[i].nickName);
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
            borderProfile.Visibility = Visibility.Hidden;
            borderModifyProfile.Visibility = Visibility.Visible;
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ShowProfileImagesEvent += ShowProfileImages;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.GetProfileImages();
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        public void ShowProfileImages(List<ProfieImageContract> profieImageContract)
        {
            listBoxImagesPerfil.ItemsSource = profieImageContract;
            listBoxImagesPerfil.Items.Refresh();
        }

        private void Button_Profile_Click(object sender, RoutedEventArgs e)
        {
            borderProfile.Visibility = Visibility.Visible;
            borderRoomGame.Visibility = Visibility.Hidden;
            borderFriends.Visibility = Visibility.Hidden;
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ShowProfileDataEvent += ShowProfileData;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.ConsultProfile(labelPlayer.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        public void ShowProfileData(PlayerContract playerContract)
        {
            textBlockNicknamePlayer.Text = playerContract.nickName;
            textBlockState.Text = playerContract.state;
            imageProfile.Source = LoadImage(playerContract.imageProfile);
            textBoxState.Text = playerContract.state;
            imageModifyPerfil.Source = LoadImage(playerContract.imageProfile);
            imagePlayer.Source = LoadImage(playerContract.imageProfile);
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
                try
                {
                    var playerService = new PlayerServiceCallback();

                    playerService.SearchPlayerEvent += SendPlayer;
                    _serverPlayerProxy = new ServerPlayerProxy(playerService);
                    _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                    _playerChannel.SearchPlayer(textBoxSearch.Text);
                }
                catch (EndpointNotFoundException ex)
                {
                    MessageBox.Show("Error");
                    log.Add(ex.ToString());
                }
                catch (System.Data.Entity.Core.EntityException ex)
                {
                    MessageBox.Show("Database error");
                    log.Add(ex.ToString());
                }
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
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ConfirmRequestAnsweredEvent += ConfirmRequestAnswered;
                string player = labelPlayer.Content.ToString();
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.SendInvitation(labelPlayer.Content.ToString(), textBoxSearch.Text);

            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        private void SendRequest(bool confirmInvitation)
        {
            GetRequest(labelPlayer.Content.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string friend = (string)listBoxFriendsGame.SelectedItem;
            try
            {
                var playerService = new PlayerServiceCallback();

                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.SendInvitationGame(labelPlayer.Content.ToString(), friend);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
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
            try
            {
                var playerService = new PlayerServiceCallback();

                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.StarGame(labelPlayer.Content.ToString(), playerConfirmation);
                _playerChannel.SendPlayerToStartTurns(labelPlayer.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void buttonAcceptGame_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxInvitations.Items.CurrentPosition;
            PlayerContract playerFriend = (PlayerContract)listBoxInvitations.Items[i];
            try
            {
                var playerService = new PlayerServiceCallback();

                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.AceptInvitation(labelPlayer.Content.ToString(), playerFriend.nickName);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
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
            this.Hide();
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
            try 
            { 
                var decksss = new PlayerServiceCallback();
                decksss.DecksListUpdatedEvent += getListOfDecks;
                _serverPlayerProxy = new ServerPlayerProxy(decksss);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.chargeDecks(labelPlayer.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
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
            try
            {
                var decksss = new PlayerServiceCallback();
                decksss.CardListUpdatedEvent += LoadListOfCards;
                _serverPlayerProxy = new ServerPlayerProxy(decksss);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.LoadCards(cellvalue);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
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
            try
            {
                var decksss = new PlayerServiceCallback();
                decksss.CardListGotEvent += LoadAllCards;

                _serverPlayerProxy = new ServerPlayerProxy(decksss);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.GetAllCards();
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
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
            DeckContract deck = new DeckContract();
            deck.deckName = deckName;
            ValidatorDeck validator = new ValidatorDeck();
            FluentValidation.Results.ValidationResult results = validator.Validate(deck);
            try
            {
                var decksss = new PlayerServiceCallback();
                decksss.DeckSavedEvent += ConfirmDeckSaved;
                _serverPlayerProxy = new ServerPlayerProxy(decksss);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                if (cardCollection.Count() > 12 && results.IsValid)
                {
                    _playerChannel.SaveDeck(cardCollection, deckName, labelPlayer.Content.ToString());
                }
                else
                {
                    MessageBox.Show("he deck must contain 13 cards or more");
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
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
            try
            {
                var decksss = new PlayerServiceCallback();
                decksss.CardListFiltredToUpdateEvent += CardListFiltredToUpdateEvent;
                _serverPlayerProxy = new ServerPlayerProxy(decksss);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.GetCardsToModify(cardNameList);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
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
            try
            {
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
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
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
            loadGameHistoryData();
        }

        public void loadGameHistoryData()
        {
            string userName = labelPlayer.Content.ToString();
            try
            {
                var service = new PlayerServiceCallback();
                service.LoadGameHistoryEvent += loadGameHistory;
                _serverPlayerProxy = new ServerPlayerProxy(service);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.getGameHistory(userName);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
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
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ConfirmRequestAnsweredEvent += ConfirmRequestAnswered;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.AcceptFriendRequest(playerFriend.idFriend);
                _playerChannel.GetPlayers(new PlayerContract
                {
                    nickName = labelPlayer.Content.ToString()
                });
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        private void buttonDenyRequest_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxFriendRequest.Items.CurrentPosition;
            FriendContract playerFriend = (FriendContract)listBoxFriendRequest.Items[i];
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ConfirmRequestAnsweredEvent += ConfirmRequestAnswered;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.DenyFriendRequest(playerFriend.idFriend);
                
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
        }

        public void ConfirmRequestAnswered(List<FriendContract> friendContract)
        {
            listBoxFriendRequest.ItemsSource = friendContract;
            listBoxFriendRequest.Items.Refresh();
            labelNotifications.Content = friendContract.Count();
        }

        private void buttonSaveChangesPerfil_Click(object sender, RoutedEventArgs e)
        {
            PlayerContract player = new PlayerContract();
            player.nickName = labelPlayer.Content.ToString();
            player.state = textBoxState.Text;
            
            if(listBoxImagesPerfil.SelectedItem != null)
            {
                ProfieImageContract profileImage = (ProfieImageContract)listBoxImagesPerfil.SelectedItem;
                player.imageProfile = profileImage.imageProfile;
            }
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ShowProfileDataEvent += ShowProfileData;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.UpdatePlayer(player);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            catch (System.Data.Entity.Core.EntityException ex)
            {
                MessageBox.Show("Database error");
                log.Add(ex.ToString());
            }
            borderModifyProfile.Visibility = Visibility.Hidden;
        }

        private void buttonCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            borderProfile.Visibility = Visibility.Visible;
            borderModifyProfile.Visibility = Visibility.Hidden;
        }

        private void listBoxImagesPerfil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxImagesPerfil.SelectedItem != null)
            {
                ProfieImageContract profileImage = (ProfieImageContract)listBoxImagesPerfil.SelectedItem;
                imageModifyPerfil.Source = LoadImage(profileImage.imageProfile);
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Disconnect();
        }

        public void Disconnect()
        {
            try
            {
                var playerService = new PlayerServiceCallback();
                playerService.ShowProfileDataEvent += ShowProfileData;
                _serverPlayerProxy = new ServerPlayerProxy(playerService);
                _playerChannel = _serverPlayerProxy.ChannelFactory.CreateChannel();
                _playerChannel.DisconnectFromUsersOnline(labelPlayer.Content.ToString());
                //_playerChannel.GetPlayers(new PlayerContract
                //{
                //    nickName = labelPlayer.Content.ToString()
                //});
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }
    }
}