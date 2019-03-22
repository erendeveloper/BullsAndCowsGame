using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//It manages UI
public class UIManager : MonoBehaviour
{
    private bool isPCPlaying = true;             //PC playing
    public GameObject PCUI;                      //PC playing UI
    public GameObject PlayerUI;                  //Player playing UI
    public GameObject StartButton;               //Start Button object
    public InputField PCGuessText;               //PC Guess Input field text
    public InputField HintPositiveText;          //Positive hint input field text
    public InputField HintNegativeText;          //Negative hint input field text
    public Text WarningText1;                    //warning in positive hint values and PC wins text
    public Text WarningText2;                    //warning on negative hint values
    public Text PlayerGuessText;                 //Text on input field player guesses
    public Text ResultText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void changeTurn()  //changes turn between PC and Player
    {
        if (isPCPlaying == true)             //changes turn from PC to player
        {
            isPCPlaying = false;
            PCUI.gameObject.SetActive(false);              //Closes PC UI
            PlayerUI.gameObject.SetActive(true);           //Opens Palyer UI
            PlayerGuessText.text = "";
            ResultText.text = "";
        }
        else if (isPCPlaying == false)       //changes turn from Player to PC
        {
            isPCPlaying = true;
            PCUI.gameObject.SetActive(true);               //Opens PC UI
            PlayerUI.gameObject.SetActive(false);          //Closes Player UI
            PCGuessText.text = "";
            HintPositiveText.text = "";                    //Removing input fileds
            HintNegativeText.text = "";
            WarningText1.text = "";
            WarningText2.text = "";
        }
    }
    public void StartGameButton()            //Start button
    {
        StartButton.gameObject.SetActive(false);           //disactivates button
        PCUI.gameObject.SetActive(true);                   //Opens PC UI
        this.gameObject.GetComponent<Number>().Guess();    //First guess in the game
    }
}
