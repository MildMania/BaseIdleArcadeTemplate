using System;
using System.Collections.Generic;
using MMFramework.TasksV2;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private MMTaskExecutor _onCollidedTasks;

    public Collider Collider;
    public bool IsCollided { get; private set; }
    public Action<Character> OnCollided;

    public Rigidbody CharacterRb { get; private set; }
    
    void Awake()
    {
        CharacterRb = gameObject.GetComponent<Rigidbody>();
    }
    
    public bool TryCollide()
    {

        _onCollidedTasks?.Execute(this);

        OnCollided?.Invoke(this);

        return true;
    }

}