using System;
using System.Threading.Tasks;

namespace GameWebApi
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);
        Task<Item> CreateItem(Guid playerId, NewItem item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, ModifiedItem item);
        Task<Item> DeleteItem(Guid playerId, Item item);
    }
}