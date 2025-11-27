using System;
using System.Text;
using System.Threading;

namespace Lab13
{
    class Program
    {
        // Об'єкт для блокування консолі (щоб текст не змішувався)
        static readonly object consoleLock = new object();

        // Дані для обчислень
        static double[] VectorX;
        static double[] VectorY;
        static double ResultA = 0;
        static double ResultB = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Лабораторна робота №13 | Литвиненко Дмитро | Варіант 8";

            // 1. Ініціалізація даних
            InitializeVectors();

            // 2. Гарний заголовок
            PrintHeader();
            Console.WriteLine($"[info] Вектор X (NX=85) згенеровано.");
            Console.WriteLine($"[info] Вектор Y (NY=150) згенеровано.\n");

            // 3. Налаштування потоків
            Thread t0 = new Thread(CalculateA);
            Thread t1 = new Thread(CalculateB);

            t0.Name = "T0 (Highest)";
            t0.Priority = ThreadPriority.Highest;

            t1.Name = "T1 (Lowest)";
            t1.Priority = ThreadPriority.Lowest;

            // 4. Запуск
            PrintColored($"[*] Запуск потоку {t0.Name} з пріоритетом {t0.Priority}...", ConsoleColor.Cyan);
            PrintColored($"[*] Запуск потоку {t1.Name} з пріоритетом {t1.Priority}...", ConsoleColor.Yellow);
            Console.WriteLine();

            t0.Start();
            t1.Start();

            // 5. Очікування завершення
            t0.Join();
            t1.Join();

            // 6. Вивід результатів
            Console.WriteLine("\n=============================================");
            Console.WriteLine("           РЕЗУЛЬТАТИ ОБЧИСЛЕНЬ");
            Console.WriteLine("=============================================");

            PrintColored($"Результат A (Сума): {ResultA:F4}", ConsoleColor.Cyan);
            PrintColored($"Результат B (Сума): {ResultB:F4}", ConsoleColor.Yellow);

            PrintFooter();
        }

        static void InitializeVectors()
        {
            Random rnd = new Random();

            VectorX = new double[85];
            for (int i = 0; i < VectorX.Length; i++)
                VectorX[i] = rnd.NextDouble() * 250.0;

            VectorY = new double[150];
            for (int i = 0; i < VectorY.Length; i++)
                VectorY[i] = -10.0 + (rnd.NextDouble() * 10.0);
        }

        // Потік T0 (Highest) -> Cyan
        static void CalculateA()
        {
            double sum = 0;
            // Імітація складних обчислень
            for (int i = 0; i < VectorX.Length; i++)
            {
                double x = VectorX[i];
                double argument = x * Math.Sin(Math.E * x * x);

                if (argument >= 0)
                    sum += Math.Pow(argument, 1.0 / 3.0);
                else
                    sum -= Math.Pow(Math.Abs(argument), 1.0 / 3.0);

                // Штучне навантаження, щоб візуалізатор встиг зафіксувати роботу
                Thread.SpinWait(100000);
            }
            ResultA = sum;
            PrintColored($"-> Потік {Thread.CurrentThread.Name} завершив роботу.", ConsoleColor.Cyan);
        }

        // Потік T1 (Lowest) -> Yellow
        static void CalculateB()
        {
            double sum = 0;
            for (int i = 0; i < VectorY.Length; i++)
            {
                double y = VectorY[i];
                double term = y * Math.Cos(y * y);

                if (term >= 0)
                    sum += Math.Pow(term, 0.25);

                // Штучне навантаження
                Thread.SpinWait(100000);
            }
            ResultB = sum;
            PrintColored($"-> Потік {Thread.CurrentThread.Name} завершив роботу.", ConsoleColor.Yellow);
        }

        static void PrintColored(string message, ConsoleColor color)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================================");
            Console.WriteLine("    ЛАБОРАТОРНА РОБОТА №13");
            Console.WriteLine("    Пріоритети потоків");
            Console.WriteLine("=============================================");
            Console.ResetColor();
        }

        static void PrintFooter()
        {
            Console.WriteLine("\n\n=============================================");
            Console.WriteLine("Робота завершена. Натисніть Enter...");
            Console.ReadKey();
        }
    }
}