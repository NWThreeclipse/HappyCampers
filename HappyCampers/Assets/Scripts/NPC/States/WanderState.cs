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
        base.Enter();

        GenerateNewPoint(camper.wanderRange);
    }

    public override void Execute()
    {
        base.Execute();

        //Calculates direction to random position
        moveDir = (randomPos - camper.transform.position).normalized;

        camper.Move(moveDir * camper.wanderSpeed);

        // Generates a new point if camper reaches random pos
        if ((camper.transform.position - randomPos).sqrMagnitude < 0.01f)
            GenerateNewPoint(camper.wanderRange);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void GenerateNewPoint(float range)
    {
        // Generates a random position within range
        randomPos = camper.transform.position + (Vector3)Random.insideUnitCircle * range;
    }
}
