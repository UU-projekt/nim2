// Jonathan Hermin & Emil Westling
// 2023-10-25
// Microsoft Visual Studio Community 2022 (64 bit) - 17.7.0
// github: https://github.com/UU-projekt/nim2

namespace gruppptojekt
{
    /// <summary>
    /// Klass som tillhandahåller sparandet av matcher
    /// </summary>
    internal class ScoreBoard
    {
        public struct RoundStep
        {
            public string player;
            public Program.SelectionStruct move;
        }
        List<RoundStep> RoundSteps = new List<RoundStep>();

        /// <summary>
        /// Sparar ett drag i en match till klassens interna lista
        /// </summary>
        /// <param name="player"></param>
        /// <param name="move"></param>
        public void recordMove(string player, Program.SelectionStruct move)
        {
            RoundSteps.Add(new RoundStep { player = player, move = move });
        }

        /// <summary>
        /// Sparar matchen till csv filen
        /// </summary>
        /// <param name="winner"></param>
        public void saveMatch(string winner)
        {
            if (RoundSteps.Count == 0) return;

            // https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "NimScore.csv"), true))
            {
                for (int i = 0; i < RoundSteps.Count; i++)
                {
                    RoundStep step = RoundSteps[i];
                    outputFile.WriteLine($"{step.player},{step.move.stack},{step.move.amount}");
                }

                outputFile.WriteLine($"ENDMATCH,{winner.Replace(',', ' ')},{DateTime.Now}");
            }
        }

        /// <summary>
        /// Skickar tillbaka en Dict som är keyad på spelarnamn och inkluderar spelarens antal vinster
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetPlayerScores()
        {
            Dictionary<string, int> players = new Dictionary<string, int>();

            foreach (var match in GetMatches())
            {
                if (!players.ContainsKey(match.winner)) players.Add(match.winner, 0);
                players[match.winner]++;
            }

            return players;
        }

        List<Match>? MatchCache;

        /// <summary>
        /// Laddar alla tidigare matcher
        /// </summary>
        /// <returns></returns>
        public List<Match> GetMatches()
        {
            if (MatchCache != null) return MatchCache;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            List<Match> matches = new List<Match>();

            // om filen inte finns skickar vi tillbaka en tom lista
            if(!File.Exists(Path.Combine(docPath, "NimScore.csv"))) return matches;

            using (StreamReader file = new StreamReader(Path.Combine(docPath, "NimScore.csv")))
            {
                string? line;
                List<RoundStep> MatchSteps = new List<RoundStep>();
                while ((line = file.ReadLine()) != null)
                {
                    string[] entries = line.Split(',');

                    if (entries.Length != 3)
                    {
                        Console.WriteLine("!!! KUNDE INTE TYDA NimScore.csv !!!\nFilen är felformaterad. Det går inte att garantera att repriserna kan spelas upp felfritt\n");
                        break;
                    };

                    // Eftersom vår kod bara sparar giltiga drag i "RoundSteps" kan vi strunta i att 
                    // validera den input vi får från NimScore.csv.
                    // Detta kan leda till att vår kod crashar om användaren själv går in och meckar i filen men i så fall är det användarens fel
                    if (entries[0] == "ENDMATCH")
                    {
                        matches.Add(new Match(MatchSteps, entries[1], DateTime.Parse(entries[2])));
                        MatchSteps.Clear();
                    } else
                    {
                        int.TryParse(entries[1], out int stack);
                        int.TryParse(entries[2], out int amount);

                        MatchSteps.Add(new RoundStep { player = entries[0], move = new Program.SelectionStruct { stack = stack, amount = amount } });
                    }
                }
            }

            MatchCache = matches;

            return matches;
        }
    }
}