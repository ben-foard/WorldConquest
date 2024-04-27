using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{   
    public static MenuController Instance;
    //The 4 screens that are shown in the menu
    [SerializeField] private GameObject StartScreen;    
    [SerializeField] private GameObject PlayerScreen;
    [SerializeField] private GameObject NameSelectScreen;
    [SerializeField] private GameObject ColourSelectScreen;

    //The UI elements of all the canvas'
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Dropdown HumanDropdown;
    [SerializeField] private Dropdown AIDropdown;
    [SerializeField] private Button confirmPlayerButton;
    [SerializeField] private List<Button> colourButtons;
    [SerializeField] private InputField nameInput;
    [SerializeField] private TextMeshProUGUI nameSelectText;
    [SerializeField] private TextMeshProUGUI errorPlayerText;
    [SerializeField] private TextMeshProUGUI errorNameText;
    [SerializeField] private TextMeshProUGUI colourSelectText;
    [SerializeField] private Button confirmNameButton;

    //Private variables used to start the game in the game manager
    private List<Color32> gameColours = new List<Color32> { 
        new Color32(0,199,240,255),
        new Color32(0,7,192,255),  
        new Color32(90,192,0,255),
        new Color32(154,0,141,255), 
        new Color32(255,101,0,255),
        new Color32(190,18,0,255)};
    private int amountOfPlayers;
    private string[] playerNames;
    private Color32[] playerColours;
    private bool nameConfirmed = false;
    private bool colourConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartScreen.SetActive(true);
        startButton.onClick.AddListener(SelectAmountOfPlayer);
        settingsButton.onClick.AddListener(SelectSettings);
    }

    //temp method to check if settings button works
    private void SelectSettings() 
    {
        print("Clicked settings");
    }

    //Method to select amount of players that you want
    private void SelectAmountOfPlayer()
    {
        StartScreen.SetActive(false);
        PlayerScreen.SetActive(true);

        confirmPlayerButton.onClick.AddListener(EnterPlayerName);
    }

    //Method to show the enter player name screen
    private void EnterPlayerName()
    {   
        //Error checking for the amont of players
        amountOfPlayers = (GetHumanPlayers()) + GetAIPlayers();
        if (amountOfPlayers > 6 || amountOfPlayers < 2) {
            errorPlayerText.text = "ERROR: Please select 2-6 players!";
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

    //A routine to go through the amount of human players and set their name
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

        //Goes to the select colour screen once finished
        StartCoroutine(SelectColour());
    }

    //Wits until the the confirm button is pressed to go to next player
    private IEnumerator WaitForNameInput()
    {
       confirmNameButton.onClick.AddListener(() => {StartCoroutine(ConfirmNameInput());});
       while (!nameConfirmed){
            yield return null;
       }
       nameConfirmed = false;
    }

    //Checks if the name is valid, and returns true if so
    private IEnumerator ConfirmNameInput()
    {   
        nameConfirmed = true;
        string name = nameInput.text.Trim(' ').ToLower();
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

    //Method to show the select colour screen
    private IEnumerator SelectColour()
    {
        NameSelectScreen.SetActive(false);
        ColourSelectScreen.SetActive(true);
        playerColours = new Color32[amountOfPlayers];
        //Goes through all the human players
        for (int i = 0; i < GetHumanPlayers(); i++)
        {
            colourSelectText.text = playerNames[i] + " select your game colour!";  
            yield return WaitForColourSelection(i);
        }
        LoadGameScene();
    }

    //Wais until the player has selected a colour
    private IEnumerator WaitForColourSelection(int playerIndex)
    {

        for (int i = 0; i < colourButtons.Count; i++)
        {
            int index = i;
            colourButtons[i].onClick.RemoveAllListeners();
            colourButtons[i].onClick.AddListener(() => { SetColour(playerIndex, index); });
                
        }
        //goes through the loop until a colour is selected
        while (!colourConfirmed)
        {
            yield return null;
        }
        colourConfirmed = false;
    }

    //Adds the colour to each of the players colour value 
    private void SetColour(int playerIndex, int colourIndex)
    {
        playerColours[playerIndex] = gameColours[colourIndex];
        //Disables the colour button if its already been selected
        colourButtons[colourIndex].gameObject.SetActive(false);
        colourConfirmed = true;
    }
    //Methods for returning the values to the GameManager
    public int GetHumanPlayers(){
        return HumanDropdown.value + 1;
    }
    public int GetAIPlayers(){
        return AIDropdown.value;
    }
    public string[] GetPlayerNames(){
        return playerNames;
    }
    public Color32[] GetPlayerColours(){
        return playerColours;
    }

    //Final method to start the game 
    private void LoadGameScene()
    {        
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}

