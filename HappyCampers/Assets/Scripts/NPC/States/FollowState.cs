using UnityEngine;

public class FollowState : CamperState
{
    public FollowState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();

        if (camper.player == null)
            camper.StateMachine.ChangeState(camper.Wander);
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 directionToPlayer = (camper.player.position - camper.transform.position).normalized;
        Vector3 targetPosition = camper.player.position - directionToPlayer * 1.5f;

        // Camper follows until they are next to player
        if ((camper.transform.position - camper.player.position).sqrMagnitude > 1f)
        {
            camper.GoTo(targetPosition);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
