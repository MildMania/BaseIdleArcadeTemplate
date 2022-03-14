using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorableStoreHandler : MonoBehaviour
{
    [SerializeField] private StorableController _storableController;
    [SerializeField] private StorableFormationController _storableFormationController;
    
    [SerializeField] private StoreCommandBase _storeCommand;

    private StorableBase _storable;

    protected void Store(StorableBase storable)
    {
        CreateStoreCommand();
        
        storable.OnStored += OnStored;

        StoreCommandBase storeCommandClone = CreateStoreCommand();
        storable.Store(storeCommandClone);
    }

    private void OnStored(StorableBase storable)
    {
        Debug.Log(storable.gameObject.name + " OBJECT STORED!");
        _storableFormationController.Reformat();
    }

    private void OnDestroy()
    {
        _storable.OnStored -= OnStored;
    }


    public StoreCommandBase CreateStoreCommand()
    {
        StoreCommandBase storeCommand = Instantiate(_storeCommand);
        storeCommand.StorableList = _storableController.StorableList;
        storeCommand.ParentTransform = _storableController.StorableContainer;
        storeCommand.TargetTransforms = _storableFormationController.TargetTransforms;
        
        return storeCommand;
    }
}