using UnityEngine;
using UnityEngine.AI;

public class WanderState : CamperState
{
    private Vector3 randomPos;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private float waitDuration = 0f;
    public WanderState(Camper camper, CamperStateMachine camperStateMachine) : base(camper, camperStateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();

        GenerateNewPoint(camper.wanderRange);
        camper.GoTo(randomPos);
    }

    public override void Execute()
    {
        base.Execute();

        if (isWaiting)
        {
            if (Time.time >= waitTimer + waitDuration)
            {
                isWaiting = false;
                GenerateNewPoint(camper.wanderRange);
                camper.GoTo(randomPos);
            }
            return;
        }

        // if Camper reaches destination start waiting
        if (Vector3.Distance(camper.transform.position, randomPos) < 0.5f)
        {
            isWaiting = true;
            waitDuration = Random.Range(1f, 4f); // wait between 1–4 seconds
            waitTimer = Time.time;
            camper.StopMoving();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void GenerateNewPoint(float range)
    {
        for (int i = 0; i < 10; i++)
        {
            // Pick a random direction within a circle
            Vector2 offset = Random.insideUnitCircle * range;
            Vector3 candidate = camper.transform.position + new Vector3(offset.x, offset.y, 0f);

            // Check if the point is not blocked by obstacles
            if (!Physics2D.OverlapCircle(candidate, 0.3f, camper.obstacleMask))
            {
                // To be replaced once we add multiple 'viewports'
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(candidate);
                if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                {
                    randomPos = candidate;
                    return;
                }
            }
        }

        // Fallback if all attempts fail
        randomPos = camper.transform.position;
    }
}
