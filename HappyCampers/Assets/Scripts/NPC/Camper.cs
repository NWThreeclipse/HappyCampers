using UnityEngine;
using UnityEngine.AI;

public class Camper : MonoBehaviour
{
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    public CamperStateMachine StateMachine {  get; private set; }
    public IdleState Idle {  get; private set; }
    public WanderState Wander {  get; private set; }
    public FollowState Follow {  get; private set; }

    [Header("Wander Variables")]
    public float wanderRange = 2f;
    public float wanderSpeed = 2f;
    public LayerMask obstacleMask;

    [Header("Follow Variables")]
    public Transform player;
    public float followSpeed = 2f;
    private void Awake()
    {
        StateMachine = new CamperStateMachine();

        //Initialize states
        Idle = new IdleState(this, StateMachine);
        Wander = new WanderState(this, StateMachine);
        Follow = new FollowState(this, StateMachine);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb = GetComponent<Rigidbody2D>();


        StateMachine.Initialize(Idle);
    }

    private void Update()
    {
        StateMachine.CurrentCamperState.Execute();

        //Debug Keys
        if (Input.GetKeyDown(KeyCode.Alpha8))   // Press 1 to make Camper idle
            StateMachine.ChangeState(Idle);
        if (Input.GetKeyDown(KeyCode.Alpha9))   // Press 2 to make Camper wander
            StateMachine.ChangeState(Wander);
        if (Input.GetKeyDown(KeyCode.Alpha0))   // Press 3 to make Camper follow
            StateMachine.ChangeState(Follow);
    }

    public void GoTo(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }
}
