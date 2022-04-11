using MMFramework.TrackerSystem;

public class UpgradableTrackable : ITrackable<UpgradableTrackData, EUpgradable>
{
	public UpgradableTrackData TrackData { get; private set; }

	public UpgradableTrackable(
		UpgradableTrackData data)
	{
		TrackData = data;
	}

	public void UpdateData(UpgradableTrackData data)
	{
		TrackData.TrackID = data.TrackID;
		TrackData.Level = data.Level;
	}
}

public class UpgradableTracker : TrackerBase<UpgradableTrackable, UpgradableTrackData, EUpgradable>
{
	public UpgradableTracker(
		TrackerIOBase<UpgradableTrackData, EUpgradable> trackerIO) 
		: base(trackerIO)
	{
	}
	
}