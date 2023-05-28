using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyProgram
{
    class MyClient
    {
        static void Main(string[] args)
        {
            // читаем текст из файла
            string text = File.ReadAllText(@"C:\vs_projects\DD\W&P.txt");

            // загружаем сборку
            Assembly myAssembly = Assembly.LoadFrom("MyLibrary.dll");

            // создаем экземпляр класса MyDictionary
            object myDictionary = myAssembly.CreateInstance("MyLibrary.MyDictionary");

            // получаем метод CountWords
            MethodInfo countWordsMethod = myDictionary.GetType().GetMethod("CountWords", BindingFlags.Instance | BindingFlags.NonPublic);

            // вызываем приватный метод и получаем результат
            var stopwatch = Stopwatch.StartNew();
            var result = countWordsMethod.Invoke(myDictionary, new object[] { text }) as Dictionary<string, int>;
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения приватного метода: {stopwatch.ElapsedMilliseconds} мс");

            var sortedResult = result.OrderByDescending(x => x.Value);

            // записываем результат в файл
            using (StreamWriter writer = new StreamWriter(@"output.txt"))
            {
                foreach (var item in sortedResult)
                {
                    writer.WriteLine($"{item.Key}: {item.Value}");
                }
                Console.WriteLine("Всё готово! Открывайте файл");
            }

            // получаем метод CountWords2
            MethodInfo countWords2Method = myDictionary.GetType().GetMethod("CountWords2");

            // вызываем публичный метод и получаем результат
            stopwatch.Restart();
            result = countWords2Method.Invoke(myDictionary, new object[] { text }) as Dictionary<string, int>;
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения публичного метода: {stopwatch.ElapsedMilliseconds} мс");

            sortedResult = result.OrderByDescending(x => x.Value);

            // записываем результат в файл
            using (StreamWriter writer = new StreamWriter(@"output2.txt"))
            {
                foreach (var item in sortedResult)
                {
                    writer.WriteLine($"{item.Key}: {item.Value}");
                }
                Console.WriteLine("Всё готово! Открывайте файл");
            }

            Console.ReadKey();
        }
    }
}