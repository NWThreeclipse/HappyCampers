using UnityEngine;

public class FollowState : CamperState
{
    private FollowManager followManager;
    public FollowState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {
        followManager = camper.player.GetComponent<FollowManager>();
    }
    public override void Enter()
    {
        //Not needed as base class has no logic
        // base.Enter();
        Debug.Log("Entering Wander State");

        if (camper.player == null)
        {
            camper.StateMachine.ChangeState(camper.Wander);
            return;
        }

        if (followManager != null)
            camper.followIndex = followManager.StartFollow(camper);
    }

    public override void Execute()
    {
        //Not needed as base class has no logic
        // base.Execute();

        // Gets the assigned trail point
        int index = Mathf.Clamp(camper.followIndex, 0, followManager.trailPoints.Count - 1);
        Vector3 targetPos = followManager.trailPoints[index];

        camper.GoTo(targetPos);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Wander State");
        //Not needed as base class has no logic
        // base.Exit();
        
        if (followManager != null)
            followManager.StopFollow(camper);
    }
}
