// Jonathan Hermin, Emil Westling, & Jesper Carlsson
// 2023-10-25
// Microsoft Visual Studio Community 2022 (64 bit) - 17.7.0
// github: https://github.com/UU-projekt/nim2

using System.Runtime.CompilerServices;

namespace gruppptojekt
{
    internal class Program
    {

        static string logo = @"          
 /$$   /$$ /$$$$$$ /$$      /$$
| $$$ | $$|_  $$_/| $$$    /$$$
| $$$$| $$  | $$  | $$$$  /$$$$
| $$ $$ $$  | $$  | $$ $$/$$ $$
| $$  $$$$  | $$  | $$  $$$| $$
| $$\  $$$  | $$  | $$\  $ | $$
| $$ \  $$ /$$$$$$| $$ \/  | $$
|__/  \__/|______/|__/     |__/
";

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
            spelregler("spelare 1", "spelare 2");

            bool run = true;
            while(run)
            {
                Console.Clear();
                int selection = MainMenu();

                switch (selection)
                {
                    case 1:
                        StartGame();
                        break;
                    case 2:
                        ShowScoreBoard();
                        break;
                    case 3:
                        Console.WriteLine("Tack för att du spelade nim! :D");
                        run = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Denna funktion startar själva spelet. Spelare(n) anger namn och om AI skall vara med
        /// </summary>
        static void StartGame()
        {
            Console.Clear();
            string p1 = Ask("Namn spelare 1").Trim();
            string p2 = "Weird AI";
            bool aiEnabled = true;


            if (Ask("Vill du spela mot AI (y/n)", new string[] { "y", "n" }) == "n")
            {
                p2 = Ask("Namn spelare 2").Trim();
                aiEnabled = false;
            }

            spelregler(p1, p2);

            spel(p2, p1, aiEnabled);
        }

        /// <summary>
        /// Denna funktion skriver ut huvudmenyn till konsolen och skickar tillbaka användarens val
        /// </summary>
        /// <returns></returns>
        static int MainMenu()
        {
            ColourLog("Jonathan Hermin, Emil Westling, & Jesper Carlsson presenterar...", ConsoleColor.DarkGray);
            ColourLog(logo, ConsoleColor.DarkCyan);

            ColourLog("#1: Spela!", ConsoleColor.Green);
            ColourLog("#2: Visa statestik!", ConsoleColor.Yellow);
            ColourLog("#3: Avsluta spelet :(\n", ConsoleColor.Red);

            Console.Write("val: ");
            int.TryParse(Console.ReadLine(), out int nr);

            return nr;
        }

        /// <summary>
        /// Denna funktion skriver ut scoreboardet till konsolen
        /// </summary>
        static void ShowScoreBoard()
        {
            Console.Clear();
            ScoreBoard score = new ScoreBoard();
            var matches = score.GetMatches();
            var playerScores = score.GetPlayerScores();

            ColourLog("Vinststatestik", ConsoleColor.Green);
            foreach(var player in playerScores)
            {
                Console.WriteLine($"{player.Key}: {player.Value}");
            }

            ColourLog("\nMatch-repriser:", ConsoleColor.Green);
            for (int i = 0; i <  matches.Count; i++)
            {
                Match iMatch = matches[i];

                // Gör varranan rad grå för att göra det lättare att tyda listan om den är lång
                if (i % 2 == 0) Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.Write($"({i}) vinnare: {iMatch.winner}");
                Console.SetCursorPosition(Console.BufferWidth - iMatch.date.ToString().Length, Console.CursorTop);
                Console.WriteLine(iMatch.date);
                Console.ForegroundColor = ConsoleColor.White;
            }

            string matchSelection = Ask("Välj match att visa (x för att avsluta)");
            bool couldParse = int.TryParse(matchSelection, out int match);

            if (match < 0 || match >= matches.Count || !couldParse)
            {
                Console.Clear();
                return;
            }

            Match SelectedMatch = matches[match];
            SelectedMatch.ShowReplay();
        }

        /// <summary>
        /// Denna funktion skriver ut spelets regler och vilken spelare som börjar
        /// </summary>
        /// <param name="firstPlayer"></param>
        /// <param name="secondPlayer"></param>
        static void spelregler(string firstPlayer, string secondPlayer)
        {
            Console.Clear();
            ColourLog("Jonathan Hermin, Emil Westling, & Jesper Carlsson presenterar...", ConsoleColor.DarkGray);
            ColourLog(logo, ConsoleColor.DarkCyan);
            Console.WriteLine($"{firstPlayer} möter {secondPlayer}\n");
            Console.WriteLine("Spelregler:");
            Console.WriteLine($"1. Ni presenteras med 3 högar med 5 stickor i vardera\n2. {firstPlayer} börjar och väljer vilken hög hen vill ta stickor från från och därefter hur många stickor som {firstPlayer} vill ta från högen. Därefter är det {secondPlayer}s tur.\n3. den som tar den sista stickan från den sista högen har vunnit!\n\nNi väljer av hög genom att ange högen och sedan antalet avskiljt med mellanslag.\nExempel: \"3 5\" vilket tar 5 stickor (alla) från hög 3");
            Console.WriteLine("\ntryck på valfri knapp för att börja spela...");
            Console.ReadKey();
        }

        /// <summary>
        /// Visar den skärm som dyker upp när en spelare vinner
        /// </summary>
        /// <param name="winningPlayer">den vinnande spelaren</param>
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
            Console.WriteLine($"{winningPlayer} vann!\nTack för att ni spelade!");
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Denna funktion kollar om det är slut på pinnar vilket indikerar att någon vann
        /// </summary>
        /// <param name="stacks"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Denna funktion tar spelarens input (om spelaren är människa)
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="PlayerSelection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Denna funktion skriver ut en hög i konsolen
        /// </summary>
        /// <param name="stack">hur många pinnar det finns i denna hög</param>
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

        /// <summary>
        /// Generarar ett random drag åt Weird AI
        /// </summary>
        /// <param name="stacks"></param>
        /// <returns></returns>
        static SelectionStruct GetAiMove(int[] stacks)
        {
            Random random = new Random();

            int stack = random.Next(1, 4);
            if (stacks[stack - 1] != 0)
            {
                int amount = random.Next(1, stacks[stack - 1]);

                return new SelectionStruct { stack = stack, amount = amount };
            }
            else return GetAiMove(stacks);
        }

        /// <summary>
        /// Funktionen där det roliga händer! Detta är själva spelet
        /// </summary>
        /// <param name="p1">spelare 1</param>
        /// <param name="p2">spelare 2</param>
        /// <param name="ai">om en AI är med</param>
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

                bool couldParse = true; //GetPlayerInput(players[playerTurn ? 1 : 0], out SelectionStruct selection)
                SelectionStruct selection;

                if (!ai || playerTurn)
                {
                    couldParse = GetPlayerInput(players[playerTurn ? 1 : 0], out SelectionStruct sel);
                    selection = sel;
                } else
                {
                    Console.Write($"{players[playerTurn ? 1 : 0]}: ");
                    Thread.Sleep(700);
                    selection = GetAiMove(pinnar);
                    Console.Write($"{selection.stack} {selection.amount}");
                    Thread.Sleep(770);
                }

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