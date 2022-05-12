using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EState = AIHelperFSMController.EState;
using ETransition = AIHelperFSMController.ETransition;
using Random = UnityEngine.Random;
using Pathfinding.RVO;
using DG.Tweening;

public class AIHelperUnloadState : State<EState, ETransition>
{
    [SerializeField] private AIHelper _aiHelper;
    [SerializeField] private AIMovementBehaviour _movementBehaviour;
    [SerializeField] private HelperAnimationController _helperAnimationController;
    [SerializeField] private float _pollDelay = 5;
    [SerializeField] private HelperAnimationController _animationController;

    private BaseConsumer _currentConsumer;
    private WaitForSeconds _pollWfs;

    private void Awake()
    {
        _pollWfs = new WaitForSeconds(_pollDelay);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected override EState GetStateID()
    {
        return EState.Unload;
    }


    private BaseConsumer SelectConsumer()
    {
        var list = GetConsumers();
        if (list == null || list.Count == 0)
        {
            // FSM.SetTransition(ETransition.Trash);
            return null;
        }

        int index = Random.Range(0, list.Count);
        var currentConsumer = list[index];

        // _aiHelper.ReserveConsumer(currentConsumer);

        return currentConsumer;
    }

    protected List<BaseConsumer> GetConsumers()
    {
        return ConsumerProvider.Instance.GetAvailableConsumers(_aiHelper.Resource.GetType());
    }

    public override void OnEnterCustomActions()
    {
        StartCoroutine(SelectConsumerRoutine());
    }

    protected override void OnExitCustomActions()
    {
        // _rvoController.locked = false;

        _aiHelper.CurrentUnloadBehaviour.OnCapacityEmpty -= OnCapacityEmpty;
        _aiHelper.CurrentUnloadBehaviour.OnConsumerCapacityFull -= OnConsumerCapacityFull;
        _aiHelper.CurrentUnloadBehaviour.Deactivate();

        StopAllCoroutines();
    }

    private IEnumerator SelectConsumerRoutine()
    {
        _currentConsumer = SelectConsumer();

        while (_currentConsumer == null)
        {
            yield return _pollWfs;
            _currentConsumer = SelectConsumer();
        }

        MoveToDeliveryPoint();
    }

    private void MoveToDeliveryPoint()
    {
        MoveToInteractionPoint(_currentConsumer.AiInteraction.GetInteractionPoint());
        _aiHelper.CurrentUnloadBehaviour.OnCapacityEmpty += OnCapacityEmpty;
        _aiHelper.CurrentUnloadBehaviour.OnConsumerCapacityFull += OnConsumerCapacityFull;
    }

    private void OnConsumerCapacityFull()
    {
        // List<BaseConsumer> availableConsumers;
        // availableConsumers = ConsumerProvider.Instance.GetAvailableConsumers(_aiHelper.Resource.GetType());

        // if (availableConsumers == null || availableConsumers.Count == 0)
        // {
        //     FSM.SetTransition(ETransition.Trash);
        // }
        // else
        // {
        FSM.SetTransition(ETransition.Unload);
        // }
    }


    private void MoveToInteractionPoint(Vector3 pos)
    {
        _helperAnimationController.PlayAnimation(EHelperAnimation.Walk);
        _movementBehaviour.MoveDestination(pos, OnPathCompleted, OnPathStucked);
    }

    private void OnPathCompleted()
    {
        _helperAnimationController.PlayAnimation(EHelperAnimation.Idle);

        Vector3 rotTarget = _currentConsumer.AiInteraction.RotationTarget.position;
        Vector3 dir = (new Vector3(rotTarget.x, _aiHelper.transform.position.y, rotTarget.z) -
                       _aiHelper.transform.position).normalized;

        _aiHelper.transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.1f).OnComplete(() =>
        {
            _aiHelper.CurrentUnloadBehaviour.Activate();
        });
        // TODO: we need coroutine rather than path completed event
    }

    private void OnCapacityEmpty()
    {
        _aiHelper.CurrentUnloadBehaviour.Deactivate();
        FSM.SetTransition(ETransition.Load);
    }

    private void OnPathStucked()
    {
        _movementBehaviour.Stop();
        FSM.SetTransition(ETransition.Unload);
    }
}