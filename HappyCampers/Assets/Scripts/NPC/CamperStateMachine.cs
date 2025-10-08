using UnityEngine;

public class CamperStateMachine
{
    public CamperState CurrentCamperState {  get; private set; }

    public void Initialize(CamperState state)
    {
        CurrentCamperState = state;
        CurrentCamperState.Enter();

        Debug.Log("Camper initialized in " +  CurrentCamperState);
    }

    public void ChangeState(CamperState state)
    {
        CurrentCamperState.Exit();
        CurrentCamperState = state;
        CurrentCamperState.Enter();

        Debug.Log("New State: " + CurrentCamperState);
    }
}
