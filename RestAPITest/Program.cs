using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RestAPITest
{
    class Program
    {
        #region Records
        private record Item(int ID, string Name, int CategoryId);
        #endregion
        #region Fields
        private static List<Item> _items = new();
        private static Dictionary<int, string> _categories = new();
        private const string REQUEST_URL = "http://tester.consimple.pro";
        #endregion
        #region Methods
        static string RequestJson(string url)
        {
            using HttpClient client = new();
            string result = string.Empty;
            try
            {
                result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Exception!\n{e.Message}");
            }
            return result;
        }
        #endregion
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine($"1. Make request\n2. Exit");
                    var user_choice = int.Parse(Console.ReadLine());
                    if (user_choice == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Making request...");
                        var raw_response = RequestJson(REQUEST_URL);
                        _categories.Clear();
                        _items.Clear();
                        JObject json = JObject.Parse(raw_response);
                        foreach (var product in json["Products"])
                        {
                            var product_id = Convert.ToInt32(product["Id"]);
                            var product_name = product["Name"].ToString();
                            var category_id = Convert.ToInt32(product["CategoryId"]);
                            _items.Add(new(product_id, product_name, category_id));
                        }
                        foreach (var category in json["Categories"])
                        {
                            var category_id = Convert.ToInt32(category["Id"].ToString());
                            var category_name = category["Name"].ToString();
                            _categories[category_id] = category_name;
                        }
                        Console.WriteLine($"{"Product name", 25}\t|\t{"Category", 5}");
                        _items.ForEach(item => Console.WriteLine($"{item.Name, 25}\t|\t{_categories[item.CategoryId], 2}"));
                        Console.ReadKey();
                    }
                    else if (user_choice == 2)
                        Environment.Exit(0);
                }
                catch {}
            }
        }
    }
}
