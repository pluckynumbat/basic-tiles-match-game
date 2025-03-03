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
    public GameObject nextLevelButton;
    
    // set title based on the data (player won or lost)
    // enable the 'next' button if the dialog is displayed in random mode
    public override void Setup(object[] data)
    {
        if (data.Length < 2)
        {
            Debug.LogError($"Setup of the level preview dialog expects 2 arguments, got {data.Length}, abort");
            return;
        }
        
        bool won = (bool)data[0];
        titleText.text = won ? WIN_TEXT : LOSS_TEXT;
        
        bool isRandomMode = (bool)data[1];
        nextLevelButton.SetActive(isRandomMode);
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
    
    //load the level scene with a new random level
    public void OnNextButtonClicked()
    {
        UIEvents.RaisePlayRandomModeRequestEvent();
    }
}
