// Jonathan Hermin & Emil Westling
// 2023-10-25
// Microsoft Visual Studio Community 2022 (64 bit) - 17.7.0
// github: https://github.com/UU-projekt/nim2

namespace gruppptojekt
{
    internal class Program
    {
        /// <summary>
        /// Skriver ut en textrad i färg
        /// </summary>
        /// <param name="text">texten du vill skriva</param>
        /// <param name="colour">färgen du vill använda</param>
        static void ColourLog(string text, ConsoleColor colour)
        {
            ConsoleColor prev = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ForegroundColor = prev;
        }

        /// <summary>
        /// Denna funktion frågar en fråga och tar input på samma rad. Funktionen kommer fortsätta att köras tills det att användaren har anget ett svar som inte är null
        /// </summary>
        /// <param name="question">prompten som ställs till användaren</param>
        /// <returns>string</returns>
        static string Ask(string question)
        {
            Console.Write($"{question}: ");
            string? ans = Console.ReadLine();
            return String.IsNullOrWhiteSpace(ans) ? Ask(question) : ans;
        }

        /// <summary>
        /// Denna funktion frågar en fråga och tar input på samma rad. Funktionen kommer fortsätta att köras tills det att användaren har anget ett svar som inte är null och som finns i arrayen "validAnswers"
        /// </summary>
        /// <param name="question">prompten som ställs till användaren</param>
        /// <param name="validAnswers">en lista med godtagbara svar på prompten</param>
        /// <returns>string</returns>
        static string Ask(string question, string[] validAnswers)
        {
            Console.Write($"{question}: ");
            string? ans = Console.ReadLine();

            if(ans == null || !validAnswers.Contains(ans)) return Ask(question, validAnswers);

            return ans;
        }


        static void Main(string[] args)
        {
            ShowScoreBoard();

            string p1 = Ask("Namn spelare 1").Trim();
            string p2 = "Weird AI";
            bool aiEnabled = true;


            if(Ask("Vill du spela mot AI (y/n)", new string[] { "y", "n" }) == "n")
            {
                p2 = Ask("Namn spelare 2").Trim();
                aiEnabled = false;
            }

            spelregler(p1, p2);

            spel(p1, p2, aiEnabled);
        }

        static void ShowScoreBoard()
        {
            ScoreBoard score = new ScoreBoard();
            var matches = score.GetMatches();

            for(int i = 0; i <  matches.Count; i++)
            {
                Match iMatch = matches[i];
                Console.WriteLine($"({i}) vinnare: {iMatch.winner} | {iMatch.date}");
            }

            string matchSelection = Ask("Välj match att visa");
            bool couldParse = int.TryParse(matchSelection, out int match);

            if (match < 0 || match > matches.Count || !couldParse) return;

            Match SelectedMatch = matches[match];
            SelectedMatch.ShowReplay();
        }

        static void spelregler(string firstPlayer, string secondPlayer)
        {
            Console.Clear();
            string logo = @"          
 /$$   /$$ /$$$$$$ /$$      /$$
| $$$ | $$|_  $$_/| $$$    /$$$
| $$$$| $$  | $$  | $$$$  /$$$$
| $$ $$ $$  | $$  | $$ $$/$$ $$
| $$  $$$$  | $$  | $$  $$$| $$
| $$\  $$$  | $$  | $$\  $ | $$
| $$ \  $$ /$$$$$$| $$ \/  | $$
|__/  \__/|______/|__/     |__/
";
            ColourLog("Jonathan Hermin & Emil Westling presenterar...", ConsoleColor.DarkGray);
            ColourLog(logo, ConsoleColor.DarkCyan);
            Console.WriteLine($"{firstPlayer} möter {secondPlayer}\n");
            Console.WriteLine("Spelregler:");
            Console.WriteLine($"1. Ni presenteras med 3 högar med 5 stickor i vardera\n2. {firstPlayer} börjar och väljer vilken hög hen vill ta stickor från från och därefter hur många stickor som {firstPlayer} vill ta från högen. Därefter är det {secondPlayer}s tur.\n3. den som tar den sista stickan från den sista högen har vunnit!\n\nNi väljer av hög genom att ange högen och sedan antalet avskiljt med mellanslag.\nExempel: \"3 5\" vilket tar 5 stickor (alla) från hög 3");
            Console.WriteLine("\ntryck på valfri knapp för att börja spela...");
            Console.ReadKey();
        }

        static void winScreen(string winningPlayer)
        {
            Console.Clear();
            Console.SetCursorPosition(Console.BufferWidth / 2, 0);
            string crown = @"
  _.+._
(^\/^\/^)
 \@*@*@/
 {_____}";
            ColourLog(crown, ConsoleColor.Yellow);
            Console.WriteLine($"{winningPlayer} vann!");
        }

        static bool GameWon(int[] stacks)
        {
            bool containsNonZeroValue(int[] arr)
            {
                foreach (int stack in stacks)
                {
                    if (stack > 0) return true;
                }
                return false;
            }

            return !containsNonZeroValue(stacks);
        }

        public struct SelectionStruct
        {
            public int stack;
            public int amount;
        }
        static bool GetPlayerInput(string playerName, out SelectionStruct PlayerSelection)
        {
            string s = Ask(playerName);
            var arr = s.Split(' ');
            if (arr.Length == 2 && int.TryParse(arr[0], out int stack) && int.TryParse(arr[1], out int amount) )
            {
                PlayerSelection = new SelectionStruct() { stack = stack, amount = amount };
                return true;
            } else
            {
                PlayerSelection = new SelectionStruct() { stack = -1, amount = -1 };
                return false;
            }
        }

        public static void DrawStack(int stack)
        {
            switch (stack)
            {
                case 5:
                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            for (int j = 0; j < stack; j++)
            {
                Console.Write("|");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int j = 0; j < 5 - stack; j++)
            {
                Console.Write("|");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        static void spel(string p1, string p2, bool ai)
        {
            int[] pinnar = { 5, 5, 5 };
            string[] players = { p1, p2 };
            bool playerTurn = true;
            ScoreBoard score = new ScoreBoard();


            while (!GameWon(pinnar))
            {
                Console.Clear();

                for (int i = 0; i < pinnar.Length; i++) DrawStack(pinnar[i]);

                bool couldParse = GetPlayerInput(players[playerTurn ? 1 : 0], out SelectionStruct selection);
                if (!couldParse || selection.stack > pinnar.Length || selection.stack < 1 || selection.amount > pinnar[selection.stack - 1] || selection.amount < 1) continue;
                
                score.recordMove(players[playerTurn ? 1 : 0], selection);
                
                pinnar[selection.stack - 1] -= selection.amount;
                playerTurn = !playerTurn;
            }

            score.saveMatch(players[playerTurn ? 0 : 1]);
            winScreen(players[playerTurn ? 0 : 1]);
        }
    }
}