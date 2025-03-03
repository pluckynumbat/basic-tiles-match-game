using UnityEngine;
using TMPro;

/// <summary>
/// Dialog shown when the level ends. using the action buttons on it, the player can leave the level or restart it 
/// </summary>
public class UILevelEndDialog : UIDialogBase
{
    private const string WIN_TEXT = "You Won!";
    private const string LOSS_TEXT = "You Lost :(";
        
    public TextMeshProUGUI titleText; // this is set during run time

    // set title based on the data (player won or lost)
    public override void Setup(object[] data)
    {
        if (data.Length == 0)
        {
            Debug.LogError("No data sent to setup the level preview dialog, abort");
            return;
        }
        
        bool won = (bool)data[0];

        titleText.text = won ? WIN_TEXT : LOSS_TEXT;
    }

    //reload the level scene
    public void OnRestartButtonClicked()
    {
        UIEvents.RaiseRestartLevelRequestEvent();
    }
    
    //go to the main scene
    public void OnQuitButtonClicked()
    {
        UIEvents.RaiseLeaveLevelRequestEvent();
    }
}
