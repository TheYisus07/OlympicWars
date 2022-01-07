using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContractsOW
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class PlayerService : IPlayerService
    {

        private OlympicWarsContext _context = new OlympicWarsContext();
        private List<PlayerContract> _playerList = new List<PlayerContract>();

        public void AceptInvitation(string playerAcepted, string playerHost)
        {
            PlayerContract player = _playerList.FirstOrDefault(p => p.nickName == playerHost);
            player.CallbackChannel.receiveConfirmation(playerAcepted);
        }

        public void AddCardInNewTurn(string deckName)
        {
            List<string> collectedCardsList = _context.CollectedCards.Where(cd => cd.nameDeck == deckName).Select(cd => cd.nameCard).ToList();
            List<CardContract> cardContractList = new List<CardContract>();
            for (int i = 0; i < collectedCardsList.Count(); i++)
            {
                string cardName = collectedCardsList[i];
                Card card = _context.Cards.Single(c => c.nameCard == cardName);
                cardContractList.Add(new CardContract
                {
                    nameCard = card.nameCard,
                    attribute = card.attribute,
                    damage = card.damage,
                    image = card.image,
                    attributeImage = card.attributeImage
                });
            }
            Random random = new Random();
            int randomIndex = random.Next(0, 2);
            CardContract cardRandom = cardContractList[randomIndex];
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().GetCardToNewTurn(cardRandom);
        }

        public void ChangeChoiceCardTurns(string player)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == player);
            if (playerContract1 != null)
            {
                playerContract1.CallbackChannel.ReceiveChoiceCardTurn(true);
            }
        }

        public void chargeDecks(string player)
        {
            
            List<Deck> listDeck = _context.Decks.Where(d => d.playerName == player).ToList();
            List<DeckContract> listDeckContract = new List<DeckContract>();
            for (int i = 0; i < listDeck.Count(); i++)
            {
                listDeckContract.Add(new DeckContract
                {
                    deckName = listDeck[i].deckName,
                    type = listDeck[i].type,
                    playerName = listDeck[i].playerName
                });
            }
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().getListOfDecks(listDeckContract);
        }

        public void ExecuteAttack(string player)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == player);
            if (playerContract1 != null)
            {
                playerContract1.CallbackChannel.StartExecuteAttack(true);
            }
        }

        public void GetAllCards()
        {


            List<Card> cardList = _context.Cards.ToList();
            List<CardContract> cardContractList = new List<CardContract>();
            for(int i = 0; i < cardList.Count(); i++)
            {
                cardContractList.Add(new CardContract
                {
                    nameCard = cardList[i].nameCard,
                    attribute = cardList[i].attribute,
                    damage = cardList[i].damage,
                    image = cardList[i].image,
                    attributeImage = cardList[i].attributeImage
                });
            }
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().LoadListOfAllCards(cardContractList);
        }

        public void GetCardsToModify(List<string> cardList)
        {
            List<Card> cards = _context.Cards.Where(c => !cardList.Contains(c.nameCard)).ToList();
            List<CardContract> cardContractList = new List<CardContract>();
            for (int i = 0; i < cards.Count(); i++)
            {
                cardContractList.Add(new CardContract
                {
                    nameCard = cards[i].nameCard,
                    attribute = cards[i].attribute,
                    damage = cards[i].damage,
                    image = cards[i].image,
                    attributeImage = cards[i].attributeImage
                });
            }
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().LoadCardsToModify(cardContractList);
        }

        public void GetFriendList(Guid guid, string playerName)
        {
            _playerList.Add(new PlayerContract
            {
                id = guid,
                nickName = playerName,
                CallbackChannel = OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>()
            });
            foreach(var player in _playerList)
            {
                if (_playerList != null)
                {
                    player.CallbackChannel.LoadFriendList(_playerList);
                    //Console.WriteLine(player.nickName);
                }
                
            }
        }

        public void getGameHistory(string playerName)
        {
            List<PlayerGame> playerGame = new List<PlayerGame>();
            List<Game> games = new List<Game>();
            List<GameContract> gameContract = new List<GameContract>();
            Player player = _context.Players.Single(p => p.nickName == playerName);
            if (player != null)
            {
                List<int> listId = _context.PlayerGames.Where(p => p.playerName == player.nickName).Select(p => p.idGame).ToList();
                 //= playerGame.Select(p => p.idGame).ToList();
                games = _context.Games.Where(g => listId.Contains(g.idGame)).ToList();
                playerGame = _context.PlayerGames.Where(p => listId.Contains(p.idGame)).ToList();
                for (int i = 0; i < games.Count(); i++)
                {
                    gameContract.Add(new GameContract
                    {
                        idGame = games[i].idGame,
                        duration = games[i].duration,
                        dateGame = games[i].dateGame,
                        stateGame = games[i].stateGame,
                        player1 = playerGame[i].playerName,
                        statePlayer1 = playerGame[i].state,
                        player2 = playerGame[i + 1].playerName,
                        statePlayer2 = playerGame[i + 1].state
                    });
                }
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().loadGameHistory(gameContract);
            }
        }

        public void LoadCards(string deckName)
        {
            List<string> collectedCardsList = _context.CollectedCards.Where(cd => cd.nameDeck == deckName).Select(cd => cd.nameCard).ToList();
            List<CardContract> cardContractList = new List<CardContract>();
            for (int i = 0; i < collectedCardsList.Count(); i++)
            {
                string cardName = collectedCardsList[i];
                Card card = _context.Cards.Single(c => c.nameCard == cardName);
                cardContractList.Add(new CardContract
                {
                    nameCard = card.nameCard,
                    attribute = card.attribute,
                    damage = card.damage,
                    image = card.image,
                    attributeImage =  card.attributeImage
                });
            }

            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().LoadListOfCards(cardContractList);
        }

        public void Logon(Guid guid, string player)
        {
            
            string playerName = _context.Players.Single(p => p.nickName == player).nickName;
            if (playerName.Equals(player))
            {
                
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().OpenWindow(true, playerName);
                Console.WriteLine($"{playerName} just connected");
            }
            else
            {
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().OpenWindow(false, player);
            }
        }

        public void PassTurnToAttack(string player)
        {
            PlayerContract playerToSend = _playerList.FirstOrDefault(p => p.nickName == player);
            if(playerToSend != null)
            {
                playerToSend.CallbackChannel.ReceiveAttackTurn(true);
            }
            
        }

        public void PassTurnToDefend(string player)
        {
            PlayerContract playerToSend = _playerList.FirstOrDefault(p => p.nickName == player);
            if (playerToSend != null)
            {
                playerToSend.CallbackChannel.ReceiveDefendTurn(true);
            }
        }

        public void SaveDeck(List<string> cardList, string deckName, string playerName)
        {
            _context.Decks.Add(new Deck
            {
                deckName = deckName, 
                type = "fuego", 
                playerName = playerName
            });
            for (int i = 0; i < cardList.Count(); i++)
            {
                _context.CollectedCards.Add(new CollectedCard
                {
                    nameDeck = deckName,
                    nameCard = cardList[i]
                });
                //Console.WriteLine(cardList[i]);
            }
            int numberEntries = _context.SaveChanges();
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmDeckSaved(numberEntries);
        }

        public void SaveGameData(GameContract game)
        {
            _context.Games.Add(new Game
            {
                playerName = game.playerName,
                duration = game.duration,
                dateGame = game.dateGame,
                stateGame = game.stateGame
            });
            _context.SaveChanges();
            int lastGameId = _context.Games.ToList().Count();
            if(lastGameId != 0)
            {
                _context.PlayerGames.Add(new PlayerGame
                {
                    idGame = lastGameId,
                    playerName = game.player1,
                    state = game.statePlayer1
                });
                _context.SaveChanges();
                _context.PlayerGames.Add(new PlayerGame
                {
                    idGame = lastGameId,
                    playerName = game.player2,
                    state = game.statePlayer2
                });
                _context.SaveChanges();
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().getGameId(lastGameId);
            }
            
        }

        public void SendAttack(string player, List<CardContract> cardsToAttack)
        {
            PlayerContract playerToSend = _playerList.FirstOrDefault(p => p.nickName == player);
            playerToSend.CallbackChannel.ReceiveAttack(cardsToAttack);
        }

        public void SendCardsOnTable(byte[] imageOnboard, string player)
        {
            PlayerContract playerToSend = _playerList.FirstOrDefault(p => p.nickName == player);
            playerToSend.CallbackChannel.ReceiveCardsOnTable(imageOnboard);
        }

        public void SendCardToAttack(string player, List<CardContract> cardList)
        {
            PlayerContract playerContract = _playerList.SingleOrDefault(p => p.nickName == player);
            playerContract.CallbackChannel.ReceiveCardToAttack(cardList);
        }

        public void SendGameOver(string playerName)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == playerName);
            if (playerContract1 != null)
            {
                playerContract1.CallbackChannel.ReceiveGameOver(true);
            }
        }

        public void SendInvitation(string nickName, PlayerContract friend)
        {
            var hostFriend = _playerList.FirstOrDefault(h => h.nickName == nickName);
            var friendExtreme = _playerList.FirstOrDefault(f => f.id == friend.id);
            if (friendExtreme != null)
            {
                friendExtreme.CallbackChannel.ReceiveInvitation(string.Format("{0}>{1}", nickName, "Has invited you to play"), hostFriend);
            }
        }

        public void sendMessageInGame(PlayerContract playerSender, PlayerContract playerReceiver, string message)
        {
            //var playerContractSender = _playerList.Single(p => p.id == playerSender.id);
            var playerContractRecevicer = _playerList.FirstOrDefault(p => p.nickName == playerReceiver.nickName);
            Console.WriteLine(playerReceiver.id);
            Console.WriteLine(playerContractRecevicer.id);
            if (playerContractRecevicer != null)
            {
                Console.WriteLine(playerContractRecevicer.CallbackChannel.ToString());
                playerContractRecevicer.CallbackChannel.ReceiveMessage(playerSender.nickName, message);
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ReceiveMessage(playerSender.nickName, message);
            }
            else
            {
                Console.WriteLine("was null");
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ReceiveMessage("was null", "was null");
            }
        }

        public void SendPlayerToStartTurns(string player1)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == player1);
            if (playerContract1 != null)
            {
                playerContract1.CallbackChannel.StartTurn(true, playerContract1.nickName);
            }
        }

        public void SendSelectCard(List<CardContract> cardList, string playerName)
        {
            PlayerContract player = _playerList.SingleOrDefault(p => p.nickName == playerName);
            player.CallbackChannel.ReceiveSelectCard(cardList);
        }

        public void SendSurvivalCards(string player, List<CardContract> cardListAttack)
        {
            PlayerContract playerContract = _playerList.SingleOrDefault(p => p.nickName == player);
            if (playerContract != null)
            {
                playerContract.CallbackChannel.CleanCardsSurvival(cardListAttack);
            }
        }

        public void SendTotalDamageDealt(string playerName, int damage)
        {
            PlayerContract playerContract = _playerList.SingleOrDefault(p => p.nickName == playerName);
            if (playerContract != null)
            {
                playerContract.CallbackChannel.ShowTotalDamage(damage);
            }
        }

        public void StarGame(string player1, string player2)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == player1);
            
            PlayerContract playerContract2 = _playerList.FirstOrDefault(p => p.nickName == player2);
            if ((playerContract1 != null) && (playerContract2 != null))
            {
                playerContract1.CallbackChannel.LoadGame(playerContract1, playerContract2);
                

                playerContract2.CallbackChannel.LoadGame(playerContract2, playerContract1);
                
            }
        }

        public void startTimer()
        {
        }

        public void StartTurns(string player1)
        {
            PlayerContract playerContract1 = _playerList.FirstOrDefault(p => p.nickName == player1);
            if (playerContract1 != null)
            {
                playerContract1.CallbackChannel.StartFirstTurn(true);
            }
        }

        public void UpdateDeck(List<CardContract> cardList, string deckName)
        {
            List<CollectedCard> collectedCardList = _context.CollectedCards.Where(cc => cc.nameDeck == deckName).ToList();
            for (int i = 0; i < collectedCardList.Count(); i++)
            {
                _context.CollectedCards.Remove(collectedCardList[i]);
            }
            for (int i = 0; i < cardList.Count(); i++)
            {
                _context.CollectedCards.Add(new CollectedCard
                {
                    nameDeck = deckName,
                    nameCard = cardList[i].nameCard
                });
            }
            int numberEntries = _context.SaveChanges();
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmDeckUpdated(numberEntries);
        }

        public void UpdateGameData(GameContract game)
        {
            int idGame = game.idGame;
            string durationGame = game.duration;
            string stateGameString = game.stateGame;
            _context.Database.ExecuteSqlCommand("UPDATE [dbo].[Game] SET [duration] = @durationParameter, [stateGame] = @stateParameter WHERE [idGame] = @idGameParameter", 
                new SqlParameter("@durationParameter", durationGame), 
                new SqlParameter("@stateParameter", stateGameString), 
                new SqlParameter("@idGameParameter", idGame));

        }

        public void UpdateGuid(Guid id, PlayerContract player, string nameToSend)
        {
            PlayerContract  playerContract = _playerList.FirstOrDefault(p => p.id == id);

            //PlayerContract playerContract = _playerList.Single(p => p.nickName == player.);
            _playerList.Remove(playerContract);
            //playerContract.id = id;
            player.CallbackChannel = OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>();
            _playerList.Add(player);
            Guid playerId = _playerList.Single(p => p.id == player.id).id;
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().GetMyId(playerId);
        }

        public void UpdateIdFriend(string name)
        {
            PlayerContract playerToSend = _playerList.Single(p => p.nickName == name);
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().GetFriendId(playerToSend);
        }







        //-------------------------------------------------------------




        public void RegisterUser(Player player)
        {
            try
            {
                _context.Players.Add(player);
                int numberChange = _context.SaveChanges();
                OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmRegistration(numberChange);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {

                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
        }

        public void LoginUser(Player player)
        {
            Player playerLogin = _context.Players.Single(u => u.nickName == player.nickName);
            PlayerContract playerContract = new PlayerContract();
            playerContract.nickName = playerLogin.nickName;
            playerContract.password = playerLogin.password;
            playerContract.email = playerLogin.Email;
            playerContract.imageProfile = playerLogin.imageProfile;
            playerContract.state = playerLogin.state;
            
            bool confirmLogin = false;
            Console.WriteLine(playerLogin.nickName);
            if (playerLogin.nickName == player.nickName && playerLogin.password == player.password)
            {
                confirmLogin = true;

            }

            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmLogin(playerContract, confirmLogin);
        }

        public void GetPlayers(PlayerContract playerLogin)
        {
            playerLogin.CallbackChannel = OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>();
            _playerList.Add(playerLogin);

            foreach (var player in _playerList)
            {
                if (_playerList != null)
                {
                    player.CallbackChannel.LoadFriendList(_playerList);
                }
            }
        }

        public void SearchPlayer(string playerName)
        {
            Player playerSearch = new Player();
            PlayerContract playerContract = new PlayerContract();
            bool isFound;
            try
            {
                playerSearch = _context.Players.Single(u => u.nickName == playerName);
                playerContract.nickName = playerSearch.nickName;
                playerContract.password = playerSearch.password;
                playerContract.email = playerSearch.Email;
                playerContract.imageProfile = playerSearch.imageProfile;
                playerContract.state = playerSearch.state;
                isFound = true;
            }
            catch
            {
                isFound = false;
            }
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().SendPlayer(playerContract, isFound);

        }

        public void SendInvitation(string playerSend, string playerReceive)
        {
            List<FriendRequest> frindRequestsList = _context.FriendRequests.Where(p => p.nicknameFriend == playerReceive).ToList();
            bool canSend = false;
            if(frindRequestsList.Count() == 0)
            {
                canSend = true;
            }
            for (int i = 0; i < frindRequestsList.Count(); i++)
            {
                if (frindRequestsList[i].nicknamePlayer != playerSend)
                {
                    canSend = true;
                }
            }

            if (canSend)
            {
                SendRequestInvitation(playerSend, playerReceive);
            }
        }

        private void SendRequestInvitation(string playerSend, string playerReceive)
        {
            bool confirmInvitation = true;
            string stateRequest = "sent";
            _context.FriendRequests.Add(new FriendRequest
            {
                nicknamePlayer = playerSend,
                nicknameFriend = playerReceive,
                stateRequest = stateRequest
            });
            _context.SaveChanges();
            PlayerContract playerContract = _playerList.FirstOrDefault(p => p.nickName == playerReceive);
            if (playerContract != null)
            {
                playerContract.CallbackChannel.SendRequest(confirmInvitation);

            }
        }

        public void SearchOfRequests(string playerName)
        {
            Console.WriteLine(playerName);
            List<FriendRequest> friendRequest = _context.FriendRequests.Where(p => p.nicknameFriend == playerName).ToList();
            List<FriendContract> friendContract = new List<FriendContract>();

            for (int i = 0; i < friendRequest.Count(); i++)
            {

                friendContract.Add(new FriendContract
                {
                    idFriend = friendRequest[i].idFriendRequest,
                    nicknameFriend = friendRequest[i].nicknameFriend,
                    nicknamePlayer = friendRequest[i].nicknamePlayer,
                    stateRequest = friendRequest[i].stateRequest
                });
            }
            Console.WriteLine(friendContract.Count());
            Console.WriteLine(friendRequest.Count());
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().SeeRequests(friendContract);
        }

        public void SendInvitationGame(string playerName, string frienName)
        {
            var hostFriend = _playerList.FirstOrDefault(h => h.nickName == playerName);
            var friendExtreme = _playerList.FirstOrDefault(f => f.nickName == frienName);
            if (friendExtreme != null)
            {
                friendExtreme.CallbackChannel.ReceiveInvitation(playerName, hostFriend);
            }
        }

        public void AcceptFriendRequest(string player, string friend)
        {
            bool isAccepted = true;
            _context.FriendRequests.Where(p => p.nicknameFriend == player).ToList();
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmRequestAnswered(isAccepted);
        }

        public void DenyFriendRequest(string player, string friend)
        {
            bool isAccepted = false;
            OperationContext.Current.GetCallbackChannel<IPlayerServiceCallback>().ConfirmRequestAnswered(isAccepted);
        }
    }
}
