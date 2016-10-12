namespace Hangman.Web.ActiveGames
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// Creates a game and returns it's id
        /// </summary>
        public string CreateGame(string word, string userId, string username, bool isMultiplayer)
        {
            var playerExistingGame = activeGames.FirstOrDefault(g => g.Value.FirstPlayer.Id == userId);

            // remove if player owns a game already
            if (playerExistingGame.Value != null)
            {
                lock (activeGamesLocker)
                {
                    activeGames.Remove(playerExistingGame.Key);
                }
            }

            var gameId = Guid.NewGuid().ToString();

            lock (activeGamesLocker)
            {
                activeGames.Add(
                gameId,
                new ActiveGameModel
                {
                    FirstPlayer = new ActiveGamePlayerModel
                    {
                        Id = userId,
                        NumberOfErrors = 0,
                        OpenedPositions = GetInitialOpenedPositions(word)
                    },
                    IsMultiplayer = isMultiplayer,
                    Word = word
                });
            }

            return gameId;
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
    }
}
