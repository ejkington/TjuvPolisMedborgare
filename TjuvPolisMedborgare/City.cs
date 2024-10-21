using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TjuvPolisMedborgare
{
    public class City
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int NumOfTheifes { get; set; }
        public int NumOfPolices { get; set; }
        public int NumOfCitizens { get; set; }
        public int Arrests { get; set; }
        public int Robberies { get; set; }
        public List<Person> People { get; set; }

        public City(int width, int height, int numOfTheifes, int numOfPolices, int numOfCitizens)
        {
            Width = width;
            Height = height;
            NumOfTheifes = numOfTheifes;
            NumOfPolices = numOfPolices;
            NumOfCitizens = numOfCitizens;
            Arrests = 0;
            Robberies = 0;
            People = new List<Person>();

            CreatePeople();
        }

        public void DrawCity()
        {
            // Skapa en 2D-array som representerar staden
            char[,] cityGrid = new char[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    cityGrid[y, x] = ' ';
                }
            }

            foreach (var person in People)
            {
                if (person is Thief)
                {
                    cityGrid[person.Y, person.X] = 'T'; // Tjuv
                }
                else if (person is Police)
                {
                    cityGrid[person.Y, person.X] = 'P'; // Polis
                }
                else if (person is Citizen)
                {
                    cityGrid[person.Y, person.X] = 'M'; // Medborgare
                }
            }

            // Rita ut staden
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.Write(cityGrid[y, x]);
                }
                Console.WriteLine();
            }
        }

        private void CreatePeople()
        {
            Random random = new Random();

            // Skapa tjuvar, värdets sätts när man skapar ny stad
            for (int i = 0; i < NumOfTheifes; i++)
            {
                int x = random.Next(0, Width);
                int y = random.Next(0, Height);
                int xDir = random.Next(-1, 2);
                int yDir = random.Next(-1, 2);
                People.Add(new Thief(x, y, xDir, yDir));
            }

            // Skapa poliser, värdets sätts när man skapar ny stad
            for (int i = 0; i < NumOfPolices; i++)
            {
                int x = random.Next(0, Width);
                int y = random.Next(0, Height);
                int xDir = random.Next(-1, 2);
                int yDir = random.Next(-1, 2);
                People.Add(new Police(x, y, xDir, yDir));
            }

            // Skapa medborgare, värdets sätts när man skapar ny stad
            for (int i = 0; i < NumOfCitizens; i++)
            {
                int x = random.Next(0, Width);
                int y = random.Next(0, Height);
                int xDir = random.Next(-1, 2);
                int yDir = random.Next(-1, 2);
                People.Add(new Citizen(x, y, xDir, yDir));
            }
        }

        public void UpdateCity()
        {
            // Flytta alla personer
            foreach (var person in People)
            {
                person.MoveRandomly(Width, Height);
            }

            //  Hantera kollisioner och interaktioner mellan personer
            string collisionResult = HandleCollisions();
        }

        public void Simulate(int steps)
        {
            for (int step = 0; step < steps; step++)
            {
                foreach (var person in People)
                {
                    person.MoveRandomly(Width, Height);
                }

                HandleCollisions();
            }
        }

        private string HandleCollisions()
        {
            // loopar över alla personer och kollar om någon kolliderar med någon annan
            for (int i = 0; i < People.Count; i++)
            {
                for (int j = i + 1; j < People.Count; j++)
                {
                    var person1 = People[i];
                    var person2 = People[j];

                    // Kolla om personerna kolliderar
                    if (person1.HasCollided(person2))
                    {
                        person1.Interact(person2);
                        person2.Interact(person1);

                        // Skriv ut meddelanden beroende på vad som händer
                        if (person1 is Thief && person2 is Citizen)
                        {
                            Robberies++;
                            return Helpers.Robbery;
                        }
                        else if (person1 is Police && person2 is Thief)
                        {
                            Arrests++;
                            People.Remove(person2); // Tjuven tas bort från staden
                            return Helpers.Arrest;

                        }
                        else if (person1 is Citizen && person2 is Thief)
                        {
                            Robberies++;
                            return Helpers.Robbery;

                        }
                        else if (person1 is Thief && person2 is Police)
                        {
                            Arrests++;
                            People.Remove(person1); // Tjuven tas bort från staden
                            return Helpers.Arrest;

                        }
                    }
                }
            }
            return string.Empty; // behövdes för att annars retunerade inte de nåt värde och då funkade inte HandleCollisions metoden
        }
    }
}
