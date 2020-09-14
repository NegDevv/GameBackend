using System;
using System.Linq;
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
            return repo.GetAll();
        }

        [HttpPost("create")]
        public Task<Player> Create([FromBody] NewPlayer player)
        {
            Player p = new Player();
            p.Name = player.Name;
            p.Id = Guid.NewGuid();
            p.Score = 0;
            p.Level = 0;
            p.IsBanned = false;
            p.CreationTime = DateTime.Now;

            return repo.Create(p);
        }

        [HttpPost("modify/{id:guid}")]
        public Task<Player> Modify(Guid id, [FromBody]ModifiedPlayer player)
        {
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