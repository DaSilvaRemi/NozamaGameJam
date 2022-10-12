using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : CharController
{
    [Header("CollectableController properties")]
    [SerializeField] private bool m_IsRotating = false;
    [SerializeField] private bool m_IsFloating = false;
    [SerializeField] private float m_CooldownJumpDuration = 0.2f;

    private float m_NextJumpTime;

    #region CharController methods
    /// <summary>
    /// Move the collectable by jumping and rotating
    /// </summary>
    protected override void Move()
    {
        if (this.m_IsRotating)
        {
            this.RotateObject();
        }

        if (this.m_IsFloating && Time.time > this.m_NextJumpTime)
        {
            this.m_NextJumpTime += m_CooldownJumpDuration;
            base.Jump();
        }
    }

    protected override void RotateObject()
    {
        Vector3 targetAngularVelocity = base.RotatingSpeed * Vector3.up;
        base.RotateObject(targetAngularVelocity);
    }
    #endregion


    #region MonoBehaviour methods
    protected override void Awake()
    {
        base.Awake();
        m_NextJumpTime = Time.time;
    }


    private void FixedUpdate()
    {
        this.Move();
    }
    #endregion
}
