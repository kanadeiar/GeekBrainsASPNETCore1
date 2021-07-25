using System;
using System.Net.Http;
using WebStore.WebAPI.Client;

namespace ConsoleAppWebAPI
{
    class Program
    {
        private static string __WebAPI = "http://localhost:5001";
        static void Main(string[] args)
        {
            Console.WriteLine("Тестирование изменений данных через сервис");
            
            var client = new ValuesClient(new HttpClient{BaseAddress = new Uri(__WebAPI)});

            PrintData(client, "Элементы изначально:");

            Console.WriteLine("Нажмите любую кнопку и будет удален один элемент");
            Console.ReadKey();

            var result = client.Delete(0);
            if (result)
                Console.WriteLine("Элемент успешно удален");

            Console.WriteLine("Нажмите кнопку для добавления элемента");
            Console.ReadKey();

            client.Add("new");
            Console.WriteLine("Элемент добавлен");

            PrintData(client, "Элементы сейчас:");

            Console.WriteLine("Для завершения нажмите любую кнопку ...");
            Console.ReadLine();
        }

        #region Вспомогательное

        private static void PrintData(ValuesClient client, string message)
        {
            Console.WriteLine(message);
            foreach (var item in client.GetAll())
            {
                Console.WriteLine($"{item}");
            }
        }

        #endregion
    }
}
