using Newtonsoft.Json;
using System;

namespace DummyDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new SampleRepository();
            var customers = repository.GetCustomers();

            Console.WriteLine(JsonConvert.SerializeObject(customers, Formatting.Indented));
            //Console.WriteLine(customers);
        }
    }
}
