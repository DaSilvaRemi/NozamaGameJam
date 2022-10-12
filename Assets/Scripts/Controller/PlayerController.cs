using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerController : CharController
{
    //[Header("Throwable Gameobjects Settings")]
    //[Tooltip("Prefab")]
    //[SerializeField] private GameObject m_ThrowableGOPrefab;

    #region CharController methods
    protected override void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        base.TranslateObject(verticalInput, transform.forward);
        base.RotateObject(horizontalInput);
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
    }

    private void FixedUpdate()
    {
        this.Move();
    }
    #endregion
}
