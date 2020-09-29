using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GameWebApi
{
    public class Program
    {
        public static void Main(string[] args) // public static void
        {
            /*
            Player player = new Player();
            player.Id = Guid.NewGuid();
            player.Name = "tedgdgh";
            player.Score = 45;
            player.Level = 5;
            player.IsBanned = true;
            player.CreationTime = DateTime.Now;
            
            //Guid id = new Guid("bfad9147-1dbd-4b63-af62-8ccb6ff296fe");
            Guid id = new Guid("608be638-b6d3-474d-968b-d35bac1f9b6e");

            FileRepository fr = new FileRepository();
            Player[] players = await fr.GetAll();
            await fr.Delete(new Guid("f3208e65-653d-47b9-a87e-dcbca8e9dfe3"));
            ModifiedPlayer mod = new ModifiedPlayer();
            mod.Score = 11;
            await fr.Modify(new Guid("ea33e16a-718b-454b-b0ee-0df27106a5bf"), mod);
            Console.WriteLine();
            /*
            Player p = await fr.Get(id);
            
            Console.WriteLine("Id: " + p.Id);
            Console.WriteLine("Name: " + p.Name);
            Console.WriteLine("Score: " + p.Score);
            Console.WriteLine("Level: " + p.Level);
            Console.WriteLine("IsBanned: " + p.IsBanned);
            Console.WriteLine("CreationDate: " + p.CreationTime);
            */
            
            //await fr.Create(player);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
