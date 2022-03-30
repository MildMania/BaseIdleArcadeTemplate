using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomUpgradeSwitchHandler : ClassroomUpgradeHandler
{
    public override void UpgradeByLevel(int level)
    {
        foreach (var levelObject in _levelObjects)
        {
            foreach (var objectList in levelObject.Value)
            {
                objectList.SetActive(levelObject.Key == level);
            }
        }
    }
}
