﻿public class FolderConsumptionController : ConsumptionController<FolderConsumer,Folder>
{
    void Awake()
    {
        if (_consumer.IsAiInteractible())
        {
            ConsumerProvider.Instance.AddConsumer(_consumer, typeof(Folder));
        }
    }
}