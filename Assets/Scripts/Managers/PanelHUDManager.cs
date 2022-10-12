using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PanelHUDManager : Manager<PanelHUDManager>
{
    protected List<GameObject> Panels { get; private set; }

    #region PanelHUDManager Handlers
    /// <summary>
    /// When back to menu button clicked we  send <see cref="MainMenuButtonClickedEvent"/>
    /// </summary>
    public void HandleBackToMenuButton()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }
    #endregion


    #region PanelHUDManager methods
    /// <summary>
    /// Hilde all panels
    /// </summary>
    protected void HideAllPanels()
    {
        this.OpenPanel(null);
    }

    /// <summary>
    /// Open a panel in the list
    /// </summary>
    /// <param name="panel">The panel to open</param>
    protected void OpenPanel(GameObject panel)
    {
        this.Panels.ForEach(item => { if (item) { item.SetActive(item.Equals(panel)); } });
    }
    #endregion

    #region Monobehaviour methods

    protected virtual void Awake()
    {
        base.InitManager();
        this.Panels = new List<GameObject>();
    }
    #endregion
}
