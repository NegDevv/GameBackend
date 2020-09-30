using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;

namespace GameWebApi
{
    // connection string: mongodb://localhost:27017
    public class MongoDbRepository : IRepository
    {
        private IMongoCollection<Player> playersCollection;
        private IMongoCollection<BsonDocument> bsonDocumentCollection;

        public MongoDbRepository()
        {
            MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = mongoClient.GetDatabase("game");
            playersCollection = db.GetCollection<Player>("players");
        }

        public async Task<Player> Get(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return await playersCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Player[]> GetAll()
        {
            List<Player> p = await playersCollection.Find(new BsonDocument()).ToListAsync();
            return p.ToArray();
        }

        public async Task<Player[]> GetPlayersWithMinScore(int minScore)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Gte(p => p.Score, minScore);
            List<Player> p = await playersCollection.Find(filter).ToListAsync();
            return p.ToArray();
        }

        public async Task<Player> GetPlayerWithName(string name)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Name, name);
            Player p = await playersCollection.Find(filter).FirstAsync();
            return p;
        }

        public async Task<Player[]> GetPlayersWithTag(string tag)
        {
            var filter = Builders<Player>.Filter.Eq("Tags", tag);
            List<Player> p = await playersCollection.Find(filter).ToListAsync();
            return p.ToArray();
        }

        public async Task<Player[]> GetPlayersWithAtLeastItemLevel(int minLevel)
        {
            var filter = Builders<Player>.Filter.ElemMatch<Item>(p => p.Items, Builders<Item>.Filter.Gte(i => i.Level, minLevel));
            List<Player> p = await playersCollection.Find(filter).ToListAsync();
            return p.ToArray();
        }

         public async Task<Player[]> GetPlayersWithMinNroItems(int itemCount)
        {
            var filter = Builders<Player>.Filter.SizeGte(p => p.Items, itemCount);
            List<Player> p = await playersCollection.Find(filter).ToListAsync();
            return p.ToArray();
        }

        public async Task<Player[]> GetTop10Players()
        {
            SortDefinition<Player> sortDef = Builders<Player>.Sort.Descending(p => p.Score);
            List<Player> top10 = await playersCollection.Find(new BsonDocument()).Sort(sortDef).Limit(10).ToListAsync();
            return top10.ToArray();
        }

        public async Task<int> GetMostCommonLevel()
        {
            List<LevelCount> levelCounts = 
                await playersCollection.Aggregate()
                    .Project(p => p.Level)
                    .Group(l => l, p => new LevelCount { Id = p.Key, Count = p.Sum() })
                    .SortByDescending(l => l.Count)
                    .Limit(1)
                    .ToListAsync();
            //Console.WriteLine("Debug: " + levelCounts.First());
            return levelCounts.First().Id;
        }



        public async Task<Player> Create(Player player)
        {
            await playersCollection.InsertOneAsync(player);
            Console.WriteLine("Created player with id: " + player.Id);
            return player;
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            Player p = await Get(id);

            if (p != null)
            {
                if (player.item != null)
                {
                    p.Items.Add(player.item);
                    Console.WriteLine("Added item: Level " + player.item.Level + " "  + player.item.ItemType.ToString() + " with id: " + player.item.Id + " to player: " + id);
                }
                if (player.Score != p.Score) // Problem if submitting empty body: updates score to 0 
                {
                    p.Score = player.Score;
                    Console.WriteLine("Changed players: " + id + " score to: " + player.Score);
                }

                FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
                await playersCollection.ReplaceOneAsync(filter, p);
                //Console.WriteLine("Added item " + player.item.ItemType.ToString() + " with id: " + player.item.Id + " to player: " + id);
                return await Get(id);
            }
            else
            {
                Console.WriteLine("Couldnt modify player: Player with id: " + id + " not found!");
                return null;
            }
        }

        public async Task<Player> UpdatePlayerName(Guid id, string newName)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var nameUpdate = Builders<Player>.Update.Set(p => p.Name, newName);
            var options = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };
            Player p = await playersCollection.FindOneAndUpdateAsync(filter, nameUpdate, options);
            return p;
        }

        public async  Task<Player> IncrementPlayerScore(Guid id, int increment)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var scoreIncrementUpdate = Builders<Player>.Update.Inc(p => p.Score, increment);
            var options = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };
            Player p = await playersCollection.FindOneAndUpdateAsync(filter, scoreIncrementUpdate, options);
            return p;
        }

        public async Task<Player> AddNewItem(Guid id, Item newItem)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var itemAddUpdate = Builders<Player>.Update.Push("Items", newItem);
            var options = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };
            Player p = await playersCollection.FindOneAndUpdateAsync(filter, itemAddUpdate, options);
            if(p != null)
            {
                Console.WriteLine("Successfully added new item Level " + newItem.Level + " " + newItem.ItemType.ToString() + " with id: " + newItem.Id + " to player: " + p.Name + "(" + p.Id + ")");
            }
            return p;
        }

        public async Task<Player> TradeItem(Guid id, Guid itemId)
        {
            var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            var itemTradeUpdate = Builders<Player>.Update.PullFilter(p => p.Items, Builders<Item>.Filter.Eq(i => i.Id, itemId)).Inc(p => p.Score, 7); // Adds 7 score for any item

            var options = new FindOneAndUpdateOptions<Player>()
            {
                ReturnDocument = ReturnDocument.After
            };

            Player p = await playersCollection.FindOneAndUpdateAsync(filter, itemTradeUpdate, options);
            if(p != null)
            {
                Console.WriteLine("Successfully traded item: " + itemId + " for 7 score on player: " + p.Name + "(" + p.Id + ")");
            }

            return p;
        }

        public async Task<Player> Delete(Guid id)
        {
            Player deletedPlayer = await Get(id);

            if (deletedPlayer != null)
            {
                FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
                await playersCollection.DeleteOneAsync(filter);
                Console.WriteLine("Deleted player with id: " + id);
                return deletedPlayer;
            }
            else
            {
                Console.WriteLine("Couldnt delete player: Player with id: " + id + " not found!");
            }

            return null;
        }

        public async Task<Item> CreateItem(Guid playerId, NewItem newItem)
        {
            ModifiedPlayer modP = new ModifiedPlayer();
            Player p = await Get(playerId);

            if (p == null)
            {
                throw new NotFoundException();
            }
            else
            {
                Item i = new Item(newItem);

                if (p != null)
                {
                    modP.Score = p.Score;
                    modP.item = i;
                }

                await Modify(playerId, modP);

                return i;
            }
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player p = await Get(playerId);
            if (p != null)
            {
                foreach (Item i in p.Items)
                {
                    if (i.Id == itemId)
                    {
                        Console.WriteLine("Returned players: " + playerId + " item: Level: " + i.Level + " " + i.ItemType.ToString() + " with id: " + i.Id);
                        return i;
                    }
                }
                Console.WriteLine("Couldnt get item: Player: " + playerId + " doesnt own item: " + itemId);
            }
            else
            {
                Console.WriteLine("Couldnt get item: Player with id: " + playerId + " doesnt own item: " + itemId);
            }

            return null;
        }

        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player p = await Get(playerId);
            List<Item> items = new List<Item>();
            if (p != null)
            {
                foreach (Item i in p.Items)
                {
                    items.Add(i);
                }
                return items.ToArray();
            }
            else
            {
                Console.WriteLine("Couldnt find player with id: " + playerId + " when trying to get all items");
            }

            return null;
        }

        public async Task<Item> UpdateItem(Guid playerId, ModifiedItem item)
        {
            ModifiedItem modItem = new ModifiedItem();
            Player p = await Get(playerId);
            Item i = new Item();
            i.Level = item.Level;
            bool itemFound = false;

            foreach (Item it in p.Items)
            {
                if (it.Id == item.Id)
                {
                    it.Level = item.Level;
                    i = it;
                    itemFound = true;
                }
            }

            if (!itemFound)
            {
                Console.WriteLine("Couldnt update item: Player: " + playerId + " doesnt own item " + item.Id);
            }

            if (p != null && itemFound)
            {
                FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
                await playersCollection.ReplaceOneAsync(filter, p);
                Console.WriteLine("Successfully modified players: " + playerId + " " + i.ItemType.ToString() + " level to: " + i.Level);
                return i;
            }
            
            return null;
        }

        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            Player p = await Get(playerId);
            Item i = new Item();
            bool itemFound = false;
            foreach (Item it in p.Items)
            {
                if (it.Id == item.Id)
                {
                    i = it;
                    p.Items.Remove(it);
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound)
            {
                Console.WriteLine("Couldnt delete item: Player: " + playerId + " doesnt own item " + item.Id);
            }

            if (p != null)
            {
                FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
                await playersCollection.ReplaceOneAsync(filter, p);
                Console.WriteLine("Successfully removed players: " + playerId + " item " + i.ItemType.ToString() + " with id: " + i.Id);
                return i;
            }

            Console.WriteLine("Couldnt find player: " + playerId + "when trying to delete item: " + item.Id);

            return null;
        }
    }
}