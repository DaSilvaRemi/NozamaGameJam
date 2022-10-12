using SDD.Events;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region MenuManager Handlers
    /// <summary>
    /// When new game button clicked we called <see cref="OnClickButton"/> and send <see cref="NewGameButtonClickedEvent"/>
    /// </summary>
    public void HandleCreateNewGameButton()
    {
        EventManager.Instance.Raise(new NewGameButtonClickedEvent());
    }

    /// <summary>
    /// When help button clicked we called <see cref="OnClickButton"/> and send <see cref="HelpButtonClickedEvent"/>
    /// </summary>
    public void HandleHelpButton()
    {
        EventManager.Instance.Raise(new HelpButtonClickedEvent());
    }

    /// <summary>
    /// When credit button clicked we called <see cref="OnClickButton"/> and send <see cref="CreditButtonClickedEvent"/>
    /// </summary>
    public void HandleCreditButton()
    {
        EventManager.Instance.Raise(new CreditButtonClickedEvent());
    }

    /// <summary>
    /// When exit game button clicked we called <see cref="OnClickButton"/> and send <see cref="ExitButtonClickedEvent"/>
    /// </summary>
    public void HandleExitButton()
    {
        EventManager.Instance.Raise(new ExitButtonClickedEvent());
    }

    /// <summary>
    /// HandleReplayGameButton call <see cref="HandleCreateNewGameButton"/> when <see cref="GameManager.IsWinning"/> is true or else call <see cref="HandleLoadGameButton"/>
    /// </summary>
    public void HandleReplayGameButton()
    {
        if (GameManager.IsWinning)
        {
            this.HandleCreateNewGameButton();
        }
    }
    #endregion
}
