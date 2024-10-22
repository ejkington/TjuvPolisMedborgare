using Microsoft.VisualBasic;
using System.Threading;
namespace TjuvPolisMedborgare
{
    internal class Program
    {
        static void Main(string[] args)
        {
            City city = new City(100, 25, numOfTheifes: 10, numOfPolices: 10, numOfCitizens: 20);

            while (true)
            {
                Console.Clear();
                city.DrawCity();
                city.Simulate(1);
                city.UpdateCity();


                Console.WriteLine("STATUS: ");
                Console.WriteLine("Antal gripna tjuvar: " + city.Arrests);
                Console.WriteLine("Antal rånade medborgare: " + city.Robberies);
               
                Thread.Sleep(800);
            }
        }
    }
}
