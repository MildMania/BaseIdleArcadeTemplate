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
            SetWalkable();
        }
        else
        {
            SetUnwalkable();
        }
        
        ScanArea();
        
    }

    private void ScanArea()
    {
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }

    private void SetUnwalkable()
    {
        foreach (var doorObject in _colliderObjects)
        {
            doorObject.transform.SetLayer(LayerMask.NameToLayer("Wall"));

            doorObject.GetComponent<Collider>().isTrigger = false;
        }
    }
    
    private void SetWalkable()
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
        SetWalkable();
        ScanArea();
    }
}
