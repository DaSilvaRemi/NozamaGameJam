using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class CharController : MonoBehaviour
{
    [Header("Mvt Setup")]
    [Tooltip("unit m/s")]
    [SerializeField] private float m_TranslationSpeed;
    [Tooltip("unit m/s")]
    [SerializeField] private float m_JumpSpeed;
    [Tooltip("unit: �/s")]
    [SerializeField] private float m_RotatingSpeed;

    [Header("SFX")]
    [Tooltip("Audio clip MV3")]
    [SerializeField] private AudioClip m_CharacterWalkClip;

    #region CharController properties
    /// <summary>
    /// The rigidbody
    /// </summary>
    protected Rigidbody Rigidbody { get; set; }

    /// <summary>
    /// The translation speed
    /// </summary>
    protected float TranslationSpeed { get => this.m_TranslationSpeed; }

    /// <summary>
    /// The rotating speed
    /// </summary>
    protected float RotatingSpeed { get => this.m_RotatingSpeed; }

    /// <summary>
    /// The jump speed
    /// </summary>
    protected float JumpSpeed { get => this.m_JumpSpeed; }
    #endregion

    #region Character physics controls methods
    /// <summary>
    /// Translate the current object
    /// </summary>
    /// <param name="direction">An direction to translate</param>
    protected virtual void TranslateObject(Vector3 direction)
    {
        this.TranslateObject(1, direction);
    }

    /// <summary>
    /// Translate the object depenfing to an input and a direction
    /// </summary>
    /// <param name="verticalInput">Vlaue of the vertical input</param>
    /// <param name="direction">The vector direction</param>
    protected virtual void TranslateObject(float verticalInput, Vector3 direction)
    {
        Vector3 targetVelocity = this.m_TranslationSpeed * verticalInput * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        Vector3 velocityChange = targetVelocity - this.Rigidbody.velocity;
        this.Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Rotate the current object
    /// </summary>
    protected virtual void RotateObject()
    {
        this.RotateObject(1);
    }

    /// <summary>
    /// Rotate the current object depending on the horizontalInput
    /// </summary>
    /// <param name="horizontalInput">The horizontal input</param>
    protected virtual void RotateObject(float horizontalInput)
    {
        Vector3 targetAngularVelocity = horizontalInput * this.m_RotatingSpeed * this.transform.up;
        this.RotateObject(targetAngularVelocity);
    }

    /// <summary>
    /// Rotate the current object depending on the targetAngularVelocity
    /// </summary>
    /// <param name="targetAngularVelocity">The targetAngularVelocity</param>
    protected virtual void RotateObject(Vector3 targetAngularVelocity)
    {
        Vector3 angularVelocityChange = targetAngularVelocity - this.Rigidbody.angularVelocity;
        this.Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Move the current character
    /// </summary>
    protected virtual void Move()
    {
        this.TranslateObject(Vector3.left);
    }

    /// <summary>
    /// Jump the current character <see cref="PlayJumpSound"/>
    /// </summary>
    protected virtual void Jump()
    {
        this.Rigidbody.velocity = this.JumpSpeed * new Vector3(0, 1, 0);
    }
    #endregion

    #region MonoBehaviour METHODS
    protected virtual void Awake()
    {
        this.Rigidbody = this.GetComponent<Rigidbody>();
    }
    #endregion
}