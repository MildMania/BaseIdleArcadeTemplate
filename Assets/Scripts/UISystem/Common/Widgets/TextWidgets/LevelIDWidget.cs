using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class LevelIDWidget : TextWidget
{
    [SerializeField] private string _preFix = "level ";
    
    protected override void ActivatingCustomActions()
    {
        TargetText = _preFix + GameManager.Instance.GetVirtualLevelID().ToString();
        
        base.ActivatingCustomActions();
    }
}
