using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject StartScreen;    
    [SerializeField] private GameObject PlayerScreen;
    [SerializeField] private GameObject NameSelectScreen;
    [SerializeField] private GameObject ColourSelectScreen;

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

    private Color32[] gameColours = new Color32[] {new Color32(21,21,31,21), new Color32(21,21,31,21),new Color32(21,21,31,21),new Color32(21,21,31,21),new Color32(21,21,31,21),new Color32(21,21,31,21)};
    private int amountOfPlayers;
    private string[] playerNames;
    private Color32[] playerColours;
    private bool nameConfirmed = false;
    private bool colourConfirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartScreen.SetActive(true);
        startButton.onClick.AddListener(SelectAmountOfPlayer);
        settingsButton.onClick.AddListener(SelectSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SelectSettings() 
    {
        print("Clicked settings");
    }
    private void SelectAmountOfPlayer()
    {
        StartScreen.SetActive(false);
        PlayerScreen.SetActive(true);

        confirmPlayerButton.onClick.AddListener(EnterPlayerName);
    }
    private void EnterPlayerName()
    {
        amountOfPlayers = (HumanDropdown.value + 1) + AIDropdown.value;
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
        StartCoroutine(EnterPlayerNamesCoroutine(HumanDropdown.value + 1));
    }
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
    private IEnumerator WaitForNameInput()
    {
       confirmNameButton.onClick.AddListener(() => {StartCoroutine(ConfirmNameInput());});
       while (!nameConfirmed){
            yield return null;
       }
       nameConfirmed = false;
    }
    private IEnumerator ConfirmNameInput()
    {   
        nameConfirmed = true;
        string name = nameInput.text.Trim(' ');
        if (name.Length > 20 || name.Length < 3) {
            errorNameText.text = "Your name must be between 3-20 characters!";
            nameConfirmed = false;
        }
        if (playerNames.Contains(name))
        {
            errorNameText.text = "Your name must be unique";
            nameConfirmed = false;
        }
        yield return new WaitForEndOfFrame();
    }
    private IEnumerator SelectColour()
    {
        NameSelectScreen.SetActive(false);
        ColourSelectScreen.SetActive(true);
        playerColours = new Color32[amountOfPlayers];
        for (int i = 0; i < HumanDropdown.value + 1; i++)
        {
            colourSelectText.text = playerNames[i] + " select your game colour!";  
            yield return WaitForColourSelection(i);
        }
        StartGame();
    }
    private IEnumerator WaitForColourSelection(int playerIndex)
    {

        while (!colourConfirmed)
        {
            for (int i = 0; i < colourButtons.Count - 1; i++)
            {
                int index = i;
               colourButtons[i].onClick.AddListener(() => { SetColour(playerIndex, index); });
                
            }
            
            yield return null;
        }
        colourConfirmed = false;
    }

    private void SetColour(int playerIndex, int colorIndex)
    {
        playerColours[playerIndex] = gameColours[colorIndex];
        colourButtons[colorIndex].gameObject.SetActive(false);
        colourConfirmed = true;
    }

    private void StartGame()
    {
        print("started");
    }
}

