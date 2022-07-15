using System;
using UnityEngine;

public class LevelStartedEvent : MMAnalyticsCustomEvent
{
    public int LevelID { get; private set; }

    public LevelStartedEvent(int levelID)
    {
        LevelID = levelID;
    }
}

public class LevelCompletedEvent : MMAnalyticsCustomEvent
{
    public int LevelID { get; private set; }

    public LevelCompletedEvent(
        int levelID)
    {
        LevelID = levelID;
    }
}

public class LevelFailedEvent : MMAnalyticsCustomEvent
{
    public int LevelID { get; private set; }

    public LevelFailedEvent(
        int levelID)
    {
        LevelID = levelID;
    }
}

public class SessionStartedEvent : MMAnalyticsCustomEvent
{
}

public class SessionEndedEvent : MMAnalyticsCustomEvent
{
    public float SessionDuration { get; private set; }

    public int SessionEndLevelID { get; private set; }

    public SessionEndedEvent(float duration, int endLevelID)
    {
        SessionDuration = duration;
        SessionEndLevelID = endLevelID;
    }
}

public class PlayerProgressionAnalyticsTracker : MMAnalyticsTrackerBase
{    
    private DateTime _sessionStartTime;

    public int LevelID { get; private set; }

    protected override void StartListeningEvents()
    {
        OnGameStarted();

        PhaseBaseNode.OnTraverseStarted_Static += OnPhaseStarted;

        base.StartListeningEvents();
    }

    protected override void FinishListeningEvents()
    {
        OnGameFinished();

        PhaseBaseNode.OnTraverseStarted_Static -= OnPhaseStarted;

        base.FinishListeningEvents();
    }

    private void OnGameStarted()
    {
        _sessionStartTime = DateTime.Now;

        AnalyticsManager.SendCustomEvent(
            new SessionStartedEvent());
    }

    private void OnGameFinished()
    {
        AnalyticsManager.SendCustomEvent(
            new SessionEndedEvent((float)(DateTime.Now - _sessionStartTime).TotalSeconds, LevelID));
    }

    private void OnPhaseStarted(PhaseBaseNode phaseNode)
    {
        if (phaseNode is LevelPhase)
            OnLevelStarted();
        else if (phaseNode is LevelWinPhase)
            OnLevelCompleted();
        else if (phaseNode is LevelFailPhase)
            OnLevelFailed();
    }

    private void OnLevelStarted()
    {
        LevelID = GameManager.Instance.GetVirtualLevelID();

        Logger.Log("Level Started: " + LevelID);

        AnalyticsManager.SendCustomEvent(
           new LevelStartedEvent(LevelID));
    }

    private void OnLevelCompleted()
    {
        Logger.Log("Level Completed: " + LevelID);

        AnalyticsManager.SendCustomEvent(
            new LevelCompletedEvent(LevelID));
    }
    
    private void OnLevelFailed()
    {
        Logger.Log("Level Failed: " + LevelID);

        AnalyticsManager.SendCustomEvent(
            new LevelFailedEvent(LevelID));
    }
}