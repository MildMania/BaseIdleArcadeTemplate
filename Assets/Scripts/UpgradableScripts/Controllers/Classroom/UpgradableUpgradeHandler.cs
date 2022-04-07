using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class UpgradableUpgradeHandler : SerializedMonoBehaviour
{
    [OdinSerialize] protected Dictionary<int, List<GameObject>> _levelObjects;
    
    public abstract void UpgradeByLevel(int level);

    
}
