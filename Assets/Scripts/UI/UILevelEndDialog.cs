using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Dialog shown when the level ends. using the action buttons on it, the player can leave the level or restart it 
/// </summary>
public class UILevelEndDialog : UIDialogBase
{
    private const string WIN_TEXT = "You Won!";
    private const string LOSS_TEXT = "You Lost :(";
        
    public TextMeshProUGUI titleText; // this is set during run time

    // set title based on the data (player won or lost)
    public override void Setup(object data)
    {
        bool won = (bool)data;

        titleText.text = won ? WIN_TEXT : LOSS_TEXT;
    }

    //reload the scene
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(1); // TODO make this a constant
    }
    
    //go to the main scene
    public void OnQuitButtonClicked()
    {
        SceneManager.LoadScene(0);  // TODO make this a constant
    }
}
