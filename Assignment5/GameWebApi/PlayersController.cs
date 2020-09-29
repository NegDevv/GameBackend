using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameWebApi
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {

        private readonly IRepository repo;

        public PlayersController(IRepository _repo)
        {
            repo = _repo;
        }

        [HttpGet("{id:guid}")]
        public Task<Player> Get(Guid id)
        {
            return repo.Get(id);
        }

        //[HttpGet("{minScore:int}")]
        public Task<Player[]> GetPlayersWithMinScore([FromQuery]int minScore)
        {
            Console.WriteLine("GetPlayersWithMinScore");
            Console.WriteLine("minScore: " + minScore);
            //return null;
            return repo.GetPlayersWithMinScore(minScore);
        }
        
        [HttpGet("{name}")]
        public Task<Player> GetPlayerWithName(string name)
        {
            Console.WriteLine("GetPlayersWithName");
            Console.WriteLine("name: " + name);
            //return null;
            return repo.GetPlayerWithName(name);
        }

        //[HttpGet("{tag}")]
        public Task<Player[]> GetPlayersWithTag(string tag)
        {
            Console.WriteLine("GetPlayersWithTag");
            Console.WriteLine("name: " + tag);
            //return null;
            return repo.GetPlayersWithTag(tag);
        }

        public Task<Player[]> GetPlayersWithAtLeastItemLevel(int level)
        {
            Console.WriteLine("GetPlayersWithAtLeastItemLevel");
            Console.WriteLine("min item level: " + level);
            //return null;
            return repo.GetPlayersWithAtLeastItemLevel(level);
        }

        public Task<Player[]> GetPlayersWithMinNroItems(int itemCount)
        {
            Console.WriteLine("GetPlayersWithMinNroItems");
            Console.WriteLine("min item count: " + itemCount);
            return repo.GetPlayersWithMinNroItems(itemCount);
        }

        [HttpGet("top10")]
        public Task<Player[]> GetTop10Players()
        {
            Console.WriteLine("GetTop10Players");
            return repo.GetTop10Players();
        }

        [HttpGet("")]
        public Task<Player[]> GetAll([FromQuery]int minScore, [FromQuery]string tag, [FromQuery] int itemLevel, [FromQuery] int minItemCount)
        {
            if(minScore > 0)
            {
                return GetPlayersWithMinScore(minScore);
            }
            else if(tag != null)
            {
                return GetPlayersWithTag(tag);
            }
            else if(itemLevel > 0)
            {
                return GetPlayersWithAtLeastItemLevel(itemLevel);
            }
            else if(minItemCount > 0)
            {
                return GetPlayersWithMinNroItems(minItemCount);
            }
            else
            {
                Console.WriteLine("GetAll(players)");
                return repo.GetAll();
            }
        }
        
        [HttpGet("MostCommonLevel")]
        public Task<int> GetMostCommonLevel()
        {
            return repo.GetMostCommonLevel();
        }

        [HttpPost("create")]
        public Task<Player> Create([FromBody] NewPlayer player)
        {
            Player p = new Player();
            p.Name = player.Name;
            p.Id = Guid.NewGuid();
            p.Score = 0;
            p.Level = player.Level;
            p.IsBanned = false;
            p.CreationTime = DateTime.UtcNow;
            p.Tags = player.Tags;
            List<Item> items = new List<Item>();
            //Item[] items = new Item[0];
            p.Items = items;
            return repo.Create(p);
        }

        [HttpPost("modify/{id:guid}")]
        public Task<Player> Modify(Guid id, [FromBody]ModifiedPlayer player)
        {
            // Problem if submitting empty body: updates score to 0 
            return repo.Modify(id, player);
        }

        [HttpPost("update/{id:guid}/name")]
        public Task<Player> UpdatePlayerName(Guid id, [FromQuery] string newName)
        {
            Console.WriteLine("UpdatePlayerName");
            Console.WriteLine("newName: " + newName);
            return repo.UpdatePlayerName(id, newName);
        }

        [HttpPost("update/{id:guid}/incrementScore")]
        public Task<Player> IncrementPlayerScore(Guid id, [FromQuery] int inc)
        {
            Console.WriteLine("IncrementPlayerScore");
            Console.WriteLine("increment: " + inc);
            return repo.IncrementPlayerScore(id, inc);
        }

        
        [HttpPost("update/{id:guid}/addItem")]
        public Task<Player> AddNewItem(Guid id, [FromBody] NewItem newItem)
        {
            Item i = new Item(newItem);
            Console.WriteLine("AddNewItem");
            return repo.AddNewItem(id, i);
        }

        [HttpDelete("{id:guid}/item")]
        public Task<Player> TradeItem(Guid id, [FromBody] Guid itemId)
        {
            //Guid itemId = new Guid(itemIdString);
            Console.WriteLine("TradeItem");
            //Console.WriteLine("Item: " + newItem);
            // Problem if submitting empty body: updates score to 0 
            return repo.TradeItem(id, itemId);
        }

        [HttpPost("delete/{id:guid}")]
        public Task<Player> Delete(Guid id)
        {
            return repo.Delete(id);   
        }

        [HttpOptions]
        public void Options() { }

    }
}