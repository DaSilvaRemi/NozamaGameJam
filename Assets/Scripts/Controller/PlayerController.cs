using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerController : CharController
{
    private bool m_IsOnGround;
    //[Header("Throwable Gameobjects Settings")]
    //[Tooltip("Prefab")]
    //[SerializeField] private GameObject m_ThrowableGOPrefab;

    #region CharController methods
    protected override void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (this.m_IsOnGround)
        {
            base.TranslateObject(horizontalInput, transform.forward);
        }
    }
    #endregion

    #region MonoBehaviour METHODS

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            EventManager.Instance.Raise(new LevelFinishEvent());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_IsOnGround = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        m_IsOnGround = false;
    }

    private void FixedUpdate()
    {
        this.Move();
    }
    #endregion
}
