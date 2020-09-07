using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2
{
    public class Game<T> where T : IPlayer
    {
        private List<T> _players;

        public Game(List<T> players)
        {
            _players = players;
        }

        public T[] GetTop10Players()
        {
            T[] top10 = new T[10];
            //Item[] items = new Item[p.Items.Count()];
            _players = _players.OrderByDescending(x => x.Score).ToList();

            for(int i = 0; i < 10; i++)
            {
                top10[i] = _players[i];
            }
            return top10;
        }
    }
}
