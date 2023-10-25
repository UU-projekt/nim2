// Jonathan Hermin & Emil Westling
// 2023-10-25
// Microsoft Visual Studio Community 2022 (64 bit) - 17.7.0
// github: https://github.com/UU-projekt/nim2

using System.Runtime.CompilerServices;

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
            string p1 = Ask("Namn spelare 1");
            string p2 = "Weird AI";
            bool aiEnabled = true;


            if(Ask("Vill du spela mot AI (y/n)", new string[] { "y", "n" }) == "n")
            {
                p2 = Ask("Namn spelare 2");
                aiEnabled = false;
            }

            spelregler(p1, p2);

            spel(p1, p2, aiEnabled);
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

        static bool GameWon(int[] stacks)
        {
            foreach(int stack in stacks)
            {
                if (stack > 0) return true;
            }
            return false;
        }

        static void spel(string p1, string p2, bool ai)
        {
            int[] pinnar = { 5, 5, 5 };

            while (!GameWon(pinnar))
            {

            }
            
            int k = 0;

            for (int i = 0; i < pinnar.Length; i++)
            {
                for (int j = 0; j <= pinnar[i]; j++)
                {
                        if (k < pinnar[i])
                        {
                            Console.Write("|");
                            k++;
                        }
                        else if (k == pinnar[i])
                        {
                            k = 0;
                            Console.WriteLine("");
                        }  
                }
            }
        }
    }
}