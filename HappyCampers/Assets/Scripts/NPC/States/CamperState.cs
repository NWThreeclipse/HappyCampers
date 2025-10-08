using UnityEngine;

public class CamperState
{
    protected Camper camper;
    protected CamperStateMachine stateMachine;

    public CamperState(Camper camper, CamperStateMachine stateMachine)
    {
        this.camper = camper;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
