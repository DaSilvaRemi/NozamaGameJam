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
            //*
            // Pour redresser le perso s'il tombe -> Quaternion de redressement
            Quaternion qRotUpright = Quaternion.FromToRotation(transform.up, Vector3.up);

            // M?canique de redressement qui fait que le perso rebondit sur une surface
            // exemple : une balle dans la t?te -> un mouvement du perso
            Quaternion qOrientSlightlyUpright = Quaternion.Slerp(transform.rotation, qRotUpright * transform.rotation, Time.fixedDeltaTime * 100);
            // -> ? chaque frame, le perso se redresse de 8% par frame (2% * 4)

            float deltaAngle = horizontalInput * 100 * this.RotatingSpeed * Time.fixedDeltaTime;

            Quaternion qRot = Quaternion.AngleAxis(deltaAngle, transform.right);
            // transform.up devrait ?tre remplac? par le up, une fois slightly
            // mais pas bcp de changement dans les vect donc c'est ok

            this.Rigidbody.MoveRotation(qRot * qOrientSlightlyUpright);
            //*/
        }
        else
        {
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

        if (this.transform.position.y <= -3.0f)
        {
            EventManager.Instance.Raise(new LevelGameOverEvent());
        }
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
