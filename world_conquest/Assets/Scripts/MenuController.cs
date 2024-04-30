using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls all menu interactions and UI transitions in the game.
/// This class manages player selection, color selection, and game initialization.
/// </summary>
public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    /// <summary>
    /// Reference to the start screen GameObject.
    /// </summary>
    [SerializeField] private GameObject StartScreen;

    /// <summary>
    /// Reference to the player screen GameObject.
    /// </summary>
    [SerializeField] private GameObject PlayerScreen;

    /// <summary>
    /// Reference to the name selection screen GameObject.
    /// </summary>
    [SerializeField] private GameObject NameSelectScreen;

    /// <summary>
    /// Reference to the color selection screen GameObject.
    /// </summary>
    [SerializeField] private GameObject ColourSelectScreen;

    /// <summary>
    /// Start button to trigger the selection of the number of players.
    /// </summary>
    [SerializeField] private Button startButton;

    /// <summary>
    /// Settings button to open game settings.
    /// </summary>
    [SerializeField] private Button settingsButton;

    /// <summary>
    /// Dropdown for selecting the number of human players.
    /// </summary>
    [SerializeField] private Dropdown HumanDropdown;

    /// <summary>
    /// Dropdown for selecting the number of AI players.
    /// </summary>
    [SerializeField] private Dropdown AIDropdown;

    /// <summary>
    /// Button to confirm the player selection and proceed to the next menu.
    /// </summary>
    [SerializeField] private Button confirmPlayerButton;

    /// <summary>
    /// List of buttons for each color selection.
    /// </summary>
    [SerializeField] private List<Button> colourButtons;

    /// <summary>
    /// Input field for entering the player's name.
    /// </summary>
    [SerializeField] private InputField nameInput;

    /// <summary>
    /// Displays the name selection prompt or error messages.
    /// </summary>
    [SerializeField] private TextMeshProUGUI nameSelectText;

    /// <summary>
    /// Displays error messages related to player settings.
    /// </summary>
    [SerializeField] private TextMeshProUGUI errorPlayerText;

    /// <summary>
    /// Displays error messages related to name selection.
    /// </summary>
    [SerializeField] private TextMeshProUGUI errorNameText;

    /// <summary>
    /// Displays the color selection prompt or error messages.
    /// </summary>
    [SerializeField] private TextMeshProUGUI colourSelectText;

    /// <summary>
    /// Button to confirm the name selection and proceed to color selection.
    /// </summary>
    [SerializeField] private Button confirmNameButton;

    /// <summary>
    /// A list of available colors that players can choose for the game.
    /// </summary>
    private List<Color32> gameColours= new List<Color32> { 
        new Color32(0,199,240,255),
        new Color32(0,7,192,255),  
        new Color32(90,192,0,255),
        new Color32(154,0,141,255), 
        new Color32(255,101,0,255),
        new Color32(190,18,0,255)};

    /// <summary>
    /// The total number of players, including both human and AI players.
    /// </summary>
    private int amountOfPlayers;

    /// <summary>
    /// An array storing the names of all players participating in the game.
    /// This array is populated during the name selection phase.
    /// </summary>
    private string[] playerNames;

    /// <summary>
    /// An array of colors selected by each player corresponding to the playerNames array.
    /// These colors are chosen during the color selection phase and are used to customize player appearances in the game.
    /// </summary>
    private Color32[] playerColours;

    /// <summary>
    /// A flag indicating whether a player's name has been confirmed through the name input process.
    /// This is set to true once a valid name is entered and confirmed.
    /// </summary>
    private bool nameConfirmed = false;

    /// <summary>
    /// A flag indicating whether a player's color selection has been confirmed.
    /// This is set to true once a player has selected a color and the selection is confirmed.
    /// </summary>
    private bool colourConfirmed = false;


    /// <summary>
    /// Initializes the singleton instance, activates the start screen, and sets up button listeners.
    /// </summary>
    void Start()
    {
        Instance = this;
        StartScreen.SetActive(true);
        startButton.onClick.AddListener(SelectAmountOfPlayer);
        settingsButton.onClick.AddListener(SelectSettings);
        UnloadGameScene();
    }

    /// <summary>
    /// Unloads the game scene until the menu scene has finished initialising the game
    /// </summary>
    void UnloadGameScene(){
        UnityEngine.SceneManagement.SceneManager.UnloadScene("GameScene");
    }
    /// <summary>
    /// Displays settings options and logs the action to the console.
    /// </summary>
    private void SelectSettings() 
    {
        print("Clicked settings");
    }

    /// <summary>
    /// Handles player amount selection by displaying the player selection screen and setting up further interactions.
    /// </summary>    
    private void SelectAmountOfPlayer()
    {
        StartScreen.SetActive(false);
        PlayerScreen.SetActive(true);
        confirmPlayerButton.onClick.AddListener(EnterPlayerName);
    }

    /// <summary>
    /// Processes the amount of players selected, validates it, and transitions to the name selection screen.
    /// </summary>
    private void EnterPlayerName()
    {   
        amountOfPlayers = GetHumanPlayers() + GetAIPlayers();
        if (amountOfPlayers > 6 || amountOfPlayers < 2) {
            errorPlayerText.text = "ERROR: Please select 2-6 players!";
            confirmPlayerButton.onClick.RemoveAllListeners();
            SelectAmountOfPlayer();
            return;
        }
        if (GetAIPlayers()  > 0){
            errorPlayerText.text = "ERROR: AI players is unavaliable";
            confirmPlayerButton.onClick.RemoveAllListeners();
            SelectAmountOfPlayer();
            return;
        }

        errorPlayerText.text = "";
        PlayerScreen.SetActive(false);
        NameSelectScreen.SetActive(true);
        playerNames = new string[amountOfPlayers];

        StartCoroutine(EnterPlayerNamesCoroutine(GetHumanPlayers()));
    }

    /// <summary>
    /// Collects and validates player names sequentially through a coroutine.
    /// </summary>
    /// <param name="numPlayers">The number of human players to process.</param>
    /// <returns>Returns an IEnumerator for coroutine management.</returns>
    private IEnumerator EnterPlayerNamesCoroutine(int numPlayers)
    {
        for (int i = 0; i < numPlayers; i++)
        {
            nameSelectText.text = "Player " + (i + 1) + " enter your name";
            yield return WaitForNameInput();
            playerNames[i] = nameInput.text;
            errorNameText.text = "";
            nameInput.text = "";
        }

        StartCoroutine(SelectColour());
    }

    /// <summary>
    /// Waits for the confirm name button to be pressed, triggering the validation of the input name.
    /// </summary>
    /// <returns>Returns an IEnumerator for coroutine management.</returns>
    private IEnumerator WaitForNameInput()
    {
        confirmNameButton.onClick.AddListener(() => { StartCoroutine(ConfirmNameInput()); });
        while (!nameConfirmed){
            yield return null;
        }
        nameConfirmed = false;
    }

    /// <summary>
    /// Validates the entered name for length and uniqueness.
    /// </summary>
    /// <returns>Returns an IEnumerator for coroutine management.</returns>
    private IEnumerator ConfirmNameInput()
    {   
        nameConfirmed = true;
        string name = nameInput.text.Trim();
        if (name.Length > 13 || name.Length < 2) {
            errorNameText.text = "Your name must be between 2-13 characters!";
            nameConfirmed = false;
        }
        if (playerNames.Contains(name))
        {
            errorNameText.text = "Your name must be unique";
            nameConfirmed = false;
        }
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// Starts the color selection process through a coroutine.
    /// </summary>
    /// <returns>Returns an IEnumerator for coroutine management.</returns>
    private IEnumerator SelectColour()
    {
        NameSelectScreen.SetActive(false);
        ColourSelectScreen.SetActive(true);
        playerColours = new Color32[amountOfPlayers];
        for (int i = 0; i < GetHumanPlayers(); i++)
        {
            colourSelectText.text = playerNames[i] + " select your game colour!";  
            yield return WaitForColourSelection(i);
        }
        LoadGameScene();
    }

    /// <summary>
    /// Waits until a player has selected a colour.
    /// </summary>
    /// <param name="playerIndex">The index of the player selecting the colour.</param>
    /// <returns>Returns an IEnumerator for coroutine management.</returns>
    private IEnumerator WaitForColourSelection(int playerIndex)
    {
        for (int i = 0; i < colourButtons.Count; i++)
        {
            int index = i;
            colourButtons[i].onClick.RemoveAllListeners();
            colourButtons[i].onClick.AddListener(() => { SetColour(playerIndex, index); });
        }
        while (!colourConfirmed)
        {
            yield return null;
        }
        colourConfirmed = false;
    }

    /// <summary>
    /// Sets the selected colour for a player and disables the corresponding button to prevent re-selection.
    /// </summary>
    /// <param name="playerIndex">The player's index in the colour array.</param>
    /// <param name="colourIndex">The index of the selected colour.</param>
    private void SetColour(int playerIndex, int colourIndex)
    {
        playerColours[playerIndex] = gameColours[colourIndex];
        colourButtons[colourIndex].gameObject.SetActive(false);
        colourConfirmed = true;
    }

    /// <summary>
    /// Returns the number of human players based on the dropdown selection.
    /// </summary>
    /// <returns>The number of human players.</returns>
    public int GetHumanPlayers()
    {
        return HumanDropdown.value + 1;
    }

    /// <summary>
    /// Returns the number of AI players based on the dropdown selection.
    /// </summary>
    /// <returns>The number of AI players.</returns>
    public int GetAIPlayers()
    {
        return AIDropdown.value;
    }

    /// <summary>
    /// Retrieves the array of player names.
    /// </summary>
    /// <returns>An array of player names.</returns>
    public string[] GetPlayerNames()
    {
        return playerNames;
    }

    /// <summary>
    /// Retrieves the array of player colors.
    /// </summary>
    /// <returns>An array of Color32 representing player colors.</returns>
    public Color32[] GetPlayerColours()
    {
        return playerColours;
    }

    /// <summary>
    /// Loads the game scene, finalizing the setup process.
    /// </summary>
    private void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

