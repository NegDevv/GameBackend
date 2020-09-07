using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    public static class PlayerExtensions
    {
        public static Item GetHighestLevelItem(this Player player)
        {
            if (player.Items != null)
            {
                Item highest = player.Items[0];
                foreach (Item i in player.Items)
                {
                    if (i.Level > highest.Level)
                    {
                        highest = i;
                    }
                }
                return highest;
            }
            Console.WriteLine("Item list for this player doesnt exist!");
            return null;
        }

    }




    class Program
    {
        static void Main(string[] args)
        {
            // 1. Guid
            List<Player> players = InstantiatePlayers();

            // 2. Extension method
            Console.WriteLine("2. Extension method\n");
            Player pl = new Player();

            Item boots = new Item();
            boots.Id = Guid.NewGuid();
            boots.Level = 3;

            Item hat = new Item();
            hat.Id = Guid.NewGuid();
            hat.Level = 6;

            Item shirt = new Item();
            shirt.Id = Guid.NewGuid();
            shirt.Level = 7;

            List<Item> playerItems = new List<Item>();

            playerItems.Add(boots);
            playerItems.Add(hat);
            playerItems.Add(shirt);

            pl.Items = playerItems;

            
            if (pl.GetHighestLevelItem() != null)
            {
                Console.WriteLine("Highest item level: " + pl.GetHighestLevelItem().Level + "\n");
            }

            // 3. LINQ
            Console.WriteLine("3. LINQ\n");
            Console.WriteLine("Normal version");
            Item[] items = GetItems(pl);

            // Print item levels to test
            foreach (Item i in items)
            {
                Console.WriteLine(i.Level);
            }

            Console.WriteLine("LINQ version");

            items = GetItemsWithLinq(pl);

            // Print item levels to test
            foreach (Item i in items)
            {
                Console.WriteLine(i.Level);
            }
            Console.WriteLine();

            // 4. LINQ 2
            Console.WriteLine("4. LINQ 2\n");
            Console.WriteLine("First item level: " + FirstItem(pl).Level);
            Console.WriteLine("First item level with LINQ: " + FirstItemWithLinq(pl).Level + "\n");

            // 5.Delegates
            Console.WriteLine("5. Delegates\n");
            ProcessEachItem(pl, PrintItem);

            // 6. Lambda
            Console.WriteLine("6. Lambda\n");
            Console.WriteLine("Lambda version: \n");

            ProcessEachItem(pl, it =>
            {
                Console.WriteLine("Id: " + it.Id);
                Console.WriteLine("Level: " + it.Level + "\n");
            });

            // 7. Generics
            Console.WriteLine("7. Generics\n");
            List<Player> players1 = new List<Player>();
            List<PlayerForAnotherGame> playersAnother = new List<PlayerForAnotherGame>();

            Random r = new Random();

            // Generate 100 players with random score between(0,99) and add them to the player list
            for (int i = 0; i < 100; i++)
            {
                Player p = new Player();
                p.Score = r.Next(0, 100);
                players1.Add(p);
            }

            // Generate 100 players with random score between(0,99) and add them to the another player list
            for (int i = 0; i < 100; i++)
            {
                PlayerForAnotherGame p = new PlayerForAnotherGame();
                p.Score = r.Next(0, 100);
                playersAnother.Add(p);
            }

            
            Game<Player> game = new Game<Player>(players1);
            Game<PlayerForAnotherGame> anotherGame = new Game<PlayerForAnotherGame>(playersAnother);

            Player[] top10 = game.GetTop10Players();
            PlayerForAnotherGame[] top10Another = anotherGame.GetTop10Players();

            Console.WriteLine("Top10");

            for (int i = 0; i < top10.Length; i++)
            {
                Console.WriteLine(i + 1 + ": " + top10[i].Score);
            }

            Console.WriteLine("\nTop10 another game");

            for (int i = 0; i < top10Another.Length; i++)
            {
                Console.WriteLine(i + 1 + ": " + top10Another[i].Score);
            }

        }


        public static void PrintItem(Item item)
        {
            Console.WriteLine("Id: " + item.Id);
            Console.WriteLine("Level: " + item.Level + "\n");
        }
        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach (Item i in player.Items)
            {
                process(i);
            }
        }



        public static Item FirstItem(Player p)
        {
            if (p.Items != null)
            {
                return p.Items[0];
            }
            Console.WriteLine("Item list was empty!");
            return null;
        }

        public static Item FirstItemWithLinq(Player p)
        {
            if (p.Items != null)
            {
                return p.Items.First();
            }
            Console.WriteLine("Item list was empty!");
            return null;
        }

        public static Item[] GetItems(Player p)
        {
            Item[] items = new Item[p.Items.Count()];

            for (int i = 0; i < p.Items.Count; i++)
            {
                items[i] = p.Items[i];
            }
            return items;
        }

        public static Item[] GetItemsWithLinq(Player p)
        {
            return p.Items.ToArray();
        }


        public static List<Player> InstantiatePlayers()
        {
            List<Player> players = new List<Player>();
            IDictionary<Guid, string> playerIDs = new Dictionary<Guid, string>();
            Guid testID = Guid.NewGuid();

            for (int i = 0; i < 1000000; i++)
            {
                Player p = new Player();
                Guid id = Guid.NewGuid();

                // Generate new keys until unique key is found
                while (playerIDs.ContainsKey(id)) // Checks Dictionary for existing key
                {
                    id = Guid.NewGuid();
                    Console.WriteLine("Key already exists!");
                }

                playerIDs.Add(id, id.ToString());
                p.Id = id;

                players.Add(p);
            }
            return players;
        }
    }
}
