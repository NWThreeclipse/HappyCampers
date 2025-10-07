using UnityEngine;

public class Camper : MonoBehaviour
{
    private Rigidbody2D rb;

    public CamperStateMachine StateMachine {  get; private set; }
    public IdleState Idle {  get; private set; }
    public WanderState Wander {  get; private set; }
    public FollowState Follow {  get; private set; }

    [Header("Wander Variables")]
    public float wanderRange = 2f;
    public float wanderSpeed = 2f;

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
        rb = GetComponent<Rigidbody2D>();


        StateMachine.Initialize(Idle);
    }

    private void Update()
    {
        StateMachine.CurrentCamperState.Execute();

        //Debug Keys
        if (Input.GetKeyDown(KeyCode.Alpha1))   // Press 1 to make Camper idle
            this.StateMachine.ChangeState(Idle);
        if (Input.GetKeyDown(KeyCode.Alpha2))   // Press 2 to make Camper wander
            this.StateMachine.ChangeState(Wander);
        if (Input.GetKeyDown(KeyCode.Alpha3))   // Press 3 to make Camper follow
            this.StateMachine.ChangeState(Follow);
    }

    public void Move(Vector2 moveDir)
    {
        rb.linearVelocity = moveDir;
    }
}
