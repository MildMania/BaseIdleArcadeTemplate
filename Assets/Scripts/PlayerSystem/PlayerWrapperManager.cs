using MatchSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWrapperManager : MonoBehaviour
{
    private static PlayerWrapperManager _instance;
    public static PlayerWrapperManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerWrapperManager>();


            return _instance;
        }
    }
    
    private PlayerStateTracker _playerStateTracker;

    public PlayerStateTracker PlayerStateTracker
    {
        get
        {
            if (_playerStateTracker == null)
                _playerStateTracker = new PlayerStateTracker();

            return _playerStateTracker;
        }
    }

    public List<PlayerWrapper> PlayerWrappers { get; private set; }
        = new List<PlayerWrapper>();

    public void Register(PlayerWrapper pw)
    {
        if (PlayerWrappers.Contains(pw))
            return;

        PlayerWrappers.Add(pw);
        
        void onPlayerSet()
        {
            PlayerStateTracker.RegisterPlayer(pw.Player);
        }

        pw.WaitUntilPlayerSet(onPlayerSet);
    }

    public void Unregister(PlayerWrapper pw)
    {
        PlayerWrappers.Remove(pw);
        
        PlayerStateTracker.UnregisterPlayer(pw.Player);
        
    }
    
    public bool TryGetLocalPlayerWrapper(out PlayerWrapper playerWrapper)
    {
        playerWrapper = PlayerWrappers.FirstOrDefault(val => true);

        if (playerWrapper == null)
            return false;

        return true;
    }


    public bool TryGetPlayerWrapper(Player player, out PlayerWrapper playerWrapper)
    {
        playerWrapper = PlayerWrappers.FirstOrDefault(val => val.Player == player);

        if (playerWrapper == null)
            return false;

        return true;
    }
}
