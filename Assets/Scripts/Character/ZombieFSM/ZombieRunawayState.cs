using UnityEngine;

public class ZombieRunawayState : ZombieState
{
    public override void OnStart() { Debug.Log("Idle State State"); }

    //TODO: create condition isPlayerAttacking to use in canEnter/canExit here
    public override bool CanEnter(IState currentState){  return false; }

    public override void OnEnter(){ Debug.Log("Entering ZombieRunaway State"); }

    public override bool CanExit() { return true; }

    public override void OnExit() {  Debug.Log("Exiting ZombieRunaway State");  }

    public override void OnUpdate(){ Debug.Log("Idle ZombieRunaway OnUpdate"); }

    public override void OnFixedUpdate() { Debug.Log("ZombieRunaway State OnFixedUpdate"); }

}
