using UnityEngine;

public class PaperUnloadBehaviour : BaseUnloadBehaviour<PaperConsumer, Paper>
{
    [SerializeField] private PaperConsumerFovController _paperConsumerFovController;


    private void Awake()
    {
        _paperConsumerFovController.OnTargetEnteredFieldOfView += OnConsumerEnteredFieldOfView;
        _paperConsumerFovController.OnTargetExitedFieldOfView += OnConsumerExitedFieldOfView;
    }


    private void OnDestroy()
    {
        _paperConsumerFovController.OnTargetEnteredFieldOfView -= OnConsumerEnteredFieldOfView;
        _paperConsumerFovController.OnTargetExitedFieldOfView -= OnConsumerExitedFieldOfView;


        StopAllCoroutines();
    }

    public override void UnloadCustomActions(int index)
    {
        //Remove from self
        int lastResourceIndex = _deliverer.GetLastResourceIndex<Paper>();

        if (lastResourceIndex == -1)
        {
            return;
        }
        Paper paper = (Paper) _deliverer.Resources[lastResourceIndex];
        _deliverer.Resources.Remove(paper);
        _updatedFormationController.RemoveCustomResourceTransform(lastResourceIndex);
        
        //Add To Consumer
        PaperConsumer paperConsumer = _consumers[index];
        UpdatedFormationController consumerFormationController =
            paperConsumer.GetComponentInChildren<UpdatedFormationController>();
        Transform targetTransform = consumerFormationController.GetLastTargetTransform(paper.transform);
        paper.Move(targetTransform, consumerFormationController.Container);
        paperConsumer.ResourceProvider.Resources.Add(paper);
    }
}