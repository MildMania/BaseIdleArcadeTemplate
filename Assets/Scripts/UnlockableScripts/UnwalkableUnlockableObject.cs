using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

public class UnwalkableUnlockableObject : UnlockableObject
{
    [OdinSerialize] private List<GameObject> _colliderObjects;
    
    protected override void OnStartCustomActions()
    {
        base.OnStartCustomActions();

        if (_unlockableTrackData.IsUnlock)
        {
            UnlockDoors();
        }
        else
        {
            LockDoors();
        }
        
        ScanAreaForDoors();
        
    }

    private void ScanAreaForDoors()
    {
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }

    private void LockDoors()
    {
        foreach (var doorObject in _colliderObjects)
        {
            doorObject.transform.SetLayer(LayerMask.NameToLayer("Wall"));

            doorObject.GetComponent<Collider>().isTrigger = false;
        }
    }
    
    private void UnlockDoors()
    {
        foreach (var doorObject in _colliderObjects)
        {
            doorObject.transform.SetLayer(LayerMask.NameToLayer("Default"));

            doorObject.GetComponent<Collider>().isTrigger = true;
            
        }
    }

    protected override void OnDetectedCustomActions()
    {
        base.OnDetectedCustomActions();
        UnlockDoors();
        ScanAreaForDoors();
    }
}
