using ContractsOW;
using Server;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DeckWindow : Window
    {

        private ServerPlayerProxy _serverDeckProxy;
        private IPlayerService _playerDeckChannel;
        public DeckWindow()
        {
            InitializeComponent();
        }

        public void loadDataGridDecks(string player)
        {
            labelPlayerName.Content = player;
            var decksss = new PlayerServiceCallback();
            decksss.DecksListUpdatedEvent += getListOfDecks;
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();

            try
            {
                _playerDeckChannel.chargeDecks(player);
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
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.LoadCards(cellvalue);
        }

        private void LoadListOfCards(List<CardContract> cardContractList)
        {
            listBoxCardFromDeck.ItemsSource = cardContractList;
            buttonModifyDeck.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridAddDeck.Visibility = Visibility.Visible;
            buttonSaveDeck.Visibility = Visibility.Visible;
            gridCards.Visibility = Visibility.Visible;
            var decksss = new PlayerServiceCallback();
            decksss.CardListGotEvent += LoadAllCards;

            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.GetAllCards();
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
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            if (cardCollection.Count() > 12)
            {
                _playerDeckChannel.SaveDeck(cardCollection, deckName, labelPlayerName.Content.ToString());
            }
            else
            {
                MessageBox.Show("he deck must contain 13 cards or more");
            }
            this.loadDataGridDecks(labelPlayerName.Content.ToString());
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
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            _playerDeckChannel.GetCardsToModify(cardNameList);
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
            _serverDeckProxy = new ServerPlayerProxy(decksss);
            _playerDeckChannel = _serverDeckProxy.ChannelFactory.CreateChannel();
            if (cardList.Count() > 12)
            {
                _playerDeckChannel.UpdateDeck(cardList, deckName);
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
    }
}
