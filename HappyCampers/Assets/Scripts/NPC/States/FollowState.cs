using UnityEngine;

public class FollowState : CamperState
{
    private Vector3 moveDir;
    public FollowState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {

    }
    public override void Enter()
    {
        //Not needed as base class has no logic
        // base.Enter();
        Debug.Log("Entering Wander State");

        if (camper.player == null)
            camper.StateMachine.ChangeState(camper.Wander);
    }

    public override void Execute()
    {
        //Not needed as base class has no logic
        // base.Execute();

        // Camper follows until they are next to player
        if ((camper.transform.position - camper.player.position).sqrMagnitude > 1f)
        {
            // Calculates direction to player
            moveDir = (camper.player.position - camper.transform.position).normalized;

            camper.Move(moveDir * camper.followSpeed);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Wander State");
        //Not needed as base class has no logic
        // base.Exit();
    }
}
