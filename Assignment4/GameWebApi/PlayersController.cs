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

        [HttpGet]
        public Task<Player[]> GetAll()
        {
            Console.WriteLine("GetAll(players)");
            //Player[] p = await repo.GetAll();
            return repo.GetAll();
            //return p;
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

        [HttpPost("delete/{id:guid}")]
        public Task<Player> Delete(Guid id)
        {
            return repo.Delete(id);   
        }

        [HttpOptions]
        public void Options() { }

    }
}