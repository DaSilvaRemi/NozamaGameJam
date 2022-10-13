using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerController : CharController, IEventHandler
{
    [SerializeField] private int m_NbPlayerLife;
    private bool m_IsOnGround;

    //[Header("Throwable Gameobjects Settings")]
    //[Tooltip("Prefab")]
    //[SerializeField] private GameObject m_ThrowableGOPrefab;

    #region CharController methods
    protected override void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (this.m_IsOnGround)
        {
            base.TranslateObject(horizontalInput, transform.forward);
            base.RotateObject(verticalInput, -transform.right);
        }
    }
    #endregion

    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent e)
    {
        if (e.eNonLivres >= this.m_NbPlayerLife)
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
    }

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

    private void OnCollisionStay(Collision collision)
    {
        m_IsOnGround = true;
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

    #region EventSubscriptions
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
    }
    #endregion
}
