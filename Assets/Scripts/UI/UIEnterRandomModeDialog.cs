using TMPro;

/// <summary>
/// UI Dialog that shows up when the player clicks the random mode button in the main scene
/// this dialog introduces the player to random mode and lets them enter the first random level!
/// </summary>
public class UIEnterRandomModeDialog : UIDialogBase
{
    private const string TITLE_TEXT = "Enter Random Mode!";
    
    private const string DESCRIPTION_TEXT = "Start an endless journey of random levels!";
    
    public TextMeshProUGUI titleTextBox;
    public TextMeshProUGUI descriptionTextBox;
    public override void Setup(object[] data)
    {
        titleTextBox.text = TITLE_TEXT;
        descriptionTextBox.text = DESCRIPTION_TEXT;
    }

    // raise the event to request starting random mode levels
    public void OnPlayButtonClicked()
    {
        UIEvents.RaisePlayRandomModeRequestEvent();
    }
    
    // only dismiss this dialog
    public void OnQuitButtonClicked()
    {
        Destroy(gameObject);
    }
}
