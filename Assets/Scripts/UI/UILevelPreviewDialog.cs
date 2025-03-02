using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// UI Dialog that shows up when player presses a level select node in the main scene,
/// It shows some details about the level that player will be playing,
/// and lets the player enter that level
/// </summary>
public class UILevelPreviewDialog : UIDialogBase
{
    private const string PLAY_TEXT_PREFIX = "Play Level: ";
        
    public TextMeshProUGUI titleText; // this is set during run time
    public UIGoalsDisplay goalsDisplay;

    // set title text and goals display based on supplied data
    public override void Setup(object data)
    {
        LevelData levelData = data as LevelData;
        if (levelData == null)
        {
            Debug.LogError("Invalid level data in the level preview dialog, abort");
            return;
        }
        
        if (goalsDisplay == null)
        {
            Debug.LogError("Goals display is null, please check the prefab, abort");
            return;
        }

        titleText.text = PLAY_TEXT_PREFIX + levelData.name;
        goalsDisplay.SetupGoalsDisplay(levelData);
    }

    //request to enter the level scene to play this level
    public void OnPlayButtonClicked()
    {
        UIEvents.RaisePlayLevelRequestEvent();
    }
    
    // only dismiss this dialog
    public void OnQuitButtonClicked()
    {
        Destroy(gameObject);
    }
}
