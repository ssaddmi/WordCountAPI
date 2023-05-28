using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MyLibrary
{
    internal class MyDictionary
    {
        private Dictionary<string, int> CountWords(string text)
        {
            // разбиваем текст на слова и помещаем их в массив
            string[] words = text.Split(new[] { ' ', ',', '.', ':', ';', '!', '?', '(', ')', '"', '-', 'I', 'V', 'X', '/', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, StringSplitOptions.RemoveEmptyEntries);

            // создаем словарь
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // добавляем каждое слово в словарь и подсчитываем число его повторений
            foreach (string word in words)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word]++;
                }
                else
                {
                    dictionary.Add(word, 1);
                }
            }

            return dictionary; // возвращаем результат
        }

        public Dictionary<string, int> CountWords2(string text)
        {
            // создаем ConcurrentDictionary
            ConcurrentDictionary<string, int> dictionary = new ConcurrentDictionary<string, int>();

            // разбиваем текст на слова и помещаем их в массив
            string[] words = text.Split(new[] { ' ', ',', '.', ':', ';', '!', '?', '(', ')', '"', '-', 'I', 'V', 'X', '/', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, StringSplitOptions.RemoveEmptyEntries);

            // создаем список задач
            List<Task> tasks = new List<Task>();

            // запускаем задачу для каждого слова
            foreach (string word in words)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    dictionary.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
                }));
            }

            // ожидаем завершения всех задач
            Task.WaitAll(tasks.ToArray());

            return new Dictionary<string, int>(dictionary); // создаем обычный словарь из ConcurrentDictionary и возвращаем результат
        }
    }
}

