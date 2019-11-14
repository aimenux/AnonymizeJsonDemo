using System;
using Newtonsoft.Json;

namespace AnonymizeJsonDemo
{
    public static class Program
    {
        public static void Main()
        {
            var employee = new Employee
            {
                FirstName = "Joe",
                LastName = "Snow",
                Email = "joe@snow.com",
                ContractType = "CDI"
            };

            var resolver = new AnonymizeContractResolver();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(employee, settings);
            Console.WriteLine(json);

            Console.WriteLine("\nPress any key to exit !");
            Console.ReadKey();
        }
    }
}
