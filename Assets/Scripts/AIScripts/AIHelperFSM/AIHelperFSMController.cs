public class AIHelperFSMController : FSMController<AIHelperFSMController.EState,
    AIHelperFSMController.ETransition>
{
    public enum EState
    {
        None = 0,
        Idle = 1,
        Load = 2,
        Unload = 3
    }

    public enum ETransition
    {
        None = 0,
        Idle = 1,
        Load = 2,
        Unload = 3
    }
}