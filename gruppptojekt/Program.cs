using System.Runtime.CompilerServices;

namespace gruppptojekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Namn spelare 1: ");
            string s1 = Console.ReadLine();
            Console.Write("Namn spelare 2: ");
            string s2 = Console.ReadLine();
            Console.WriteLine(s1 + " kommer möta " + s2);

            spelregler(s1);

            Console.ReadLine();
            Console.WriteLine(s1 + " börjar");

            spel();
        }

        static void spelregler(string s1)
        {
            Console.WriteLine();
            Console.WriteLine("Spelregler:");
            Console.WriteLine("Ni kommer presenteras med 3 högar. Ni kommer välja en av högarna genom klicka en siffra mellan 1 och 3, ni får bara välja en hög i taget. Sedan kommer ni få välja hur många stickor ni vill ta från högarna. Ni vinner genom att ta den sista stickan som finns kvar i sista högen. Licka till!");
        }

        static void spel()
        {
            int[] pinnar = {5, 5, 5};
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