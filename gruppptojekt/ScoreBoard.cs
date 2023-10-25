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
            RoundSteps.Add(new RoundStep {  player = player, move = move });
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

                outputFile.WriteLine($"ENDMATCH, {winner.Replace(',', ' ')}, {DateTime.Now}");
            }
        }

        /// <summary>
        /// Laddar alla tidigare matcher
        /// </summary>
        /// <returns></returns>
        public List<Match> GetMatches()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            List<Match> matches = new List<Match>();

            using (StreamReader file = new StreamReader(Path.Combine(docPath, "NimScore.csv")))
            {
                string? line;
                List<RoundStep> MatchSteps = new List<RoundStep>();
                while ((line = file.ReadLine()) != null)
                {
                    string[] entries = line.Split(',');

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

            return matches;
        }
    }
}