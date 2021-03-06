using MMFramework.TrackerSystem;
using System;

public class UserTrackableData<TTracker, TTrackable, TTrackData, TID> : UserData
    where TTracker : TrackerBase<TTrackable, TTrackData, TID>
    where TTrackable : ITrackable<TTrackData, TID>
    where TTrackData : TrackData<TID>
{
    public TTracker Tracker { get; private set; }

    public UserTrackableData(
        TrackerIOBase<TTrackData, TID> trackerIO)
    {
        Tracker = (TTracker)Activator.CreateInstance(typeof(TTracker), trackerIO);
    }

    public override void LoadData(Action onLoadedCallback)
    {
        void onLoaded()
        {
            DataLoaded();

            onLoadedCallback?.Invoke();
        }

        Tracker.Read(onLoaded);
    }

    public override void SaveData(Action onSavedCallback)
    {
        Tracker.Write(onSavedCallback);
    }
}
