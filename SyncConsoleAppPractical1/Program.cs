using System;
using System.Net;

namespace SyncConsoleAppPractical1
{
    internal class Program
    {

        static int Max { get; set; }

        static int Min { get; set; }

        static double Average { get; set; }

        static void Main(string[] args)
        {
            Console.Write("1 - Task1, 2 - Task2: ");
            int option;
            int.TryParse(Console.ReadLine(), out option);

            Console.Clear();
            switch(option)
            {
                case 1: Task1(); break;

                case 2: Task2(); break;
            }
        }

        public static void PrintRange(object r)
        {
            var range = (Range)r;

            for (int i = range.Start.Value; i < range.End.Value; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }
        }

        public static void Task1()
        {
            Console.Write("Enter start range: ");
            int startRange;
            int.TryParse(Console.ReadLine(), out startRange);

            Console.Write("Enter end range: ");
            int endRange;
            int.TryParse(Console.ReadLine(), out endRange);

            if (startRange >= endRange)
            {
                Console.Error.WriteLine("Wrong input. Error");
            }

            Range range = new Range(startRange, endRange);

            Console.Write("Enter number of threads: ");
            int countThreads;
            int.TryParse(Console.ReadLine(), out countThreads);

            for (int i = 0; i < countThreads; i++)
            {
                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(PrintRange);
                Thread newThread = new Thread(threadStart);
                newThread.Start(range);
            }
        }

        public static void GenerateListOfNumbers(object arr)
        {
            var array = arr as List<int>;

            if (array == null) return;

            Random random = new Random();

            for (int i = 0; i < 10000; i++)
            {
                array.Add(random.Next(1000));
            }
        }

        public static void FindMax(object arr)
        {
            var array = arr as List<int>;

            if (array == null) return;

            Max = array.Max();
        }

        public static void FindMin(object arr)
        {
            var array = arr as List<int>;

            if (array == null) return;

            Min = array.Min();
        }

        public static void FindAverage(object arr)
        {
            var array = arr as List<int>;

            if (array == null) return;

            Average = array.Average();
        }

        public static void WriteFile(object arr)
        {
            var array = arr as List<int>;

            if (array == null) return;

            using (StreamWriter writer = new StreamWriter("result.txt"))
            {
                foreach(var item in array)
                {
                    writer.WriteLine(item);
                }
                writer.WriteLine();

                writer.WriteLine($"Max - {Max}");
                writer.WriteLine($"Min - {Min}");
                writer.WriteLine($"Average - {Average}");
            }
        }


        public static void Task2()
        {
            List<int> arr = new List<int>();

            ParameterizedThreadStart threadStartGenerate = new ParameterizedThreadStart(GenerateListOfNumbers);
            Thread threadGenerate = new Thread(threadStartGenerate);

            ParameterizedThreadStart threadStartMax = new ParameterizedThreadStart(FindMax);
            Thread threadMax = new Thread(threadStartMax);

            ParameterizedThreadStart threadStartMin = new ParameterizedThreadStart(FindMin);
            Thread threadMin = new Thread(threadStartMin);

            ParameterizedThreadStart threadStartAverage = new ParameterizedThreadStart(FindAverage);
            Thread threadAverage = new Thread(threadStartAverage);

            threadGenerate.Start(arr);
            threadGenerate.Join();

            threadMax.Start(arr);
            threadMax.Join();

            threadMin.Start(arr);
            threadMin.Join();

            threadAverage.Start(arr);
            threadAverage.Join();

            ParameterizedThreadStart threadStartFile = new ParameterizedThreadStart(WriteFile);
            Thread threadWriteFile = new Thread(threadStartFile);

            threadWriteFile.Start(arr);
        }
    }
}
