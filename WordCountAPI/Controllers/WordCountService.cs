using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace WordCountService.Controllers
{
    public class WordCountController : ApiController
    {
        [HttpPost]
        public Dictionary<string, int> CountWords(string text)
        {
            // создаем словарь
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // разбиваем текст на слова и помещаем их в массив
            string[] words = text.Split(new[] { ' ', ',', '.', ':', ';', '!', '?', '(', ')', '"', '-', 'I', 'V', 'X', '/', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, StringSplitOptions.RemoveEmptyEntries);

            // создаем список задач
            List<Task> tasks = new List<Task>();

            // запускаем задачу для каждого слова
            foreach (string word in words)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    lock (dictionary) // блокируем доступ к словарю
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
                }));
            }

            // ожидаем завершения всех задач
            Task.WaitAll(tasks.ToArray());

            return dictionary; // возвращаем результат
        }
    }
}
