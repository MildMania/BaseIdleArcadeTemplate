using System;
using UnityEngine;

public abstract class BaseRequirement : MonoBehaviour
{
    public abstract bool IsRequirementMet();

    public virtual void ExecuteRequirement(Action onRequirementExecuted)
    {
    }
}