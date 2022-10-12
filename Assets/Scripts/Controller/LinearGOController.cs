using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.Tweening;
using SDD.Events;

public class LinearGOController : CharController, IEventHandler
{
    [Header("Enemy Setup")]
    [Tooltip("The final position of object")]
    [SerializeField] private Transform m_TransformEnd;
    [Tooltip("If the object move or not ?")]
    [SerializeField] private bool m_IsMove;
    [Tooltip("If the object move in cycle ?")]
    [SerializeField] private bool m_IsCycle;

    private Vector3 m_StartPosition; 
    private IEnumerator m_MyTranslateCoroutine = null;

    #region LinearGOController properties
    /// <summary>
    /// If the current object is moving
    /// </summary>
    private bool IsMove { get => this.m_IsMove; set => this.m_IsMove = value; }

    /// <summary>
    /// If the current object is finish to translate
    /// </summary>
    private bool IsFinishTranslate { get => base.transform.position.Equals(this.m_TransformEnd.position) || base.transform.position.Equals(this.m_StartPosition); }
    #endregion

    #region Events handlers
    /// <summary>
    /// OnButtonActivateGOClickedEvent we check if <see cref="ButtonActivateGOClickedEvent.eGameObject"/> is not null and equals to the current object, after we move the object
    /// </summary>
    /// <param name="e"></param>
    private void OnButtonActivateGOClickedEvent(ButtonActivateGOClickedEvent e)
    {
        if(e.eGameObject != null && e.eGameObject.Equals(this.gameObject))
        {
            this.IsMove = true;
            this.Move();
        }
    }
    #endregion

    #region CharController methods
    protected override void Move()
    {
        if (this.m_MyTranslateCoroutine != null && this.IsMove) StartCoroutine(this.m_MyTranslateCoroutine);
    }
    #endregion

    #region LinearGOController Methods
    /// <summary>
    /// If the current object movement is cycle and is finish to transltate we reverse the translation
    /// </summary>
    private void UpdateLinearGOController()
    {
        if (!this.m_IsCycle || !this.IsFinishTranslate)
        {
            return;
        }

        //We reverse the translation to go alternatively between start and finish
        this.StopTranslate();
        Vector3 endPosition = this.transform.position.Equals(this.m_TransformEnd.position) ? this.m_StartPosition : this.m_TransformEnd.position;
        this.m_MyTranslateCoroutine = Tools.MyTranslateCoroutine(base.transform, base.transform.position, endPosition, 200, EasingFunctions.Linear, TranslationSpeed);
        this.Move();
    }

    /// <summary>
    /// Stop translate the current object
    /// </summary>
    private void StopTranslate()
    {
        if (this.m_MyTranslateCoroutine != null) {
            StopCoroutine(this.m_MyTranslateCoroutine);
            this.m_MyTranslateCoroutine = null;
        }
    }
    #endregion

    #region Events suscribtions
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<ButtonActivateGOClickedEvent>(OnButtonActivateGOClickedEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<ButtonActivateGOClickedEvent>(OnButtonActivateGOClickedEvent);
    }
    #endregion

    #region MonoBehaviour methods
    protected override void Awake()
    {
        base.Awake();
        this.m_MyTranslateCoroutine = Tools.MyTranslateCoroutine(base.transform, base.transform.position, this.m_TransformEnd.position, 200, EasingFunctions.Linear, TranslationSpeed);
        this.m_StartPosition = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
        this.SubscribeEvents();
    }

    private void FixedUpdate()
    {
        this.UpdateLinearGOController(); 
    }

    protected virtual void OnDisable()
    {
        this.StopTranslate();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}