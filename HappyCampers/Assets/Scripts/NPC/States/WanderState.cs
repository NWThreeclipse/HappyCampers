using UnityEngine;

public class WanderState : CamperState
{
    private Vector3 randomPos;
    private Vector3 moveDir;
    public WanderState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {

    }
    public override void Enter()
    {
        //Not needed as base class has no logic
        // base.Enter();
        Debug.Log("Entering Wander State");

        GenerateNewPoint(camper.wanderRange);
    }

    public override void Execute()
    {
        //Not needed as base class has no logic
        // base.Execute();

        //Calculates direction to random position
        moveDir = (randomPos - camper.transform.position).normalized;

        camper.Move(moveDir * camper.wanderSpeed);

        // Generates a new point if camper reaches random pos
        if ((camper.transform.position - randomPos).sqrMagnitude < 0.01f)
            GenerateNewPoint(camper.wanderRange);
    }

    public override void Exit()
    {
        Debug.Log("Exiting Wander State");

        //Not needed as base class has no logic
        // base.Exit();
    }

    private void GenerateNewPoint(float range)
    {
        // Generates a random position within range
        randomPos = camper.transform.position + (Vector3)Random.insideUnitCircle * range;
    }
}
