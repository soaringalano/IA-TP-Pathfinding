using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterControllerStateMachine : AbstractStateMachine<CharacterState>, IDamageable
{
    //animator vars
    public const string KEY_STATUS_BOOL_TOUCHGROUND = "TouchGround";
    public const string KEY_STATUS_FLOAT_MOVEX = "MoveX";
    public const string KEY_STATUS_FLOAT_MOVEY = "MoveY";

    public const float TO_RADIAN = Mathf.PI / 180;

    [SerializeField]
    public float m_stunHeight = 10.0f;

    public bool AcceptInput { get; private set; } = false;

    public Camera Camera { get; private set; }

    [field: SerializeField]
    public Rigidbody RB { get; private set; }

    [field: SerializeField]
    public Animator Animator { get; set; }

    [field: SerializeField]
    public float AccelerationValue { get; private set; }

    public Vector2 CurrentRelativeVelocity { get; private set; }

    public Vector2 CurrentDirectionalInputs { get; private set; }

    [field: SerializeField]
    public float DecelerationValue { get; private set; } = 0.3f;

    [field: SerializeField]
    public float MaxVelocity { get; private set; }

    [field: SerializeField]
    public float MaxForwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxBackwardVelocity { get; private set; }

    [field: SerializeField]
    public float MaxSideVelocity { get; private set; }

    [field: SerializeField]
    public float InAirAccelerationValue { get; private set; } = 0.2f;

    [field: SerializeField]
    public float JumpIntensity { get; private set; } = 500.0f;

    [field: SerializeField]
    public float CommAttackDamage { get; set; } = 10.0f;

    [SerializeField]
    private CharacterFloorTrigger m_floorTrigger;

    [field: SerializeField]
    private Vector2 m_secureFieldOfView;

    [field: SerializeField]
    private float m_life { get; set; }

    private float m_newLife;

    [field: SerializeField]
    private float m_myDamage { get; set; }

    private GameObject[] m_enemies;
    
    public bool OnHitStimuliReceived { get; set; } = false;
    
    public bool OnStunStimuliReceived { get; set; } = false;

    private bool EnemyDefeated = false;

    private Vector2 m_highestPosition = Vector2.zero;

    private Vector2 m_lowestPosition = Vector2.positiveInfinity;

    public List<AudioSource> m_audioSources;

    private Vector3 m_lastPosition = Vector3.zero;

    //[SerializeField]
    //public CharacterEffectController EffectController;

    protected override void Awake()
    {
        base.Awake();
        EnemyDefeated = false;
        m_enemies = FindEnemies();
        m_newLife = m_life;
    }


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<CharacterState>();
        m_possibleStates.Add(new FreeState());
        m_possibleStates.Add(new JumpState());
        m_possibleStates.Add(new AttackState());
        m_possibleStates.Add(new VictoryState());
    }

    protected override void Start()
    {
        Camera = Camera.main;


        foreach (CharacterState state in m_possibleStates)
        {
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
        m_lastPosition = new Vector2(RB.transform.position.x, RB.transform.position.z);
    }

    protected override void Update()
    {

        UpdateAnimatorKeyValues();
        base.Update();
    }

    public void UpdateAnimatorKeyValues()
    {
        UpdateAnimatorBoolValue(KEY_STATUS_BOOL_TOUCHGROUND, m_floorTrigger.IsOnFloor);
        float maxSpeed = AcceptInput ? GetCurrentMaxSpeedFromInput() : CurrentRelativeVelocity.magnitude;
        Animator.SetFloat("MoveX", CurrentRelativeVelocity.x / maxSpeed);
        Animator.SetFloat("MoveY", CurrentRelativeVelocity.y / maxSpeed);
    }

    protected override void FixedUpdate()
    {
        if(AcceptInput)
        {
            SetDirectionalInputs();
            m_currentState.OnFixedUpdate();
            FixedSet2dRelativeVelocityFromInput();
        }
        else
        {
            m_currentState.OnFixedUpdate();
            FixedSet2dRelativeVelocity();
        }
    }

    // Only called by InAirState to ensure only when in air it's called
    public void InAirFixedUpdateFallHeight()
    {
        if(m_highestPosition.y < transform.position.y)
        {
            m_highestPosition = transform.position;
        }    
        if(m_lowestPosition.y >  transform.position.y)
        {
            m_lowestPosition = transform.position;
        }
        float f = m_highestPosition.y - m_lowestPosition.y;
        if (f > 0)
        {
            SetFloatFallHeight(f);
        }
        else
        {
            SetFloatFallHeight(0.0f);
        }
    }

    public void InAirResetFallHeight()
    {
        m_highestPosition = Vector2.zero;
        m_lowestPosition = Vector2.positiveInfinity;
        SetFloatFallHeight(0);
    }

    public const string KEY_STATUS_FLOAT_FALL_HEIGHT = "FallHeight";

    public bool IsFallingFromHigh()
    {
        return Animator.GetFloat(KEY_STATUS_FLOAT_FALL_HEIGHT) > m_stunHeight;
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        if (collision.collider.name == "Arms")
        {
            IDamageable enemy = collision.collider.GetComponentInParent<EnemyController>();
            enemy.ReceiveDamage(EDamageType.Count, m_myDamage);
        }
    }

    public void Jump()
    {
        RB.AddForce(Vector3.up * JumpIntensity, ForceMode.Acceleration);
    }

    public void ReceiveDamage(EDamageType damageType, float damage)
    {

        if (damageType == EDamageType.Normal)
        {
            m_life -= damage;
        }
        else if (damageType == EDamageType.Stunning)
        {
            m_life -= 2 * damage;
        }
        else if (damageType == EDamageType.Count)
        {
            m_life -= damage;
        }
        if (m_life <= 0)
        {
            OnStunStimuliReceived = true;
            m_life = m_newLife;
        }
        else
        {
            OnHitStimuliReceived = true;
        }
    }

    public void FixedUpdateQuickDeceleration()
    {
        var oppositeDirectionForceToApply = -RB.velocity * DecelerationValue * Time.fixedDeltaTime;
        RB.AddForce(oppositeDirectionForceToApply, ForceMode.Acceleration);
    }

    public bool IsInContactWithFloor()
    {
        return m_floorTrigger.IsOnFloor;
    }

    GameObject[] FindEnemies()
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new System.Collections.Generic.List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == 10)
            {
                goList.Add(goArray[i]);
            }
        }
        return goList.Count == 0 ? null : goList.ToArray();
    }

    public List<Collider> GetAttackableEnemies()
    {

        if(m_enemies == null || m_enemies.Length == 0)
            return null;
        
        List<Collider> ret = new List<Collider>();

        foreach (GameObject enemy in m_enemies)
        {
            if (enemy == null || enemy.IsDestroyed())
                continue;

            Vector3 v3 = enemy.transform.position - RB.transform.position;
            float angle = Vector3.Angle(v3 * TO_RADIAN, RB.transform.forward);

            if (angle <= m_secureFieldOfView.y && v3.magnitude <= m_secureFieldOfView.x)
                ret.Add(enemy.GetComponent<Collider>());

        }
        return ret.Count == 0 ? null : ret;
    }

    public void UpdateEnemies()
    {
        m_enemies = FindEnemies();
    }

    public void UpdateAnimatorBoolValue(string key, bool value)
    {
        Animator.SetBool(key, value);
    }

    public void FixedSet2dRelativeVelocityFromInput()
    {
        Vector3 relativeVelocity = RB.transform.InverseTransformDirection(RB.velocity);
        CurrentRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z) / GetCurrentMaxSpeedFromInput();
    }

    public void FixedSet2dRelativeVelocity()
    {
        Vector3 distance = RB.transform.position - m_lastPosition;
        CurrentRelativeVelocity = new Vector2(distance.x, distance.z) / Time.fixedDeltaTime;
        m_lastPosition = RB.transform.position;
    }

    public float GetCurrentMaxSpeedFromInput()
    {
        if (Mathf.Approximately(CurrentDirectionalInputs.magnitude, 0))
        {
            return MaxForwardVelocity;
        }

        var normalizedInputs = CurrentDirectionalInputs.normalized;

        var currentMaxVelocity = Mathf.Pow(normalizedInputs.x, 2) * MaxSideVelocity;

        if (normalizedInputs.y > 0)
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxForwardVelocity;
        }
        else
        {
            currentMaxVelocity += Mathf.Pow(normalizedInputs.y, 2) * MaxBackwardVelocity;
        }

        return currentMaxVelocity;
    }

    public void SetDirectionalInputs()
    {
        CurrentDirectionalInputs = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            CurrentDirectionalInputs += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            CurrentDirectionalInputs += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            CurrentDirectionalInputs += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            CurrentDirectionalInputs += Vector2.right;
    }

    public void EnableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, true);
    }

    public void DisableTouchGround()
    {
        Animator.SetBool(KEY_STATUS_BOOL_TOUCHGROUND, false);
    }

    public void SetFloatFallHeight(float fallHeight)
    {
        Animator.SetFloat(KEY_STATUS_FLOAT_FALL_HEIGHT, fallHeight);
    }

    public void SetEnemyDefeated(bool  defeated)
    {
        EnemyDefeated = defeated;
    }

    public bool IsEnemyDefeated()
    {
        return EnemyDefeated;
    }

}
