using System;

public class UnlockableUpdater
{
	private IUnlockableDataProvider _unlockableDataProvider;

	public Action<Guid, int> OnUnlockableUpdated { get; set; }
    
	public UnlockableUpdater(
		IUnlockableDataProvider unlockableDataProvider)
	{
		_unlockableDataProvider = unlockableDataProvider;
	}

	public void UpdateUnlockable(Guid unlockableID, int currentCount , bool isUnlock)
	{
		_unlockableDataProvider.SetUnlockable(unlockableID, 
			currentCount,isUnlock);

		OnUnlockableUpdated?.Invoke(unlockableID, _unlockableDataProvider.GetUnlockable(unlockableID));
	}

	public int GetUnlockable(Guid unlockableID)
	{
		return _unlockableDataProvider.GetUnlockable(unlockableID);
	}
}