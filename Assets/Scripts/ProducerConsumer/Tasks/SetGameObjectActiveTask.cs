using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MMFramework.TasksV2;

public class SetGameObjectActiveTask : MMTask
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private bool _value;

    public override ETaskStatus Execute(MonoBehaviour caller)
    {
        _gameObject.SetActive(_value);
        return ETaskStatus.Completed;
    }
}
