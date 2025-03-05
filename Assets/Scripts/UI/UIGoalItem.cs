using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI script for a single goal item in a level
/// </summary>
public class UIGoalItem : MonoBehaviour
{
    public Image goalImage; // this will contain the goal sprite
    public TextMeshProUGUI textBox; // this will contain remaining goal count
    public Image goalCountBackground; //the background image of the goal count
    public Image completedImage; // this will be an indicator for when the goal is completed
    
    public LevelGoal.GoalType MyGoalType; // the goal type that this item represents
    
    private bool isCompleted; // has the goal been completed already?

    // initial setup at the beginning of a level
    public void SetupGoalItem(LevelGoal.GoalType goalType, Sprite sprite, int count)
    {
        MyGoalType = goalType;
        goalImage.sprite = sprite;
        textBox.text = count.ToString();
        completedImage.enabled = false;
    }

    // whenever there is any goal update
    public void RefreshGoalItem(int newCount)
    {
        if (isCompleted) // if already completed, no need to refresh anymore
        {
            return;
        }

        textBox.text = newCount.ToString();
        
        if (newCount <= 0) // this item was newly completed, enable the completed indicator, and mark as complete
        {
            textBox.enabled = false;
            goalCountBackground.enabled = false;
            completedImage.enabled = true;
            isCompleted = true;
        }
    }

    // used to set up the goals display as part of the level end dialog
    public void SetupPreCompletedGoalItem(LevelGoal.GoalType goalType, Sprite sprite)
    {
        MyGoalType = goalType;
        goalImage.sprite = sprite;
        textBox.enabled = false;
        goalCountBackground.enabled = false;
        completedImage.enabled = true;
    }
}
