using System;
using Unity.Mathematics;
using UnityEngine;

public class FreeState : CharacterState
{

   // private AudioSource m_clip;

    /*public FreeState(AudioSource clip)
    {
        m_clip = clip;
        m_efxState = EFXState.EWalk;
    }*/

    public override void OnEnter()
    {
        Debug.Log("Entering free state");
        base.OnEnter();
    }

    public override void OnExit()
    {
        Debug.Log("Exiting free state");
        base.OnExit();
        /*
            if (m_clip != null)
            {
                m_clip.Stop();
            }
        */
    }

    public override void OnUpdate()
    {
        m_stateMachine.UpdateAnimatorKeyValues();
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {

        FixedUpdateRotateWithCamera();

        if (m_stateMachine.CurrentDirectionalInputs == Vector2.zero)
        {
            m_stateMachine.FixedUpdateQuickDeceleration();
            return;
        }
        Vector2 velocity = m_stateMachine.CurrentDirectionalInputs;
        //float speed = velocity.magnitude;
        ApplyMovementsOnFloorFU(velocity);

        // the higher the speed is, the faster the step sounds
        //float speed = m_stateMachine.GetCurrentMaxSpeed();

        /*
        if (m_clip != null)
        {
            if (speed > 0 && !m_clip.isPlaying)
            {
                if (m_stepTimer >= 1 / speed)
                {
                    m_clip.Play();
                    m_stepTimer = 0;
                }
            }
            m_stepTimer += Time.fixedDeltaTime;
        }
        */
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2)
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward * inputVector2.y, Vector3.up);
        vectorOnFloor += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right * inputVector2.x, Vector3.up);
        vectorOnFloor.Normalize();

        m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);

        var currentMaxSpeed = m_stateMachine.MaxVelocity;

        if (m_stateMachine.RB.velocity.magnitude > currentMaxSpeed)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= currentMaxSpeed;
        }
    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return true;
    }

    private void FixedUpdateRotateWithCamera()
    {
        var forwardCamOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        m_stateMachine.RB.transform.LookAt(forwardCamOnFloor + m_stateMachine.RB.transform.position);
    }

}
