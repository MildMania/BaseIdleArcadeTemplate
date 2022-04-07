using System;
using System.Collections.Generic;

namespace MatchSystem
{
    public class PlayerStateTracker
    {
        private List<Player> _players = new List<Player>();

        public void RegisterPlayer(Player player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

        }
        
        public void UnregisterPlayer(Player player)
        {
            _players.Remove(player);


        }
        
    }
}