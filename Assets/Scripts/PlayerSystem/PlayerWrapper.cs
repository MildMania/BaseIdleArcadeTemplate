using System;
using MatchSystem;
using UnityEngine;


public class PlayerWrapper : MonoBehaviour
{
    private Player _player;

    public Player Player
    {
        get => _player;
        set
        {
            _player = value;
            OnPlayerSet?.Invoke();
        }
    }

    public Action OnPlayerSet { get; set; }

    private void Awake()
    {
        RegisterToManager();
    }

    private void OnDestroy()
    {
        UnregisterFromManager();
    }

    private void RegisterToManager()
    {
        _player = new Player(UserManager.Instance.LocalUser);
        PlayerWrapperManager.Instance.Register(this);
    }

    private void UnregisterFromManager()
    {
        PlayerWrapperManager.Instance.Unregister(this);
    }

    public void WaitUntilPlayerSet(Action callback)
    {
        if (Player != null)
            callback?.Invoke();

        void onPlayerSet()
        {
            OnPlayerSet -= onPlayerSet;

            callback?.Invoke();
        }

        if (callback != null)
            OnPlayerSet += onPlayerSet;
    }
}