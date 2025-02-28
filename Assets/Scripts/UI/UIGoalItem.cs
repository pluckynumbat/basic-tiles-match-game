using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI script for a single goal item in a level
/// </summary>
public class UIGoalItem : MonoBehaviour
{
    public Image goalImage; // this will contain the sprite
    public TextMeshProUGUI textBox; // this will contain remaining goal count
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
            completedImage.enabled = true;
            isCompleted = true;
        }
    }
}
