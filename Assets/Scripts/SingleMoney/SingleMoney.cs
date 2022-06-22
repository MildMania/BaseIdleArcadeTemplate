using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMoney : BaseResource
{
    public void DestroyItself()
    {
        GoPoolObject.Push();
    }
}