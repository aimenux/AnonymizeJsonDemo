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
                Email = "joe@snow.com"
            };

            var resolver = new AnonymizeContractResolver();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver
            };
            var json = JsonConvert.SerializeObject(employee, settings);
            Console.WriteLine(json);
            Console.ReadKey();
        }
    }
}
