using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [ServiceContract(CallbackContract = typeof(IPlayerServiceCallback))]
    public interface IPlayerService
    {
        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // </param>
        [OperationContract(IsOneWay = true)]
        void chargeDecks(string player);

        // <sumary>
        // Load cards of a deck
        // </sumary>
        // <param name = "deckName">
        // </param>
        [OperationContract(IsOneWay = true)]
        void LoadCards(string deckName);

        // <sumary>
        // get all cards
        // </sumary>
        [OperationContract(IsOneWay = true)]
        void GetAllCards();

        // <sumary>
        // save deck in database
        // </sumary>
        // <param name = "cardList">
        // </param>
        // <param name = "deckName">
        // </param>
        // <param name = "playerName">
        // </param>
        [OperationContract(IsOneWay = true)]
        void SaveDeck(List<string> cardList, string deckName, string playerName);

        // <sumary>
        // get card to modify
        // </sumary>
        // <param name = "cardList">
        // </param>
        [OperationContract(IsOneWay = true)]
        void GetCardsToModify(List<string> cardList);

        // <sumary>
        // update a deck
        // </sumary>
        // <param name = "cardList">
        // </param>
        // <param name = "deckName">
        // </param>
        [OperationContract(IsOneWay = true)]
        void UpdateDeck(List<CardContract> cardList, string deckName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void StarGame(string player1, string player2);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void sendMessageInGame(PlayerContract playerSender, PlayerContract playerReceiver, string message);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void UpdateGuid(Guid id, PlayerContract name, string nameToSend);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void UpdateIdFriend(string name);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void getGameHistory(string playerName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SaveGameData(GameContract game);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void UpdateGameData(GameContract game);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendSelectCard(List<CardContract> cardList, string playerName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendAttack(string player, List<CardContract> cardsToAttack);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendCardToAttack(string player, List<CardContract> cardList);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void StartTurns(string player1);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendPlayerToStartTurns(string player1);


        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void PassTurnToDefend(string player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void PassTurnToAttack(string player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void ExecuteAttack(string player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void ChangeChoiceCardTurns(string player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendSurvivalCards(string player, List<CardContract> cardListAttack);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void AddCardInNewTurn(string deckName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendTotalDamageDealt(string playerName, int damage);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendGameOver(string playerName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void RegisterUser(PlayerContract player);

        [OperationContract(IsOneWay = true)]
        void LoginUser(PlayerContract player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void GetPlayers(PlayerContract userLogin);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SearchPlayer(string playerName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendInvitation(string playerSend, string playerReceive);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SearchOfRequests(string playerName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void SendInvitationGame(string playerName, string frienName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void AceptInvitation(string playerAcepted, string playerHost);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void AcceptFriendRequest(int id);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void DenyFriendRequest(int id);

        [OperationContract(IsOneWay = true)]
        void GetPlayerList(PlayerContract playerContract);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void ConsultProfile(string nickName);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void GetProfileImages();

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void UpdatePlayer(PlayerContract player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void DisconnectFromUsersOnline(string player);

        // <sumary>
        // Charge decks fo a player
        // </sumary>
        // <param name = "player">
        // Friend request to send
        // </param>
        [OperationContract(IsOneWay = true)]
        void GetOutGameWindow(string player);

    }


    [ServiceContract]
    public interface IPlayerServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void getListOfDecks(List<DeckContract> decks);

        [OperationContract(IsOneWay = true)]
        void OpenWindow(bool access, string player);

        [OperationContract(IsOneWay = true)]
        void LoadListOfCards(List<CardContract> cardContractList);

        [OperationContract(IsOneWay = true)]
        void LoadListOfAllCards(List<CardContract> cardContractList);

        [OperationContract(IsOneWay = true)]
        void ConfirmDeckSaved(int numberEntries);

        [OperationContract(IsOneWay = true)]
        void LoadCardsToModify(List<CardContract> cardList);

        [OperationContract(IsOneWay = true)]
        void ConfirmDeckUpdated(int numerEntries);

        [OperationContract(IsOneWay = true)]
        void ReceiveInvitation(string invitationName, PlayerContract player);

        [OperationContract(IsOneWay = true)]
        void LoadFriendList(List<PlayerContract> playerList);

        [OperationContract(IsOneWay = true)]
        void receiveConfirmation(string playerConfirmation);

        [OperationContract(IsOneWay = true)]
        void LoadGame(PlayerContract playe1, PlayerContract player2);

        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string playerSender, string message);

        [OperationContract(IsOneWay = true)]
        void GetFriendId(PlayerContract player);

        [OperationContract(IsOneWay = true)]
        void GetMyId(Guid id);

        [OperationContract(IsOneWay = true)]
        void ReceiveCardsOnTable(byte[] imageOnboard);

        [OperationContract(IsOneWay = true)]
        void loadGameHistory(List<GameContract> gameHistory);

        [OperationContract(IsOneWay = true)]
        void getGameId(int id);

        [OperationContract(IsOneWay = true)]
        void ReceiveSelectCard(List<CardContract> cardList);

        [OperationContract(IsOneWay = true)]
        void ReceiveAttack(List<CardContract> cardsToAttack);

        [OperationContract(IsOneWay = true)]
        void ReceiveCardToAttack(List<CardContract> cardList);

        [OperationContract(IsOneWay = true)]
        void StartFirstTurn(bool startAttack);

        [OperationContract(IsOneWay = true)]
        void StartTurn(bool startAttack, string player1);

        [OperationContract(IsOneWay = true)]
        void ReceiveDefendTurn(bool changeTurn);

        [OperationContract(IsOneWay = true)]
        void ReceiveAttackTurn(bool changeTurn);

        [OperationContract(IsOneWay = true)]
        void StartExecuteAttack(bool attack);

        [OperationContract(IsOneWay = true)]
        void ReceiveChoiceCardTurn(bool turn);

        [OperationContract(IsOneWay = true)]
        void CleanCardsSurvival(List<CardContract> cardsSurvival);

        [OperationContract(IsOneWay = true)]
        void GetCardToNewTurn(CardContract card);

        [OperationContract(IsOneWay = true)]
        void ShowTotalDamage(int damage);

        [OperationContract(IsOneWay = true)]
        void ReceiveGameOver(bool isGameOver);

        [OperationContract(IsOneWay = true)]
        void ConfirmRegistration(int confirmPlayer);

        [OperationContract(IsOneWay = true)]
        void ConfirmLogin(PlayerContract player, bool confirmLogin);

        [OperationContract(IsOneWay = true)]
        void SendPlayer(PlayerContract player, bool isFound);

        [OperationContract(IsOneWay = true)]
        void SendRequest(bool confirmInvitation);

        [OperationContract(IsOneWay = true)]
        void SeeRequests(List<FriendContract> _friends);

        [OperationContract(IsOneWay = true)]
        void ConfirmRequestAnswered(List<FriendContract> friendContract);

        [OperationContract(IsOneWay = true)]
        void ChargeListOfFriends(List<PlayerContract> playerContracts);

        [OperationContract(IsOneWay = true)]
        void ShowProfileData(PlayerContract playerContract);

        [OperationContract(IsOneWay = true)]
        void ShowProfileImages(List<ProfieImageContract> profieImageContract);

        [OperationContract(IsOneWay = true)]
        void OpenMainWindow(PlayerContract player);
    }
}
