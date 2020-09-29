using System;
using System.Threading.Tasks;

namespace GameWebApi
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player[]> GetPlayersWithMinScore(int minScore);
        Task<Player> GetPlayerWithName(string name);
        Task<Player[]> GetPlayersWithAtLeastItemLevel(int level);
        Task<Player[]> GetPlayersWithMinNroItems(int itemCount);
        Task<Player[]> GetPlayersWithTag(string tag);
        Task<Player[]> GetTop10Players();
        Task<int> GetMostCommonLevel();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> UpdatePlayerName(Guid id, string newName);
        Task<Player> AddNewItem(Guid id, Item newItem);
        Task<Player> TradeItem(Guid id, Guid itemId);
        Task<Player> IncrementPlayerScore(Guid id, int increment);
        Task<Player> Delete(Guid id);
        Task<Item> CreateItem(Guid playerId, NewItem item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, ModifiedItem item);
        Task<Item> DeleteItem(Guid playerId, Item item);
    }
}