using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualBasic;

namespace TjuvPolisMedborgare
{
    public class Person
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XDir { get; set; }
        public int YDir { get; set; }

        // Konstruktor för att initiera position och riktning
        public Person(int x, int y, int xDir, int yDir)
        {
            X = x;
            Y = y;
            XDir = xDir;
            YDir = yDir;
        }

        // Metod för att uppdatera position slumpmässigt
        public void MoveRandomly(int cityWidth = 100, int cityHeight = 25)
        {
            X += XDir;
            Y += YDir;
            X = (X + cityWidth) % cityWidth;
            Y = (Y + cityHeight) % cityHeight;
        }

        // Metod för att kontrollera kollision med en annan person
        public bool HasCollided(Person other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public virtual void Interact(Person other)
        {
            // Måste finnas 
        }     
    }

    class Police : Person
    {
        public List<string> seizedItems { get; set; }

        public Police(int x, int y, int xDir, int yDir) : base(x, y, xDir, yDir)
        {
            seizedItems = new List<string>();
        }

        // Interaktion med Thief: polisen arresterar tjuven och beslagtar all loot
        public override void Interact(Person person)
        {
            if (person is Thief thief)
            {
                if (thief.loot.Count > 0)
                {
                    seizedItems.AddRange(thief.loot);
                    thief.loot.Clear();
                    Console.WriteLine(Helpers.Arrest + "och tar allt stöldgods ");
                }
                else
                {
                    Console.WriteLine(Helpers.NoLoot);
                }
            }
        }
    }

    class Thief : Person
    {
        public List<string> loot { get; set; }

        public Thief(int x, int y, int xDir, int yDir) : base(x, y, xDir, yDir)
        {
            loot = new List<string>();
        }

        // Interaktion med Citizen: stjäl en slumpmässig sak från Citizen
        public override void Interact(Person person)
        {
            if (person is Citizen citizen)
            {
                // Tjuven stjäl något från medborgaren
                if (citizen.belongings.Count > 0)
                {
                    Random random = new Random();
                    int itemIndex = random.Next(citizen.belongings.Count);
                    string stolenItem = citizen.belongings[itemIndex];
                    citizen.belongings.RemoveAt(itemIndex);
                    loot.Add(stolenItem);

                    Console.WriteLine(Helpers.Robbery + " och tar " + stolenItem);
                }
                else
                {
                    Console.WriteLine(Helpers.NoBelongings);
                }
            }
        }
    }

    class Citizen : Person
    {
        public List<string> belongings { get; set; }

        // Konstruktor som sätter default tillhörigheter
        public Citizen(int x, int y, int xDir, int yDir) : base(x, y, xDir, yDir)
        {
            // Initiera belongings-listan med några föremål
            belongings = new List<string> { "klocka", "plånbok", "nycklar", "mobiltelefon" };
        }
        public override void Interact(Person person)
        {
            if (person is Police police)
            {
                Console.WriteLine(Helpers.NoAction);
            }
        }
    }
}
