using System.Collections;
using System.Collections.Generic;
using TMPro;
using SDD.Events;
using UnityEngine;

public class HUDManager : PanelHUDManager, IEventHandler
{
    [Header("Pause panel")]
    [Tooltip("Game Object")]
    [SerializeField] private GameObject m_PausePanel;

    [Header("HUD TEXT")]
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_NbEnStockTxt;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_ScoreValueTxt;
    [Tooltip("TextMeshPro")]
    [SerializeField] private TextMeshProUGUI m_HabitantNonLivreTxt;

    #region Setters
    /// <summary>
    /// Set score value text
    /// </summary>
    /// <param name="score">The score</param>
    private void SetNbEnStockText(int nbEnStock)
    {
        if (this.m_NbEnStockTxt)
        {
            this.m_NbEnStockTxt.text = nbEnStock.ToString();
        }
    }
    private void SetScoreValueText(int nbLivres)
    {
        if (this.m_ScoreValueTxt)
        {
            this.m_ScoreValueTxt.text = nbLivres.ToString();
        }
    }
    private void SetHabitantNonLivreText(int nbNonLivres)
    {
        if (this.m_HabitantNonLivreTxt)
        {
            this.m_HabitantNonLivreTxt.text = nbNonLivres.ToString();
        }
    }
    #endregion

    #region HUDManager handlers
    /// <summary>
    /// HandleContinueButton in pause and called <see cref="HideAllPanels"/> and throw <seealso cref="ContinueGameEvent"/>
    /// </summary>
    public void HandleContinueButton()
    {
        base.HideAllPanels();
        EventManager.Instance.Raise(new ContinueGameEvent());
    }

    /**
 * <summary>Handle the camera change UI button <see cref="CameraChangeUIButtonEvent"/></summary> 
 */
    public void HandleCameraChangeUIButton()
    {
        EventManager.Instance.Raise(new CameraChangeUIButtonEvent());
    }
    #endregion

    #region Event Listeners
    /// <summary>
    /// On gameStatisticsChangedEvent we calls <see cref="SetTimeValueText"/> and <see cref="SetScoreValueText"/>
    /// </summary>
    /// <param name="gameStatisticsChangedEvent"></param>
    private void OnGameStatisticsChangedEvent(GameStatisticsChangedEvent gameStatisticsChangedEvent)
    {
        // Debug.Log("mon event : " + gameStatisticsChangedEvent.eNonLivres);
        this.SetScoreValueText(gameStatisticsChangedEvent.eNbColisLivree);
        this.SetNbEnStockText(gameStatisticsChangedEvent.eStock);
        this.SetHabitantNonLivreText(gameStatisticsChangedEvent.eNonLivres);
    }

    /// <summary>
    /// OnGamePlayEvent we call <see cref="HideAllPanels"/>
    /// </summary>
    /// <param name="e"></param>
    private void OnGamePlayEvent(GamePlayEvent e)
    {
        base.HideAllPanels();
    }

    /// <summary>
    /// OnGamePauseEvent we call <see cref="GamePauseEvent"/>
    /// </summary>
    /// <param name="e"></param>
    private void OnGamePauseEvent(GamePauseEvent e)
    {
        base.OpenPanel(this.m_PausePanel);
    }
    #endregion

    #region Events Suscribption
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
        EventManager.Instance.AddListener<GamePlayEvent>(OnGamePlayEvent);
        EventManager.Instance.AddListener<GamePauseEvent>(OnGamePauseEvent);
    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(OnGameStatisticsChangedEvent);
        EventManager.Instance.RemoveListener<GamePlayEvent>(OnGamePlayEvent);
        EventManager.Instance.RemoveListener<GamePauseEvent>(OnGamePauseEvent);
    }
    #endregion

    #region MonoBehaviour methods
    protected override void Awake()
    {
        base.Awake();
        base.Panels.AddRange(new GameObject[] { this.m_PausePanel });
    }

    private void OnEnable()
    {
        this.SubscribeEvents();
    }

    private void OnDisable()
    {
        this.UnsubscribeEvents();
    }
    #endregion
}
