using System;
using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;
using MMFramework_2._0.PhaseSystem.Core.EventListener;
using MMFramework.Utilities;
using UnityEngine;

public class AIHelperIdleState : State<EState, ETransition>
{
    [SerializeField] private float _delay;
    [SerializeField] private HelperAnimationController _helperAnimationController;
    protected override EState GetStateID()
    {
        return EState.Idle;
    }

    public override void OnEnterCustomActions()
    {
        base.OnEnterCustomActions();

        _helperAnimationController.PlayAnimation(EHelperAnimation.Idle);
    }

    private void Awake()
    {
        InitAiHelper();
    }

    private void InitAiHelper()
    {
        CoroutineRunner.Instance.WaitForSeconds(_delay, () =>
        {
            FSM.SetTransition(ETransition.Load);
        });
    }
}