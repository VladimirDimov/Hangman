namespace Hangman.Web.ActiveGames
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using ViewModels.Games;

    public class ActiveGamesManager
    {
        private static Dictionary<string, ActiveGameModel> activeGames = new Dictionary<string, ActiveGameModel>();
        private static object activeGamesLocker = new object();

        public ActiveGameModel this[string id]
        {
            get
            {
                return activeGames[id];
            }
        }

        /// <summary>
        /// Creates a game and returns game ID
        /// </summary>
        public string CreateGame(string word, string userId, string username, bool isMultiplayer)
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
                        playerExistingGame.Value.Players = playerExistingGame.Value.Players.Where(p => p.Id != userId);
                    }
                }
            }

            var gameId = Guid.NewGuid().ToString();

            var playerModel = new ActiveGamePlayerModel
            {
                Id = userId,
                NumberOfErrors = 0,
                OpenedPositions = GetInitialOpenedPositions(word)
            };

            var players = new List<ActiveGamePlayerModel>()
            {
                playerModel
            };

            lock (activeGamesLocker)
            {
                activeGames.Add(
                gameId,
                new ActiveGameModel
                {
                    Players = players,
                    Owner = players.First(),
                    IsMultiplayer = isMultiplayer,
                    Word = word.ToLower(),
                    GameStatus = isMultiplayer ? GameStatus.WaitingForOpponent : GameStatus.Active
                });
            }

            return gameId;
        }

        public void RemoveGame(string gameId)
        {
            activeGames.Remove(gameId);
        }

        public ActiveGamePlayerModel MakeGuess(string gameId, string playerId, IEnumerable<MakeGuessRequestViewModel> guesses, bool guessAll)
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

            player.NumberOfGuesses++;

            var positionsToOpen = new Queue<int>();
            foreach (var guess in guesses)
            {
                var guessLetter = char.ToLower(guess.Letter);
                if (game.Word[guess.Index] == guessLetter)
                {
                    positionsToOpen.Enqueue(guess.Index);
                }
                else
                {
                    if (guessAll)
                    {
                        // Set player to loose game
                    }
                    else
                    {
                        player.NumberOfErrors++;

                        return player;
                    }
                }
            }

            this.OpenPositions(player, game, positionsToOpen);
            this.UpdateGameStatus(player, game);
            return player;
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
                    game.GameStatus = GameStatus.Closed;
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

        private static IEnumerable<char> GetInitialOpenedPositions(string word)
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
