using System.Collections.Generic;
using UnityEngine;
using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;
using ST = StateTransition<AIHelperFSMController.EState, AIHelperFSMController.ETransition>;

public class AIHelperFSM : MMFSM<EState, ETransition>
{
    [SerializeField] private EState _initialState = EState.Idle;

    protected override EState GetEnteranceStateID()
    {
        return _initialState;
    }

    protected override Dictionary<ST, EState> GetTransitionDict()
    {
        return new Dictionary<ST, EState>
        {
            { new ST(EState.Idle, ETransition.Load), EState.Load},
            { new ST(EState.Idle, ETransition.Unload), EState.Unload},
            { new ST(EState.Load, ETransition.Unload), EState.Unload},
            { new ST(EState.Unload, ETransition.Load), EState.Load},
        };
    }
}