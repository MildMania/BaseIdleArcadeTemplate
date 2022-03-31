using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;

using Pathfinding;

public class AIHelperDeliverState : State<EState, ETransition>
{
    [SerializeField] private AIHelper _aiHelper;

    [SerializeField] private AIMovementBehaviour _movementBehaviour;
    private BaseConsumer _currentConsumer;
    [SerializeField] private HelperAnimationController _helperAnimationController;
    protected override EState GetStateID()
    {
        return EState.Deliver;
    }

    private BaseConsumer SelectConsumer()
    {
        BaseConsumer currentConsumer = default(BaseConsumer);

        var list = GetConsumers();
        int indx = Random.Range(0, list.Count - 1);

        currentConsumer = list[indx];

        return currentConsumer;
    }

    protected List<BaseConsumer> GetConsumers()
    {
        return ConsumerProvider.Instance.GetConsumers(_aiHelper.Resource.GetType());
    }

    public override void OnEnterCustomActions()
    {
        base.OnEnterCustomActions();

        _currentConsumer = SelectConsumer();
        MoveToInteractionPoint(_currentConsumer.AiInteraction.GetInteractionPoint());
        _helperAnimationController.PlayAnimation(EHelperAnimation.Walk);
        _aiHelper.CurrentLoadBehaviour.OnCapacityEmpty += OnCapacityEmpty;
    }

    protected override void OnExitCustomActions()
    {
        _aiHelper.CurrentLoadBehaviour.OnCapacityEmpty -= OnCapacityEmpty;
    }

    private void MoveToInteractionPoint(Vector3 pos)
    {
        _movementBehaviour.MoveDestination(pos, OnPathCompleted);
    }

    private void OnPathCompleted()
    {
        
    }

    private void OnCapacityEmpty()
    {
        FSM.SetTransition(AIHelperFSMController.ETransition.Store);
    }
}
