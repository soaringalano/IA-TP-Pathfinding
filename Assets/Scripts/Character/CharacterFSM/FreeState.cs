using System;
using Unity.Mathematics;
using UnityEngine;

public class FreeState : CharacterState
{

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        //Debug.Log("Exiting free state");
        base.OnExit();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {

        FixedUpdateRotateWithCamera();

        if(m_stateMachine.AcceptInput)
        {
            if (m_stateMachine.CurrentDirectionalInputs == Vector2.zero)
            {
                m_stateMachine.FixedUpdateQuickDeceleration();
                return;
            }
            Vector2 directionInput = m_stateMachine.CurrentDirectionalInputs;
            ApplyMovementsOnFloorFU(directionInput, m_stateMachine.CurrentRelativeVelocity);
        }
    }

    private void ApplyMovementsOnFloorFU(Vector2 inputVector2, Vector2 velocity2)
    {
        if(m_stateMachine.AcceptInput)
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
        else
        {

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
