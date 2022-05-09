using UnityEngine;

public class CharacterStateRequirement : BaseRequirement
{
    [SerializeField] private CharacterMovementFSM _characterMovementFsm;
    [SerializeField] private CharacterFSMController.EState _targetState;

    public override bool IsRequirementMet()
    {
        return _characterMovementFsm.GetCurStateID() == _targetState;
    }
}