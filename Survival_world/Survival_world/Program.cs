using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival_world
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int playerHealth = 100, maxHealth = 100, eat = 100, gold = 0,
                posX = 0, posY = 0, turns = 0;
            int enemyDamage = 0;

            Console.WriteLine("##########");
            Console.WriteLine("##########");
            Console.WriteLine("##########");
            Console.WriteLine("##########");

            Console.SetCursorPosition(posX, posY);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("H");
            Console.ResetColor();
            Console.SetCursorPosition(0, 5);

            Console.WriteLine($"{playerHealth}/{maxHealth}/{eat} ${gold,-5}");

            Console.Write("Введите урон противника: ");
            int.TryParse(Console.ReadLine(), out enemyDamage);
            playerHealth -= enemyDamage;

            if (playerHealth < 0)
                playerHealth = 0;
            Console.SetCursorPosition(0, 5);

            Console.WriteLine($"{playerHealth}/{maxHealth}/{eat} ${gold,-5}");

            Console.Write("Введите направление движения");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.D)
            {

                turns++;

                Console.SetCursorPosition(posX, posY);
                Console.WriteLine("#");
                posX += 1;
                Console.SetCursorPosition(posX, posY);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("H");
                Console.ResetColor();
                Console.SetCursorPosition(0, 5);
                //каждый третий шаг уменьшается голод
                if (turns % 3 == 0)
                {
                    eat = eat - 1;
                }


                if (eat <= 0)
                {
                    Console.SetCursorPosition(0, 6);
                    Console.WriteLine("Персонаж умер от голода.");
                    return;
                }

                Console.WriteLine($"{playerHealth}/{maxHealth}/{eat} ${gold,-5}");
            }
        
            }
    }
}
