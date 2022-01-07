using ContractsOW;
using Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Interaction logic for PlayGame.xaml
    /// </summary>
    public partial class PlayGame : Window
    {


        private Guid _userId;
        private Guid _friendiId;
        private int _idGame;
        private string _deckName;
        private ServerPlayerProxy _serverDeckProxy;
        private IPlayerService _playerDeckChannel;
        private int minute = 0;
        private int second = 0;
        private int saveGame = 0;
        private string _turn;
        private CardContract _cardToMoveChoice = null;
        private CardContract _cardToMoveOnTable = null;
        private CardContract _cardToMoveOnPlay = null;
        private Log log = new Log();

        public PlayGame()
        {
            //_userId = Guid.NewGuid();
            InitializeComponent();

            
            DispatcherTimer dispatcherTimerSeconds = new DispatcherTimer();
            dispatcherTimerSeconds.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimerSeconds.Tick += Second_tick;
            dispatcherTimerSeconds.Start();
        }

        private void startGameSave()
        {
            GameContract game = new GameContract();
            game.playerName = labelLocalPlayer.Content.ToString();
            game.dateGame = DateTime.Now;
            game.stateGame = "Unfinished";
            game.duration = "00:00";
            game.player1 = labelLocalPlayer.Content.ToString();
            game.player2 = labelPlayerFriend.Content.ToString();
            game.statePlayer1 = "Undefined";
            game.statePlayer2 = "Undefined";
            try
            {
                var service = new PlayerServiceCallback();
                service.GetGameIdEvent += getGameId;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SaveGameData(game);
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

        private void getGameId(int id)
        {
            _idGame = id;
        }

        private void UpdateGameInfo(bool isGameOver, bool isWinner)
        {
            GameContract game = new GameContract();
            game.playerName = labelLocalPlayer.Content.ToString();
            game.idGame = _idGame;
            game.dateGame = DateTime.Now;
            
            game.duration = minute + " : " + second;
            game.player1 = labelLocalPlayer.Content.ToString();
            game.player2 = labelPlayerFriend.Content.ToString();
            
            if (isGameOver)
            {
                game.stateGame = "finished";
                
            }
            else
            {
                game.stateGame = "Unfinished";
            }
            if (isGameOver && isWinner)
            {
                game.statePlayer1 = "Winner";
                game.statePlayer2 = "Loser";
            }
            else if (isGameOver && (!isWinner))
            {
                game.statePlayer1 = "Loser";
                game.statePlayer2 = "Winner";
            }
            try
            {
                var service = new PlayerServiceCallback();
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.UpdateGameData(game);
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

        private void Second_tick(object sender, EventArgs e)
        {
            second++;
            saveGame++;
            if(saveGame == 5)
            {
                UpdateGameInfo(false, false);
                saveGame = 0;
            }
            
            if (second > 59)
            {
                minute++;
                labelMinutes.Content = minute.ToString();
                second = 0;
            }
            labelSeconds.Content = second.ToString();
        }

        public void StartGame(PlayerContract player1, PlayerContract player2)
        {
            labelLocalPlayer.Content = player1.nickName;
            labelPlayerFriend.Content = player2.nickName;
            Guid oldId = player1.id;
            player1.id = Guid.NewGuid();
            _userId = player1.id;
            try
            {
                var service = new PlayerServiceCallback();
                service.ReceiveMessageEvent += ReceiveMessage;
                service.DecksListUpdatedEvent += getListOfDecks;
                service.ReceiveSelectedCardEvent += ReceiveSelectCard;
                service.GetMyIdEvent += GetMyId;
                service.ReceiveAttackEvent += ReceiveAttack;
                service.ReceiveCardToAttacKEvent += ReceiveCardToAttack;
                service.StartFirstTurnEvent += StartFirstTurn;
                service.ReceiveAttackTurnEvent += ReceiveAttackTurn;
                service.ReceiveDefendTurnEvent += ReceiveDefendTurn;
                service.StartExecuteAttackEvent += StartExecuteAttack;
                service.ReceiveChoiceCardTurnEvent += ReceiveChoiceCardTurn;
                service.CleanCardsSurvivalEvent += CleanCardsSurvival;
                service.ShowTotalDamageEvent += ShowTotalDamage;
                service.ReceiveGameOverEvent += ReceiveGameOver;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.chargeDecks(labelLocalPlayer.Content.ToString());
                _playerDeckChannel.UpdateGuid(oldId, player1, player2.nickName);
                startGameSave();
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

        private void ConfirmTurn()
        {
            if (_turn == "Choice1")
            {
                buttonAttackDefend.IsEnabled = true;
                labelPass.Visibility = Visibility.Visible;
                gridCoverListBoxVhoiceCard.Visibility = Visibility.Hidden;
                gridCoverListBoxCardsOnTable.Visibility = Visibility.Hidden;
                gridCoverCardsOnPlay.Visibility = Visibility.Visible;
                gridCoverButton.Visibility = Visibility.Visible;
            }
            else if (_turn == "Choice2")
            {
                buttonAttackDefend.IsEnabled = true;
                labelPass.Visibility = Visibility.Visible;
                gridCoverListBoxVhoiceCard.Visibility = Visibility.Hidden;
                gridCoverListBoxCardsOnTable.Visibility = Visibility.Hidden;
                gridCoverCardsOnPlay.Visibility = Visibility.Visible;
                gridCoverButton.Visibility = Visibility.Visible;
            }
            else if (_turn == "Attack")
            {
                buttonAttackDefend.IsEnabled = true;
                labelPass.Visibility = Visibility.Visible;
                gridCoverListBoxVhoiceCard.Visibility = Visibility.Visible;
                gridCoverListBoxCardsOnTable.Visibility = Visibility.Hidden;
                gridCoverCardsOnPlay.Visibility = Visibility.Hidden;
                gridCoverButton.Visibility = Visibility.Hidden;
            }
            else if (_turn == "Defend")
            {
                buttonAttackDefend.IsEnabled = true;
                labelPass.Visibility = Visibility.Visible;
                gridCoverListBoxVhoiceCard.Visibility = Visibility.Visible;
                gridCoverListBoxCardsOnTable.Visibility = Visibility.Hidden;
                gridCoverCardsOnPlay.Visibility = Visibility.Hidden;
                gridCoverButton.Visibility = Visibility.Hidden;
            }
            else
            {
                buttonAttackDefend.IsEnabled = true;
                labelPass.Visibility = Visibility.Visible;
                gridCoverListBoxVhoiceCard.Visibility = Visibility.Visible;
                gridCoverListBoxCardsOnTable.Visibility = Visibility.Visible;
                gridCoverCardsOnPlay.Visibility = Visibility.Visible;
                gridCoverButton.Visibility = Visibility.Visible;
            }
        }


        private void ReceiveChoiceCardTurn(bool turn)
        {
            _turn = "Choice2";
            ConfirmTurn();
        }


        public void StartTurnsPlayer1(string player1)
        {
            try
            {
                var service = new PlayerServiceCallback();
                service.StartFirstTurnEvent += StartFirstTurn;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.StartTurns(player1);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }

        }

        private void StartFirstTurn(bool startAttack)
        {
            if (startAttack)
            {
                labelPass.Visibility = Visibility.Visible;
                buttonAttackDefend.IsEnabled = true;
                _turn = "Choice1";
                ConfirmTurn();
            }
        }

        private void GetMyId(Guid id)
        {
        }

        private void GetFriendId(PlayerContract player)
        {
            _friendiId = player.id;
        }
        private void getListOfDecks(List<DeckContract> decks)
        {
            listBoxDecks.ItemsSource = decks;
            gridChoiceDecksLocked.Visibility = Visibility.Visible;
        }


        private void listBoxDecks_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DeckContract deck = (DeckContract)listBoxDecks.SelectedItem;
            string cellvalue = deck.deckName;
            _deckName = deck.deckName;
            try
            {
                var service = new PlayerServiceCallback();
                service.CardListUpdatedEvent += LoadListOfCards;
                service.ReceiveMessageEvent += ReceiveMessage;
                service.ReceivCardOnTableEvent += ReceiveCardsOnTable;

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.LoadCards(cellvalue);
                gridChoiceDecksLocked.Visibility = Visibility.Hidden;
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

        private void ReceiveCardsOnTable(byte[] imageOnboard)
        {
            
            //cardAttackEnemy1.Source = LoadImage(imageOnboard);
        }

        private void LoadListOfCards(List<CardContract> cardContractList)
        {
            List<CardContract> initializeCardOnHand = new List<CardContract>();
            //listBoxCardsToChoice.ItemsSource = cardContractList;
            initializeCardOnHand.Add(cardContractList[0]);
            initializeCardOnHand.Add(cardContractList[1]);
            initializeCardOnHand.Add(cardContractList[2]);
            listBoxChoiceCard.ItemsSource = initializeCardOnHand;

            //listBoxGetRandomCard.ItemsSource = cardContractList;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updateId();
            SendTheMessage();
        }

        public void updateId()
        {
            try
            {
                var service = new PlayerServiceCallback();
                service.GetFriendIdEvent += GetFriendId;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                string playerRecevier = labelPlayerFriend.Content.ToString();
                _playerDeckChannel.UpdateIdFriend(playerRecevier);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }


        public void SendTheMessage()
        {
            try
            {
                var service = new PlayerServiceCallback();
                service.ReceiveMessageEvent += ReceiveMessage;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                string playerSender = labelLocalPlayer.Content.ToString();
                string playerRecevier = labelPlayerFriend.Content.ToString();
                PlayerContract playerHost = new PlayerContract();
                playerHost.nickName = playerSender;
                playerHost.id = _userId;
                PlayerContract playerFriend = new PlayerContract();
                playerFriend.nickName = playerRecevier;
                playerFriend.id = _friendiId;
                string message = textBoxChat.Text;
                _playerDeckChannel.sendMessageInGame(playerHost, playerFriend, message);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
            textBoxChat.Text = "";
        }
        private void ReceiveMessage(string playerSender, string message)
        {
            listBoxChat.Items.Add(string.Format("{0} > {1}", playerSender, message));
        }


        private void ReceiveSelectCard(List<CardContract> cardList)
        {
            listBoxCardsEnemyOnTable.ItemsSource = cardList;
        }

        private void SendCardsOnTableList()
        {
            List<CardContract> cardList = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
            try
            {
                var service = new PlayerServiceCallback();
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendSelectCard(cardList, labelPlayerFriend.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }


        private void listBoxCardsOnTable_MouseUp(object sender, MouseButtonEventArgs e)
        {
            List<CardContract> cardList = new List<CardContract>();
            List<CardContract> cardListRemove = new List<CardContract>();
            gridShowCardSelected.Visibility = Visibility.Hidden;

            if (_cardToMoveChoice != null && _cardToMoveChoice.nameCard != null && listBoxCardsOnTable.ItemsSource != null)
            {

                cardList = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
                cardList.Add(_cardToMoveChoice);
                listBoxCardsOnTable.ItemsSource = cardList;
                _cardToMoveChoice = null;
                ChangeTurnToChoiceCard();
                SendCardsOnTableList();
            }
            else if (_cardToMoveChoice != null && _cardToMoveChoice.nameCard != null && listBoxCardsOnTable.ItemsSource == null)
            {
                cardList.Add(_cardToMoveChoice);
                listBoxCardsOnTable.ItemsSource = cardList;
                _cardToMoveChoice = null;
                ChangeTurnToChoiceCard();
                SendCardsOnTableList();
            }
            else if (_cardToMoveOnPlay != null && _cardToMoveOnPlay.nameCard != null && listBoxCardsOnTable.ItemsSource == null)
            {
                cardListRemove = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
                cardList = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
                cardList.Add(_cardToMoveOnPlay);
                cardListRemove.Remove(_cardToMoveOnPlay);
                listBoxCardsOnTable.ItemsSource = cardList;
                listBoxCardsOnPlay.ItemsSource = cardListRemove;
                _cardToMoveOnPlay = null;
                listBoxCardsOnPlay.Items.Refresh();
            }
            else if (listBoxCardsOnTable.SelectedItem != null)
            {
                CardContract card = (CardContract)listBoxCardsOnTable.SelectedItem;
                _cardToMoveOnTable = card;
            }
            listBoxCardsOnTable.Items.Refresh();
            SendCardsInPlay();
            _ = ConfirmPermissionToAttack();
        }


        private void listBoxCardsOnPlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            gridShowCardSelected.Visibility = Visibility.Hidden;
            if (_cardToMoveOnTable != null && _cardToMoveOnTable.nameCard != null && listBoxCardsOnPlay.ItemsSource != null && listBoxCardsOnPlay.SelectedItem != null)
            {
                FillListBoxCardsOnPlay(1);
            }
            else if (_cardToMoveOnTable != null && _cardToMoveOnTable.nameCard != null && listBoxCardsOnPlay.ItemsSource == null)
            {
                FillListBoxCardsOnPlay(2);
            }
            else if (listBoxCardsOnPlay.SelectedItem != null)
            {
                CardContract card = (CardContract)listBoxCardsOnPlay.SelectedItem;
                _cardToMoveOnPlay = card;
            }
            listBoxCardsOnPlay.Items.Refresh();
            SendCardsOnTableList();

            _ = ConfirmPermissionToAttack();
        }

        private void FillListBoxCardsOnPlay(int num)
        {
            List<CardContract> cardList = new List<CardContract>();
            List<CardContract> cardListRemove = new List<CardContract>();
            if (num == 1)
            {
                cardList = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
                cardListRemove = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
                
                string cardName;
                try
                {
                    CardContract card = (CardContract)listBoxCardsOnPlay.SelectedItem;
                    cardName = card.nameCard;
                }
                catch (NullReferenceException)
                {
                    cardName = "";
                }
                int i = listBoxCardsOnPlay.SelectedIndex;
                int j = cardList.Count();
                if (_turn == "Attack")
                {
                    cardList.Add(_cardToMoveOnTable);
                    
                }
                else if (_turn == "Defend" && cardName == null && (i < j || i == j))
                {
                    
                    cardList.RemoveAt(i);
                    cardList.Insert(i, _cardToMoveOnTable);
                    cardListRemove.Remove(_cardToMoveOnTable);
                }
                
                listBoxCardsOnPlay.ItemsSource = cardList;
                listBoxCardsOnTable.ItemsSource = cardListRemove;
                _cardToMoveOnTable = null;
                listBoxCardsOnTable.Items.Refresh();
                SendCardsInPlay();
            }
            else if (num == 2)
            {
                cardListRemove = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
                cardList.Add(_cardToMoveOnTable);
                cardListRemove.Remove(_cardToMoveOnTable);
                listBoxCardsOnPlay.ItemsSource = cardList;
                listBoxCardsOnTable.ItemsSource = cardListRemove;
                _cardToMoveOnTable = null;
                listBoxCardsOnTable.Items.Refresh();
                SendCardsInPlay();
            }
        }

        private void listBoxChoiceCard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_cardToMoveChoice ==  null)
            {
                gridShowCardSelected.Visibility = Visibility.Visible;
                CardContract card = (CardContract)listBoxChoiceCard.SelectedItem;
                List<CardContract> cardList = new List<CardContract>();
                cardList.Add(card);
                _cardToMoveChoice = card;
                listBoxCardToChoice.ItemsSource = cardList;
                List<CardContract> cardListSource = (List<CardContract>)listBoxChoiceCard.ItemsSource;
                cardListSource.Remove(card);
                listBoxChoiceCard.ItemsSource = cardListSource;
                listBoxChoiceCard.Items.Refresh();
            }
        }

        private void ChangeTurnToChoiceCard()
        {
            try
            {
                var service = new PlayerServiceCallback();

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                if (_turn == "Choice1")
                {
                    _playerDeckChannel.ChangeChoiceCardTurns(labelPlayerFriend.Content.ToString());
                    buttonAttackDefend.IsEnabled = false;
                    labelPass.Visibility = Visibility.Hidden;
                    gridCoverListBoxVhoiceCard.Visibility = Visibility.Visible;
                    gridCoverListBoxCardsOnTable.Visibility = Visibility.Visible;
                    gridCoverCardsOnPlay.Visibility = Visibility.Visible;
                }
                else if (_turn == "Choice2")
                {
                    _playerDeckChannel.PassTurnToAttack(labelPlayerFriend.Content.ToString());
                    buttonAttackDefend.IsEnabled = false;
                    labelPass.Visibility = Visibility.Hidden;
                    gridCoverListBoxVhoiceCard.Visibility = Visibility.Visible;
                    gridCoverListBoxCardsOnTable.Visibility = Visibility.Visible;
                    gridCoverCardsOnPlay.Visibility = Visibility.Visible;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void SendCardsInPlay()
        {
            List<CardContract> cardList = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
            try
            {
                var service = new PlayerServiceCallback();

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendCardToAttack(labelPlayerFriend.Content.ToString(), cardList);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void ReceiveCardToAttack(List<CardContract> cardList)
        {
            listBoxCardsEnemyOnPlay.ItemsSource = cardList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool permission = ConfirmPermissionToAttack();
            if (permission && _turn == "Attack" && labelPass.Visibility == Visibility.Hidden)
            {
                PassToDefend();
            }
            else if (permission && _turn == "Defend" && labelPass.Visibility == Visibility.Hidden)
            {
                ChageTurn();
            }
            else if (_turn == "Attack" && labelPass.Visibility == Visibility.Visible)
            {
                ExecutePass();
            }
            else if (permission && _turn == "Defend" && labelPass.Visibility == Visibility.Visible)
            {
                ChageTurn();
            }
            _turn = null;
            ConfirmTurn();
            _ = ConfirmPermissionToAttack();
        }

        private bool ConfirmPermissionToAttack()
        {
            bool permission = false;
            List<CardContract> cardList = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
            if(cardList != null && cardList.Count() > 0)
            {
                permission = true;
            }
            if (permission && _turn == "Attack")
            {
                swordIcon.Visibility = Visibility.Visible;
                labelPass.Visibility = Visibility.Hidden;
            }
            else if (permission && _turn == "Defend")
            {
                shieldIcon.Visibility = Visibility.Visible;
                labelPass.Visibility = Visibility.Hidden;
            }
            else
            {
                swordIcon.Visibility = Visibility.Hidden;
                shieldIcon.Visibility = Visibility.Hidden;
                labelPass.Visibility = Visibility.Visible;
            }
            return permission;
        }

        private void ExecutePass()
        {
            StartTurnsPlayer1(labelPlayerFriend.Content.ToString());
        }

        private void ChageTurn()
        {
            SendExecuteAttack();
            StartTurnsPlayer1(labelLocalPlayer.Content.ToString());
        }

        private void PassToDefend()
        {
            try
            {
                var service = new PlayerServiceCallback();
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.PassTurnToDefend(labelPlayerFriend.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        public void ReceiveDefendTurn(bool changeTurn)
        {
            if (changeTurn)
            {
                _turn = "Defend";
                ConfirmTurn();
                FillWithEmptyCards();
            }
        }

        public void FillWithEmptyCards()
        {
            List<CardContract> cardList = (List<CardContract>)listBoxCardsEnemyOnPlay.ItemsSource;
            if (cardList != null && _turn == "Defend")
            {
                List<CardContract> cardListNull = new List<CardContract>();
                for (int i = 0; i < cardList.Count(); i++)
                {
                    cardListNull.Add(new CardContract());
                }
                listBoxCardsOnPlay.ItemsSource = cardListNull;
            }
        }

        public void ReceiveAttackTurn(bool changeTurn)
        {
            if (changeTurn)
            {
                _turn = "Attack";
                _ = ShowNewTurnLabel();
                ConfirmTurn();
            }
        }


        private void SendExecuteAttack()
        {
            try
            {
                var service = new PlayerServiceCallback();
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.ExecuteAttack(labelPlayerFriend.Content.ToString());
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void StartExecuteAttack(bool startAttack)
        {
            if (startAttack)
            {
                SendAttackClick();
            }
        }

        private void SendAttackClick()
        {
            List<CardContract> cardList = new List<CardContract>();
            cardList = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
            try
            {
                var service = new PlayerServiceCallback();

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendAttack(labelPlayerFriend.Content.ToString(), cardList);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }

        }

        private void ReceiveAttack(List<CardContract> cardsToAttack)
        {
            int damage = 0;
            string actualLifeString = labelLocalLife.Content.ToString();
            
            damage = BlockAttack(cardsToAttack);
            
            if (damage > 0)
            {
                _ = ShowTotalDagameDealtAsync(damage);
            }
            SendTotalDamage(damage);
            int actualLife = int.Parse(actualLifeString);
            actualLife = actualLife - damage;
            labelLocalLife.Content = actualLife;
            bool isGameOver = GameOver(actualLife);
            if (isGameOver)
            {
                FinallyGame();
            }
        }

        private void FinallyGame()
        {
            try
            {
                var service = new PlayerServiceCallback();
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendGameOver(labelPlayerFriend.Content.ToString());
                ReceiveGameOver(true);
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

        private void SendTotalDamage(int damage)
        {
            try
            {
                var service = new PlayerServiceCallback();

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendTotalDamageDealt(labelPlayerFriend.Content.ToString(), damage);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void ShowTotalDamage(int damage)
        {
            string actualLifeString = labelEnemyLife.Content.ToString();
            if (damage > 0)
            {
                _ = ShowTotalDagameDealtEnemyAsync(damage);
            }
            int actualLife = int.Parse(actualLifeString);
            actualLife = actualLife - damage;
            labelEnemyLife.Content = actualLife;
        }

        private async Task ShowTotalDagameDealtAsync(int damage)
        {
            labelTotalDamageDealt.Content = string.Format("-{0}",damage);
            labelTotalDamageDealt.Visibility = Visibility.Visible;
            await Task.Delay(1800);
            labelTotalDamageDealt.Visibility = Visibility.Hidden;
        }

        private async Task ShowTotalDagameDealtEnemyAsync(int damage)
        {
            labelTotalDamageDealtEnemy.Content = string.Format("-{0}", damage);
            labelTotalDamageDealtEnemy.Visibility = Visibility.Visible;
            await Task.Delay(1800);
            labelTotalDamageDealtEnemy.Visibility = Visibility.Hidden;
        }


        private int BlockAttack(List<CardContract> cardsToAttack)
        {
            bool isBlocked = false;
            int damage = 0;
            List<CardContract> cardsToDefend = new List<CardContract>();
            List<CardContract> finalCardsAttacker = new List<CardContract>();
            List<CardContract> finalCardsDefender = new List<CardContract>();

            if (listBoxCardsOnPlay.ItemsSource != null)
            {
                cardsToDefend = (List<CardContract>)listBoxCardsOnPlay.ItemsSource;
            }

            if (cardsToDefend.Count() > 0)
            {
                isBlocked = true;
            }
            for (int i = 0; i < cardsToDefend.Count(); i++)
            {
                if (isBlocked && (cardsToDefend[i] != null))
                {
                    if (cardsToDefend[i].damage < cardsToAttack[i].damage)
                    {
                        cardsToAttack[i].damage = cardsToAttack[i].damage - cardsToDefend[i].damage;
                        finalCardsAttacker.Add(cardsToAttack[i]);
                        damage += cardsToAttack[i].damage;
                    }
                    else if (cardsToDefend[i].damage > cardsToAttack[i].damage)
                    {
                        cardsToDefend[i].damage = cardsToDefend[i].damage - cardsToAttack[i].damage;
                        finalCardsDefender.Add(cardsToDefend[i]);
                    }
                }
                else
                {
                    damage += cardsToAttack[i].damage;
                    finalCardsAttacker.Add(cardsToAttack[i]);
                }
            }
            ReturnCardListWithLive(finalCardsAttacker);
            CleanCardsSurvival(finalCardsDefender);
            
            return damage;
        }

        private void ReturnCardListWithLive(List<CardContract> cardListAttack)
        {
            try
            {
                var service = new PlayerServiceCallback();

                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.SendSurvivalCards(labelPlayerFriend.Content.ToString(), cardListAttack);
            }
            catch (EndpointNotFoundException ex)
            {
                MessageBox.Show("Error");
                log.Add(ex.ToString());
            }
        }

        private void CleanCardsSurvival(List<CardContract> cardsSurvival)
        {
            listBoxCardsOnPlay.ItemsSource = null;
            listBoxCardsEnemyOnPlay.ItemsSource = null;
            List<CardContract> cards = (List<CardContract>)listBoxCardsOnTable.ItemsSource;
            if (cardsSurvival != null)
            {
                for (int i = 0; i < cardsSurvival.Count(); i++)
                {
                    cards.Add(cardsSurvival[i]);
                }
            }
            listBoxCardsOnTable.ItemsSource = cards;
            listBoxCardsOnTable.Items.Refresh();
            SendCardsOnTableList();
            if (_deckName != null)
            {
                GetNewCard();
            }
        }

        private async Task ShowNewTurnLabel()
        {
            labelNewTurn.Visibility = Visibility.Visible;
            await Task.Delay(4000);
            labelNewTurn.Visibility = Visibility.Hidden;
        }

        private void GetNewCard()
        {
            try
            {
                var service = new PlayerServiceCallback();
                service.GetCardToNewTurnEvent += GetCardToNewTurn;
                _serverDeckProxy = new ServerPlayerProxy(service);
                _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
                _playerDeckChannel.AddCardInNewTurn(_deckName);
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

        private void GetCardToNewTurn(CardContract card)
        {
            List<CardContract> cardList = (List<CardContract>)listBoxChoiceCard.ItemsSource;
            cardList.Add(card);
            listBoxChoiceCard.ItemsSource = cardList;
            listBoxChoiceCard.Items.Refresh();
        }


        private bool GameOver(int playerLife)
        {
            bool isGameOver = false;
            if (playerLife == 0 || playerLife < 0)
            {
                isGameOver = true;
            }
            return isGameOver;
        }

        private void ReceiveGameOver(bool isGameOver)
        {
            string life = labelLocalLife.Content.ToString();
            int myLife = int.Parse(life);
            bool isWinner = false;
            _turn = null;
            if (myLife > 0)
            {
                isWinner = true;
            }
            if (isGameOver && isWinner)
            {
                gridYouAreTheWinner.Visibility = Visibility.Visible;
            }
            else if (isGameOver && (!isWinner))
            {
                gridGameOver.Visibility = Visibility.Visible;
            }
            UpdateGameInfo(isGameOver, isWinner);
        }


        private void CloseWindowWhenEndTheGame()
        {
            var service = new PlayerServiceCallback();
            service.OpenMainWindowEvent += OpenMainWindow;
            _serverDeckProxy = new ServerPlayerProxy(service);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.GetOutGameWindow(labelLocalPlayer.Content.ToString());
        }

        public void OpenMainWindow(PlayerContract player)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.LoadData(player);
            mainWindow.GetName(labelLocalPlayer.Content.ToString());
            mainWindow.ReceiveRequest();
            this.Close();
            mainWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CloseWindowWhenEndTheGame();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CloseWindowWhenEndTheGame();
        }
    }
}
