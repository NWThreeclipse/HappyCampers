using UnityEngine;

public class IdleState : CamperState
{
    public IdleState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {

    }
    public override void Enter()
    {
        // base.Enter();
        Debug.Log("Entering Idle State");
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting Idle State");
    }
}
