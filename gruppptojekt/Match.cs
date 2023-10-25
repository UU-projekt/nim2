// Jonathan Hermin, Emil Westling, & Jesper Carlsson
// 2023-10-25
// Microsoft Visual Studio Community 2022 (64 bit) - 17.7.0
// github: https://github.com/UU-projekt/nim2
using static gruppptojekt.ScoreBoard;

namespace gruppptojekt
{
    /// <summary>
    /// Denna klass representerar en match i datafilen. Matchen innehåller vinnaren, när den spelades, och en lista med alla steg i matchen.
    /// Det går att kalla en funktion som spelar upp matchen
    /// </summary>
    internal class Match
    {
        public string winner;
        public DateTime date;
        public List<ScoreBoard.RoundStep> rounds;
        private int cursor = 0;

        public Match(List<RoundStep> MatchSteps, string gameWinner, DateTime time)
        {
            this.rounds = new List<RoundStep>(MatchSteps);
            this.winner = gameWinner;
            this.date = time;
        }

        /// <summary>
        /// Intern funktion för denna klass som för varje kallelse hoppar fram ett steg i listan tills det att listan är slut då funktionen returnerar false
        /// </summary>
        /// <param name="step">detta steg i matchen</param>
        /// <returns></returns>
        bool NextStep(out ScoreBoard.RoundStep? step)
        {
            if (rounds.Count <= cursor)
            {
                step = null;
                return false;
            } else
            {
                step = rounds.ElementAt(cursor);
                cursor++;
                return true;
            }
        }

        /// <summary>
        /// Visar "replayen" av en match
        /// </summary>
        public void ShowReplay()
        {
            int[] pinnar = { 5, 5, 5 };

            while(this.NextStep(out RoundStep? step)) {
                if (!step.HasValue) break;
                Console.Clear();

                var move = step.Value.move;
                string player = step.Value.player;

                if(move.stack > pinnar.Length || 1 > move.stack)
                {
                    Console.WriteLine($"!!! NÅGOT GICK FEL !!!\nFörsökte spela med ogiltig hög");
                    break;
                }

                pinnar[move.stack - 1] -= move.amount;

                for (int i = 0; i < pinnar.Length; i++)
                {
                    Program.DrawStack(pinnar[i]);
                }

                Console.SetCursorPosition(6, move.stack - 1);

                bool plural = move.amount > 1;

                Console.WriteLine($"<- {player} tar {move.amount} stick{ (plural ? "or" : "a") }");

                Thread.Sleep(1000);
            }
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"{winner} vann denna match! (tryck på valfri tangent för att avbryta)");
            Console.ReadKey();
        }
    }
}