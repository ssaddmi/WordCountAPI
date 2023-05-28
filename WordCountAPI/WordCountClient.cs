using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WordCountClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut volutpat velit id erat porttitor, vel viverra massa pulvinar. Nullam vel fringilla mi. Praesent non pretium libero. Donec risus mi, elementum nec vestibulum vitae, dictum non augue. Duis interdum, velit vel interdum sagittis, odio tortor vulputate magna, at consequat purus elit eget nibh. Integer sed diam neque. Etiam laoreet egestas neque, ut elementum turpis pulvinar non. Integer dictum neque at velit sagittis malesuada. Aliquam ullamcorper, quam varius blandit aliquam, sapien elit placerat est, vel vulputate nisi augue vel risus.";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:12345/"); // адрес сервиса

                var response = await client.PostAsJsonAsync("api/wordcount", text); // вызываем метод сервиса

                if (response.IsSuccessStatusCode)
                {
                    var dictionary = await response.Content.ReadAsAsync<Dictionary<string, int>>(); // получаем результат
                    foreach (var item in dictionary)
                    {
                        Console.WriteLine($"{item.Key} : {item.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }

            Console.ReadKey();
        }
    }
}
