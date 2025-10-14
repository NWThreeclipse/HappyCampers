using UnityEngine;

public abstract class CamperState
{
    protected Camper camper;
    protected CamperStateMachine stateMachine;

    public CamperState(Camper camper, CamperStateMachine stateMachine)
    {
        this.camper = camper;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { Debug.Log($"Entering : {stateMachine.CurrentCamperState}");}
    public virtual void Execute() { }
    public virtual void Exit(){Debug.Log($"Exiting : {stateMachine.CurrentCamperState}");}
}
