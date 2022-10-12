using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class ButtonUI : MonoBehaviour
{
    /// <summary>
    /// Handle the UI Button Click to raise <see cref="ButtonClickedEvent"/>
    /// </summary>
    public void HandleButtonClick()
    {
        EventManager.Instance.Raise(new ButtonClickedEvent());
    }
}
