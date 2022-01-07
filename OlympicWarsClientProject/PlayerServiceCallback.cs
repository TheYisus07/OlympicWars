using ContractsOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlympicWarsClientProject
{
    public class PlayerServiceCallback : IPlayerServiceCallback
    {
        public delegate void DecksListUpdatedDelegate(List<DeckContract> decks);
        public event DecksListUpdatedDelegate DecksListUpdatedEvent;

        public delegate void PlayersListUpdatedDelegate(bool access, string player);
        public event PlayersListUpdatedDelegate PlayersListUpdatedEvent;

        public delegate void CardsListUpdatedDelegate(List<CardContract> cardContractList);
        public event CardsListUpdatedDelegate CardListUpdatedEvent;

        public delegate void CardsListGotDelegate(List<CardContract> cardContractList);
        public event CardsListGotDelegate CardListGotEvent;

        public delegate void DeckSavedDelegate(int numberEntrie);
        public event DeckSavedDelegate DeckSavedEvent;

        public delegate void CardListFiltredToUpdateDelegate(List<CardContract> cardContractList);
        public event CardListFiltredToUpdateDelegate CardListFiltredToUpdateEvent;

        public delegate void DeckUpdatedDelegate(int numberEntrie);
        public event DeckUpdatedDelegate DeckUpdatedEvent;

        public delegate void ReceiveConfirmactionDelegate(string playerconfirmation);
        public event ReceiveConfirmactionDelegate ReceiveConfirmationEvent;

        public delegate void LoadGameDelegate(PlayerContract player1, PlayerContract player2);
        public event LoadGameDelegate LoadGameEvent;

        public delegate void ReceiveMessageDelegate(string playerSender, string message);
        public event ReceiveMessageDelegate ReceiveMessageEvent;

        public delegate void GetFriendIdDelegate(PlayerContract player);
        public event GetFriendIdDelegate GetFriendIdEvent;

        public delegate void GetMyIdDelegate(Guid id);
        public event GetMyIdDelegate GetMyIdEvent;

        public delegate void ReceiveCardOnTableDelegate(byte[] image);
        public event ReceiveCardOnTableDelegate ReceivCardOnTableEvent;

        public delegate void LoadGameHistoryDelegate(List<GameContract> gameHistory);
        public event LoadGameHistoryDelegate LoadGameHistoryEvent;

        public delegate void GetGameIdDelegate(int id);
        public event GetGameIdDelegate GetGameIdEvent;

        public delegate void ReceiveSelectedCardDelegate(List<CardContract> cardList);
        public event ReceiveSelectedCardDelegate ReceiveSelectedCardEvent;

        public delegate void ReceiveAttackDelegate(List<CardContract> cardsToAttack);
        public event ReceiveAttackDelegate ReceiveAttackEvent;

        public delegate void ReceiveCardToAttackDelegate(List<CardContract> cardList);
        public event ReceiveCardToAttackDelegate ReceiveCardToAttacKEvent;

        public delegate void StartFirstTurnDelegate(bool startAttack);
        public event StartFirstTurnDelegate StartFirstTurnEvent;

        public delegate void StartTurnDelegate(bool startAttack, string player1);
        public event StartTurnDelegate StartTurnEvent;

        public delegate void ReceiveDefendTurnDelegate(bool changeTurn);
        public event ReceiveDefendTurnDelegate ReceiveDefendTurnEvent;

        public delegate void ReceiveAttackTurnDelegate(bool changeTurn);
        public event ReceiveAttackTurnDelegate ReceiveAttackTurnEvent;

        public delegate void StartExecuteAttackDelegate(bool startAttack);
        public event StartExecuteAttackDelegate StartExecuteAttackEvent;

        public delegate void ReceiveChoiceCardTurnDelegate(bool turn);
        public event ReceiveChoiceCardTurnDelegate ReceiveChoiceCardTurnEvent;

        public delegate void CleanCardsSurvivalDelegate(List<CardContract> cardsSurvival);
        public event CleanCardsSurvivalDelegate CleanCardsSurvivalEvent;

        public delegate void GetCardToNewTurnDelegate(CardContract card);
        public event GetCardToNewTurnDelegate GetCardToNewTurnEvent;

        public delegate void ShowTotalDamageDelegate(int damage);
        public event ShowTotalDamageDelegate ShowTotalDamageEvent;

        public delegate void ReceiveGameOverDelegate(bool isGameOver);
        public event ReceiveGameOverDelegate ReceiveGameOverEvent;




        public delegate void ConfirmRegistrationDelegate(int confirmPlayer);
        public event ConfirmRegistrationDelegate ConfirmRegistrationEvent;

        public delegate void ConfirmLoginDelegate(PlayerContract player, bool confirmLogin);
        public event ConfirmLoginDelegate ConfirmLoginEvent;

        public delegate void LoadFriendListDelegate(List<PlayerContract> _playersOnline);
        public event LoadFriendListDelegate LoadFriendListEvent;

        public delegate void SearchPlayerDelegate(PlayerContract player, bool isFound);
        public event SearchPlayerDelegate SearchPlayerEvent;

        public delegate void SendInvitationDelegate(bool confirmInvitation);
        public event SendInvitationDelegate SendInvitationEvent;

        public delegate void SeeRequestsDelegate(List<FriendContract> _friends);
        public event SeeRequestsDelegate SeeRequestsEvent;

        public delegate void ReceiveInvitationDelegate(string nameInvitation, PlayerContract friend);
        public event ReceiveInvitationDelegate ReceiveInvitationEvent;


        public delegate void ConfirmRequestAnsweredDelegate(List<FriendContract> friendContract);
        public event ConfirmRequestAnsweredDelegate ConfirmRequestAnsweredEvent;

        public delegate void ChargeListOfFriendsDelegate(List<PlayerContract> playerContract);
        public event ChargeListOfFriendsDelegate ChargeListOfFriendsEvent;

        public delegate void ShowProfileDataDelegate(PlayerContract playerContract);
        public event ShowProfileDataDelegate ShowProfileDataEvent;

        public delegate void ShowProfileImagesDelegate(List<ProfieImageContract> profieImageContract);
        public event ShowProfileImagesDelegate ShowProfileImagesEvent;

        public delegate void OpenMainWindowDelegate(PlayerContract playerContract);
        public event OpenMainWindowDelegate OpenMainWindowEvent;

        public void getListOfDecks(List<DeckContract> decks)
        {
            if (DecksListUpdatedEvent != null)
            {
                DecksListUpdatedEvent(decks);
            }
        }

        public void OpenWindow(bool access, string player)
        {
            if (PlayersListUpdatedEvent != null)
            {
                PlayersListUpdatedEvent(access, player);
            }
        }

        public void LoadListOfCards(List<CardContract> cardContractList)
        {
            if (CardListUpdatedEvent != null)
            {
                CardListUpdatedEvent(cardContractList);
            }
        }

        public void LoadListOfAllCards(List<CardContract> cardContractList)
        {
            if (CardListGotEvent != null)
            {
                CardListGotEvent(cardContractList);
            }
        }

        public void ConfirmDeckSaved(int numberEntries)
        {
            if (DeckSavedEvent != null)
            {
                DeckSavedEvent(numberEntries);
            }
        }

        public void LoadCardsToModify(List<CardContract> cardList)
        {
            if (CardListFiltredToUpdateEvent != null)
            {
                CardListFiltredToUpdateEvent(cardList);
            }
        }

        public void ConfirmDeckUpdated(int numerEntries)
        {
            if (DeckUpdatedEvent != null)
            {
                DeckUpdatedEvent(numerEntries);
            }
        }


        public void receiveConfirmation(string playerConfirmation)
        {
            if (ReceiveConfirmationEvent != null)
            {
                ReceiveConfirmationEvent(playerConfirmation);
            }
        }

        public void LoadGame(PlayerContract playe1, PlayerContract player2)
        {
            if (LoadGameEvent != null)
            {
                LoadGameEvent(playe1, player2);
            }
        }

        public void ReceiveMessage(string playerSender, string message)
        {
            if (ReceiveMessageEvent != null)
            {
                ReceiveMessageEvent(playerSender, message);
            }
        }

        public void GetFriendId(PlayerContract player)
        {
            if (GetFriendIdEvent != null)
            {
                GetFriendIdEvent(player);
            }
        }

        public void GetMyId(Guid id)
        {
            if (GetMyIdEvent != null)
            {
                GetMyIdEvent(id);
            }
        }

        public void ReceiveCardsOnTable(byte[] imageOnboard)
        {
            if (ReceivCardOnTableEvent != null)
            {
                ReceivCardOnTableEvent(imageOnboard);
            }
        }

        public void loadGameHistory(List<GameContract> gameHistory)
        {
            if (LoadGameHistoryEvent != null)
            {
                LoadGameHistoryEvent(gameHistory);
            }
        }

        public void updateTime()
        {
            throw new NotImplementedException();
        }

        public void getGameId(int id)
        {
            if (GetGameIdEvent != null)
            {
                GetGameIdEvent(id);
            }
        }

        public void ReceiveSelectCard(List<CardContract> cardList)
        {
            if (ReceiveSelectedCardEvent != null)
            {
                ReceiveSelectedCardEvent(cardList);
            }
        }

        public void ReceiveAttack(List<CardContract> cardsToAttack)
        {
            if (ReceiveAttackEvent != null)
            {
                ReceiveAttackEvent(cardsToAttack);
            }
        }

        public void ReceiveCardToAttack(List<CardContract> cardList)
        {
            if (ReceiveCardToAttacKEvent != null)
            {
                ReceiveCardToAttacKEvent(cardList);
            }
        }

        public void StartFirstTurn(bool startAttack)
        {
            if (StartFirstTurnEvent != null)
            {
                StartFirstTurnEvent(startAttack);
            }
        }

        public void StartTurn(bool startAttack, string player1)
        {
            if (StartTurnEvent != null)
            {
                StartTurnEvent(startAttack, player1);
            }
        }

        public void ReceiveDefendTurn(bool changeTurn)
        {
            if (ReceiveDefendTurnEvent != null)
            {
                ReceiveDefendTurnEvent(changeTurn);
            }
        }

        public void ReceiveAttackTurn(bool changeTurn)
        {
            if (ReceiveAttackTurnEvent != null)
            {
                ReceiveAttackTurnEvent(changeTurn);
            }
        }

        public void StartExecuteAttack(bool attack)
        {
            if (StartExecuteAttackEvent != null)
            {
                StartExecuteAttackEvent(attack);
            }
        }

        public void ReceiveChoiceCardTurn(bool turn)
        {
            if (ReceiveChoiceCardTurnEvent != null)
            {
                ReceiveChoiceCardTurnEvent(turn);
            }
        }

        public void CleanCardsSurvival(List<CardContract> cardsSurvival)
        {
            if (CleanCardsSurvivalEvent != null)
            {
                CleanCardsSurvivalEvent(cardsSurvival);
            }
        }

        public void GetCardToNewTurn(CardContract card)
        {
            if (GetCardToNewTurnEvent != null)
            {
                GetCardToNewTurnEvent(card);
            }
        }

        public void ShowTotalDamage(int damage)
        {
            if (ShowTotalDamageEvent != null)
            {
                ShowTotalDamageEvent(damage);
            }
        }

        public void ReceiveGameOver(bool isGameOver)
        {
            if (ReceiveGameOverEvent != null)
            {
                ReceiveGameOverEvent(isGameOver);
            }
        }



        public void ConfirmRegistration(int confirmPlayer)
        {
            if (ConfirmRegistrationEvent != null)
            {
                ConfirmRegistrationEvent(confirmPlayer);
            }
        }

        public void ConfirmLogin(PlayerContract player, bool confirmLogin)
        {
            if (ConfirmLoginEvent != null)
            {
                ConfirmLoginEvent(player, confirmLogin);
            }
        }

        public void LoadFriendList(List<PlayerContract> _playersOnline)
        {
            if (LoadFriendListEvent != null)
            {
                LoadFriendListEvent(_playersOnline);
            }
        }
        public void SendPlayer(PlayerContract player, bool isFound)
        {
            if (SearchPlayerEvent != null)
            {
                SearchPlayerEvent(player, isFound);
            }
        }

        public void SendRequest(bool confirmInvitation)
        {
            if (SendInvitationEvent != null)
            {
                SendInvitationEvent(confirmInvitation);
            }
        }

        public void SeeRequests(List<FriendContract> _friends)
        {
            if (SeeRequestsEvent != null)
            {
                SeeRequestsEvent(_friends);
            }
        }

        public void ReceiveInvitation(string invitationName, PlayerContract friend)
        {
            if (ReceiveInvitationEvent != null)
            {
                ReceiveInvitationEvent(invitationName, friend);
            }
        }

        public void ConfirmRequestAnswered(List<FriendContract> friendContract)
        {
            if (ConfirmRequestAnsweredEvent != null)
            {
                ConfirmRequestAnsweredEvent(friendContract);
            }
        }

        public void ChargeListOfFriends(List<PlayerContract> playerContracts)
        {
            if (ChargeListOfFriendsEvent != null)
            {
                ChargeListOfFriendsEvent(playerContracts);
            }

        }

        public void ShowProfileData(PlayerContract playerContract)
        {
            if (ShowProfileDataEvent != null)
            {
                ShowProfileDataEvent(playerContract);
            }
        }

        public void ShowProfileImages(List<ProfieImageContract> profieImageContract)
        {
            if (ShowProfileImagesEvent != null)
            {
                ShowProfileImagesEvent(profieImageContract);
            }
        }

        public void OpenMainWindow(PlayerContract player)
        {
            if (OpenMainWindowEvent != null)
            {
                OpenMainWindowEvent(player);
            }
        
        }

    }

}
