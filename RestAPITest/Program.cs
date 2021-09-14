using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RestAPITest
{
    class Program
    {
        #region Records
        private record Item(int ID, string Name, int CategoryID);
        #endregion
        #region Fields
        private static List<Item> _items = new();
        private static Dictionary<int, string> _categories = new();
        private const string GET_URL = "http://tester.consimple.pro";
        #endregion
        #region Methods
        static string GetRequest(string url)
        {
            HttpClient client = new();
            string result = string.Empty;
            try
            {
                result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception!");
                Console.WriteLine($"{e.Message}");
            }
            return result;
        }
        #endregion
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            while (true)
            {
                Console.Clear();

                Console.WriteLine($"1. Make request\n2. Exit");
                var choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    Console.WriteLine("Making request...");
                    var response = GetRequest(GET_URL);
                    try
                    {
                        _categories.Clear();
                        _items.Clear();
                        JObject json = JObject.Parse(response);
                        foreach (var product in json["Products"])
                        {
                            var id = Convert.ToInt32(product["Id"]);
                            var name = product["Name"].ToString();
                            var catId = Convert.ToInt32(product["CategoryId"]);
                            _items.Add(new(id, name, catId));
                        }
                        foreach (var category in json["Categories"])
                            _categories[Convert.ToInt32(category["Id"].ToString())] = category["Name"].ToString();

                        Console.WriteLine(string.Format("{0,25}\t|\t{1,5}", "Product name", "Category"));

                        foreach (var item in _items)
                            Console.WriteLine(string.Format("{0,25}\t|\t{1,2}", $"{item.Name}", $"{_categories[item.CategoryID]}"));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error occured");
                    }
                    Console.ReadKey();

                }
                else if (choice == 2)
                    Environment.Exit(0);
            }
        }
    }
}
