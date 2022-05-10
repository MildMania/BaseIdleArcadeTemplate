using System;
using System.Collections.Generic;
using UnityEngine;

public class Deliverer : MonoBehaviour
{
    [SerializeField] private UpdatedFormationController _updatedFormationController;
    [SerializeField] public Transform Container;

    public Action<bool> OnContainerEmpty; 

    public UpdatedFormationController FormationController
    {
        get => _updatedFormationController;
        set => _updatedFormationController = value;
    }

    public List<BaseResource> Resources { get; set; } = new List<BaseResource>();

    public int GetLastResourceIndex<TResource>()
    {
        for (int i = Resources.Count - 1; i >= 0; i--)
        {
            if (Resources[i] is TResource)
            {
                return i;
            }
        }

        return -1;
    }
}