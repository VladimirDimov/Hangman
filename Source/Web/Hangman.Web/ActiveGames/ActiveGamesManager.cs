namespace Hangman.Web.ActiveGames
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;

    public class ActiveGamesManager
    {
        private static Dictionary<string, ActiveGameModel> activeGames = new Dictionary<string, ActiveGameModel>();
        private static object activeGamesLocker = new object();

        public Dictionary<string, ActiveGameModel> Games
        {
            get
            {
                return activeGames;
            }
        }

        public ActiveGameModel this[string id]
        {
            get
            {
                if (!activeGames.ContainsKey(id))
                {
                    return null;
                }

                return activeGames[id];
            }
        }

        /// <summary>
        /// Creates a game and returns game ID
        /// </summary>
        public string CreateGame(string word, string description, string userId, string username, bool isMultiplayer, string gameName)
        {
            var playerExistingGame = activeGames.FirstOrDefault(g => g.Value.Players.Any(p => p.Id == userId));

            // remove if player owns a game already
            if (playerExistingGame.Value != null)
            {
                lock (activeGamesLocker)
                {
                    if (playerExistingGame.Value.Owner.Id == userId)
                    {
                        // Delete the game if user owns it
                        activeGames.Remove(playerExistingGame.Key);
                    }
                    else
                    {
                        // leave the game if the user doesn't own it
                        playerExistingGame.Value.Players = playerExistingGame.Value.Players.Where(p => p.Id != userId).ToList();
                    }
                }
            }

            var gameId = Guid.NewGuid().ToString();

            var playerModel = new ActiveGamePlayerModel
            {
                Id = userId,
                NumberOfErrors = 0,
                OpenedPositions = this.GetInitialOpenedPositions(word),
                Name = username,
                NumberOfGuesses = 0
            };

            var players = new List<ActiveGamePlayerModel>()
            {
                playerModel
            };

            var activeGameModel = new ActiveGameModel
            {
                GameId = Guid.NewGuid().ToString(),
                Players = players,
                Owner = players.First(),
                IsMultiplayer = isMultiplayer,
                Word = word.ToLower(),
                Description = description,
                GameStatus = isMultiplayer ? GameStatus.WaitingForOpponent : GameStatus.Active,
                GameName = gameName
            };

            lock (activeGamesLocker)
            {
                activeGames.Add(gameId, activeGameModel);
            }

            return gameId;
        }

        public void RemoveGame(string gameId)
        {
            activeGames.Remove(gameId);
        }

        public ActiveGamePlayerModel MakeGuess(string gameId, string playerId, char guess, bool guessAll)
        {
            if (!activeGames.ContainsKey(gameId))
            {
                throw new ArgumentException("Invalid game id");
            }

            var game = activeGames[gameId];
            var player = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                throw new ArgumentException("User is not in the game");
            }

            var openedPositions = player.OpenedPositions.ToList();
            var positionsToOpen = new Queue<int>();
            for (int i = 0; i < game.Word.Length; i++)
            {
                if (openedPositions[i] == '\0' && game.Word[i] == char.ToLower(guess))
                {
                    positionsToOpen.Enqueue(i);
                }
            }

            if (positionsToOpen.Any())
            {
                player.NumberOfGuesses += positionsToOpen.Count;
            }
            else
            {
                player.NumberOfErrors += 1;
            }

            this.OpenPositions(player, game, positionsToOpen);
            this.UpdateGameStatus(player, game);
            return player;
        }

        public void JoinGame(string gameId, string playerId, string playerName)
        {
            var game = this[gameId];
            if (game.Players.Any(u => u.Id == playerId))
            {
                return;
            }

            game.Players.Add(new ActiveGamePlayerModel
            {
                Id = playerId,
                NumberOfErrors = 0,
                NumberOfGuesses = 0,
                OpenedPositions = this.GetInitialOpenedPositions(game.Word),
                Name = playerName
            });

            if (game.Players.Count > 1)
            {
                game.GameStatus = GameStatus.Active;
            }
        }

        internal void Leave(string gameId, string playerId)
        {
            var game = this[gameId];
            if (game == null)
            {
                return;
            }

            var playerToRemove = game.Players.FirstOrDefault(p => p.Id == playerId);
            if (playerId == null)
            {
                return;
            }

            game.Players.Remove(playerToRemove);

            if (game.Players.Count == 1)
            {
                game.GameStatus = GameStatus.WaitingForOpponent;
            }

            if (!game.Players.Any())
            {
                this.RemoveGame(gameId);
            }
        }

        internal void GuessAll(string userId, string gameId, string guess)
        {
            if (!activeGames.ContainsKey(gameId))
            {
                return;
            }

            var game = activeGames[gameId];
            if (game.Word.ToLower() == guess.ToLower())
            {
                game.WinnerId = userId;
                game.GameStatus = GameStatus.HasWinner;
            }
            else
            {
                var player = game.Players.FirstOrDefault(p => p.Id == userId);
                if (player == null)
                {
                    return;
                }

                player.NumberOfErrors = GlobalConstants.MaxNumberOfErrors;
            }
        }

        private void UpdateGameStatus(ActiveGamePlayerModel player, ActiveGameModel game)
        {
            if (!player.OpenedPositions.Contains('\0'))
            {
                game.GameStatus = GameStatus.HasWinner;
                game.WinnerId = player.Id;
            }
            else if (player.NumberOfErrors == GlobalConstants.MaxNumberOfErrors)
            {
                if (!game.IsMultiplayer)
                {
                    game.GameStatus = GameStatus.Over;
                }
                else if (game.Players.Count() == 2)
                {
                    var winner = game.Players.First(p => p.Id != player.Id);
                    game.WinnerId = winner.Id;
                    game.GameStatus = GameStatus.HasWinner;

                    // TODO: Signal other competitors
                }
            }
        }

        private IEnumerable<char> GetInitialOpenedPositions(string word)
        {
            var positions = new char[word.Length];
            positions[0] = word[0];
            positions[word.Length - 1] = word[word.Length - 1];
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == ' ')
                {
                    positions[i] = '*';
                    positions[i - 1] = word[i - 1];
                    positions[i + 1] = word[i + 1];
                }
            }

            return positions.ToList();
        }

        private void OpenPositions(ActiveGamePlayerModel player, ActiveGameModel game, Queue<int> positionsToOpen)
        {
            var currentOpenedPositions = player.OpenedPositions.ToList();
            var newOpenedPositions = new List<char>();
            for (int i = 0; i < currentOpenedPositions.Count; i++)
            {
                if (positionsToOpen.Any())
                {
                    if (i != positionsToOpen.Peek())
                    {
                        newOpenedPositions.Add(currentOpenedPositions[i]);
                    }
                    else
                    {
                        newOpenedPositions.Add(game.Word[positionsToOpen.Dequeue()]);
                    }
                }
                else
                {
                    newOpenedPositions.Add(currentOpenedPositions[i]);
                }
            }

            player.OpenedPositions = newOpenedPositions;
        }
    }
}
