using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieFSM : AbstractStateMachine<ZombieState>
{
    public Rigidbody RB { get; private set; }
    public Animator ZombieAnimator { get; private set; }

    public bool m_isPreyInSight = false;
    public bool m_isPreyInReach = false;

    public Vector3 m_preyPosition = Vector3.zero;
    public Vector3 m_newDirection = Vector3.zero;
    private float m_chasingSpeed = 2f;

    public Collider outerCollider;
    public Collider innerCollider;

    public NavMeshAgent m_agent;

    [SerializeField]
    public float patrolRange = 30f;

    protected override void Start()
    {
        RB = GetComponent<Rigidbody>();
        m_agent = GetComponent<NavMeshAgent>();

        if (RB == null) Debug.LogError("RB is null");
        //ZombieAnimator = GetComponentInChildren<Animator>();
        //if (ZombieAnimator == null) Debug.LogError("ZombieAnimator is null");

        foreach (ZombieState state in m_possibleStates)
        {
            state.OnStart(this);//tem que passar um state
        }
 
        base.Start();
        m_currentState = m_possibleStates[0];//roaming state in this case
        m_currentState.OnEnter();
    }


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<ZombieState>();
        //we start with the roaming state
        m_possibleStates.Add(new RoamingState());
        m_possibleStates.Add(new ChasingState());
        m_possibleStates.Add(new ZombieAttackState());
        //m_possibleStates.Add(new ZombieRunawayState());
    }

    /*
    public void GoToDirection(Vector3 position)
    {
        var direction = position - RB.transform.position;
        // Rotate horizontally towards the prey
        var rotation = Quaternion.LookRotation(direction);
        Quaternion horizintalRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        RB.transform.rotation = Quaternion.Slerp(RB.transform.rotation, horizintalRotation, Time.deltaTime * 2.0f);

        // Advance towards the prey
        var vectorOnFloor = new Vector3(direction.x, 0, direction.z);
        RB.AddForce(vectorOnFloor * m_chasingSpeed, ForceMode.Acceleration);

        if (direction.magnitude < 0.01f)
        {
            Debug.Log("At position.");
            m_isPreyInSight = false;
            m_preyPosition = Vector3.zero;
        }
    }
    */

}
