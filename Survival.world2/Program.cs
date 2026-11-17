using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survivel.world2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int playerHealth = 100, maxHealth = 100, eat = 100, maxEat = 100, gold = 0,
      posX = 0, posY = 0, turns = 0;

           
            int mapWidth = 100, mapHeight = 20;
            int hudPosition = mapHeight + 2;
            int inputPosition = hudPosition + 2;

            Random rnd = new Random();

            // сундуки
            int boxCount = 15;
            int[] boxX = new int[boxCount];
            int[] boxY = new int[boxCount];
            bool[] boxAlive = new bool[boxCount];

            for (int b = 0; b < boxCount; b++)
            {
                int attempt = 0;
                while (attempt < 200)
                {
                    int x = rnd.Next(0, mapWidth);
                    int y = rnd.Next(0, mapHeight);
                    bool free = true;

                    if (x == posX && y == posY) free = false;

                    for (int k = 0; k < b; k++)
                    {
                        if (boxX[k] == x && boxY[k] == y) free = false;
                    }

                    if (free)
                    {
                        boxX[b] = x;
                        boxY[b] = y;
                        boxAlive[b] = true;
                        break;
                    }
                    attempt++;
                }
            }

            // мобы
            int mob = 10;
            int[] skelletX = new int[mob];
            int[] skelletY = new int[mob];

            for (int m = 0; m < mob; m++)
            {
                int attempt2 = 0;
                while (attempt2 < 200)
                {
                    int x = rnd.Next(0, mapWidth);
                    int y = rnd.Next(0, mapHeight);
                    bool free = true;

                    if (x == posX && y == posY) free = false;

                    // не на другом мобе
                    for (int a = 0; a < m; a++)
                    {
                        if (skelletX[a] == x && skelletY[a] == y) free = false;
                    }

                    // не на сундуке
                    for (int b = 0; b < boxCount; b++)
                    {
                        if (boxAlive[b] && boxX[b] == x && boxY[b] == y) free = false;
                    }

                    if (free)
                    {
                        skelletX[m] = x;
                        skelletY[m] = y;
                        break;
                    }
                    attempt2++;
                }
            }

            
            while (true)
            {
                
                Console.SetCursorPosition(0, 0);
                for (int v = 0; v < mapHeight; v++)
                {
                    for (int n = 0; n < mapWidth; n++)
                    {
                        // игрок
                        if (n == posX && v == posY)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write('H');
                            Console.ResetColor();
                            continue;
                        }

                        // моб
                        bool isMob = false;
                        for (int m = 0; m < mob; m++)
                        {
                            if (skelletX[m] == n && skelletY[m] == v)
                            {
                                isMob = true;
                                break;
                            }
                        }
                        if (isMob)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write('K');
                            Console.ResetColor();
                            continue;
                        }

                        // сундук
                        bool isBox = false;
                        for (int b = 0; b < boxCount; b++)
                        {
                            if (boxAlive[b] && boxX[b] == n && boxY[b] == v)
                            {
                                isBox = true;
                                break;
                            }
                        }
                        if (isBox)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write('☐');
                            Console.ResetColor();
                            continue;
                        }

                        
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write('#');
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }

                
                Console.SetCursorPosition(0, hudPosition);
                Console.WriteLine($" {playerHealth}/{maxHealth} {eat}/{maxEat} ${gold,-5}    ");

                
                Console.SetCursorPosition(0, inputPosition);
                Console.Write("Введите направление движения:  ");
                var key = Console.ReadKey();
                int dirX = 0, dirY = 0;

                if (key.Key == ConsoleKey.D)
                {
                    if (posX == mapWidth - 1) continue;
                    dirX = 1;
                }
                else if (key.Key == ConsoleKey.A)
                {
                    if (posX == 0) continue;
                    dirX = -1;
                }
                else if (key.Key == ConsoleKey.W)
                {
                    if (posY == 0) continue;
                    dirY = -1;
                }
                else if (key.Key == ConsoleKey.S)
                {
                    if (posY == mapHeight - 1) continue;
                    dirY = 1;
                }

                posX += dirX;
                posY += dirY;

                // голод 
                if (dirX != 0 || dirY != 0)
                {
                    turns++;
                    if (turns % 3 == 0) eat = eat - 1;

                    if (eat <= 0)
                    {
                        Console.SetCursorPosition(0, hudPosition + 1);
                        Console.WriteLine("Персонаж умер от голода.     ");
                        return;
                    }
                }

                // столкновение с мобом 
                for (int m = 0; m < mob; m++)
                {
                    if (posX == skelletX[m] && posY == skelletY[m])
                    {
                        int damage = rnd.Next(2, 6); 
                        switch (damage)
                        {
                            case 2: playerHealth -= 2; break;
                            case 3: playerHealth -= 3; break;
                            case 4: playerHealth -= 4; break;
                            case 5: playerHealth -= 5; break;
                        }

                        if (playerHealth <= 0)
                        {
                            playerHealth = 0;
                            Console.SetCursorPosition(0, hudPosition + 1);
                            Console.WriteLine("Вы мертвы ");
                            return;
                        }
                    }
                }

                // подбор сундука 
                for (int b = 0; b < boxCount; b++)
                {
                    if (boxAlive[b] && posX == boxX[b] && posY == boxY[b])
                    {
                        boxAlive[b] = false;

                        int dropType = rnd.Next(1, 4);
                        switch (dropType)
                        {
                            case 1:
                                eat += rnd.Next(10, 31);
                                if (eat > maxEat) eat = maxEat;
                                break;
                            case 2:
                                gold += rnd.Next(5, 21);
                                break;
                            case 3:
                                eat += rnd.Next(5, 16);
                                if (eat > maxEat) eat = maxEat;
                                gold += rnd.Next(3, 11);
                                break;
                        }
                    }
                }
            }

        }
    }
}
