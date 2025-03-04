using UnityEngine;
using TMPro;

/// <summary>
/// Dialog shown when the level ends. using the action buttons on it, the player can leave the level or restart it
/// or go on to the next level in random mode
/// </summary>
public class UILevelEndDialog : UIDialogBase
{
    private const string WIN_TEXT = "You Won!";
    private const string LOSS_TEXT = "You Lost :(";
        
    public TextMeshProUGUI titleText; // this is set during run time
    public UIGoalsDisplay goalsDisplay; // shown if the player won the level
    public GameObject leaveButton; // only shown when the player wins
    public GameObject restartButton; // only shown when the player loses
    public GameObject nextButton; // only shown in random mode
    
    // set title based on the data (player won or lost)
    // enable the 'next' button if the dialog is displayed in random mode
    // show goals display if the player won the level
    public override void Setup(object[] data)
    {
        if (data.Length < 3)
        {
            Debug.LogError($"Setup of the level preview dialog expects 2 arguments, got {data.Length}, abort");
            return;
        }
        
        bool won = (bool)data[0];
        titleText.text = won ? WIN_TEXT : LOSS_TEXT;
        leaveButton?.SetActive(won);
        restartButton?.SetActive(!won);
        goalsDisplay?.gameObject.SetActive(won);
        
        if (won) // set up goals display
        {
            LevelData levelData = data[1] as LevelData;
            if (levelData == null)
            {
                Debug.LogError("Invalid level data in the level end dialog, abort");
                return;
            }
        
            if (goalsDisplay == null)
            {
                Debug.LogError("Goals display is null, please check the prefab, abort");
                return;
            }
            
            goalsDisplay.SetupGoalsDisplay(levelData, true);
        }
        
        bool isRandomMode = (bool)data[2];
        nextButton?.SetActive(isRandomMode);
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
