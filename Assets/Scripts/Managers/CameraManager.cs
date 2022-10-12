using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class CameraManager : Manager<CameraManager>, IEventHandler
{
    [Header("Cameras")]
    [Tooltip("Camera")]
    [SerializeField] private Camera[] m_Cameras;
    [Tooltip("Unit : s")]
    [SerializeField] private float m_CooldownDuration;

    private int m_IndexCameraSelected = 0;
    private float m_NextCameraChangedTime;

    #region CameraManager listeners
    /// <summary>
    /// OnCameraChangeUIButtonEvent we called <see cref="ChangeCamera"/>
    /// </summary>
    /// <param name="e">The event</param>
    private void OnCameraChangeUIButtonEvent(CameraChangeUIButtonEvent e)
    {
        this.ChangeCamera();
    }
    #endregion

    #region CameraManager Handler
    /**
     * <summary>Handle the camera change key</summary> 
     */
    private void HandleCameraChangeKey()
    {
        if (Input.GetButton("ChangeCamera") && Time.time > this.m_NextCameraChangedTime)
        {
            this.ChangeCamera();
            this.m_NextCameraChangedTime = Time.time + this.m_CooldownDuration;
        }
    }
    #endregion

    #region CameraManager Methods
    /**
     * <summary>Change the camera</summary>
     */
    private void ChangeCamera()
    {
        int nextCameraWillBeSelected = this.m_IndexCameraSelected + 1;

        if (nextCameraWillBeSelected >= this.m_Cameras.Length) nextCameraWillBeSelected = 0;

        this.ChooseCamera(this.m_Cameras[nextCameraWillBeSelected]);
        this.m_IndexCameraSelected = nextCameraWillBeSelected;
    }

    /// <summary>
    /// Choose an camera by desactivating others
    /// </summary>
    /// <param name="selectedCamera">The camera will be choosen</param>
    private void ChooseCamera(Camera selectedCamera)
    {
        foreach (Camera camera in this.m_Cameras)
        {
            if (!camera.Equals(selectedCamera))
            {
                camera.enabled = false;
            }
        }
        selectedCamera.enabled = true;
    }
    #endregion

    #region Events Subscriptions
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<CameraChangeUIButtonEvent>(OnCameraChangeUIButtonEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<CameraChangeUIButtonEvent>(OnCameraChangeUIButtonEvent);
    }
    #endregion

    #region MonoBehaviour methods

    private void Awake()
    {
        base.InitManager();
        this.SubscribeEvents();
    }

    private void Start()
    {
        this.m_NextCameraChangedTime = Time.time;
        this.ChooseCamera(this.m_Cameras[0]);
    }

    private void FixedUpdate()
    {
        this.HandleCameraChangeKey();
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
