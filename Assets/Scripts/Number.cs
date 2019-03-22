using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Main script of the game
//It manages PC and Player guesses and checking if there is any error on play
//PC gets numbers from "bcwTreeXml" XML File with the script "loadXmlFile"

public class Number : MonoBehaviour
{
    private const int MaxDigits = 4; //max number of digits

    private int[] PCGuessNumber = new int[MaxDigits]; //the number PC guesses
    private int[] PcPickNumber = new int[MaxDigits]; //the number PC picks at the start of the game

    private int positiveHintValue = 0; //number of digits which are in the right positions
    private int negativeHintValue = 0; //number of digits which are in the wrong positions

    //Text UI's
    public Text PCGuessText;           //PC guess number text
    public Text HintPositiveText;      //positive hint input
    public Text HintNegativeText;      //negative hint input
    public Text WarningText1;          //warning in positive hint values and PC wins text
    public Text WarningText2;          //warning on negative hint values
    public Text PlayerGuessText;       //Text on input field player guesses
    public Text ResultText;

    private bool ContinueGame = false; //waits for showing results
    private bool GameOver = false;


    private bool FirstPCGuess = true;  //First guess of PC

    // Start is called before the first frame update
    void Start()
    {
        pickNumber();                  //PC picks numbers
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Guess()
    {
        string PcGuessNumberString = "";
        if (FirstPCGuess == true)     //first turn of the game
        {
            PcGuessNumberString = this.gameObject.GetComponent<LoadXmlFile>().getCurrentGuess();        //calling LoadXmlFile to guess
            PCGuessText.text = "Is This Your Number: "+ PcGuessNumberString;
            FirstPCGuess = false;

        }
        else                         //other turns
        {
            string hintString = positiveHintValue.ToString() + "." + (-negativeHintValue).ToString();
            PcGuessNumberString = this.gameObject.GetComponent<LoadXmlFile>().guess(hintString);       //calling LoadXmlFile to guess
            PCGuessText.text = "Is This Your Number: " + PcGuessNumberString;

        }
        for(int i=0; i<MaxDigits; i++)    //assigning guess numbers from string to array
        {
            PCGuessNumber[i] = (int)char.GetNumericValue(PcGuessNumberString[i]);      
        }
        
    }
    private void pickNumber()  //PC picks numbers
    {
        List<int> numberList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };   //all numbers list
        int randomNumberIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            randomNumberIndex = Random.Range(0, numberList.Count);  //select random index
            PcPickNumber[i] = numberList[randomNumberIndex];        //select random number
            numberList.RemoveAt(randomNumberIndex);                 //remove selected number from list, digits must be unique
        }
    }
    private void useHints()
    {
        if (positiveHintValue == 4)
        {
            WarningText1.text = "PC wins.Click Enter to play Again";
            GameOver = true;
        }
        else if (positiveHintValue == 0 && negativeHintValue == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                //UsableNumbersList.Remove(PCGuessNumber[i]);
                //NonExistingNumbers.Add(PCGuessNumber[i]);
            }
        }

        if(GameOver==false)
        this.gameObject.GetComponent<UIManager>().changeTurn();

    }


    public void checkHintErrors() //checks if there is any error on hints hgich playergives
    {
        bool noHintError = true;       //on any error, it will be true
        WarningText1.text = "";      
        WarningText2.text = "";

        int positiveValue = 1000;      //just selecting different number to check error
        int negativeValue = 1000;      

        string positiveText = HintPositiveText.text;         //hints value on input field
        string negativeText = HintNegativeText.text;



        if (positiveText == "")     //empty input field means 0
        {
            positiveValue = 0;
        }
        else if (positiveText.Length == 2)    //there must be {-} or null
        {
            if (positiveText[0] == '+')
            {
                bool isNumber = System.Char.IsNumber(positiveText[1]); //checking if numbers or else
                if (isNumber == false)
                {
                    WarningText1.text = "Positive hint must be number between +1 and +4";
                    noHintError = false;

                }
                else if (isNumber == true)
                {
                    int numberValue = (int)char.GetNumericValue(positiveText[1]);  //selects int item from string
                    if (0 < numberValue && numberValue <= 4)
                    {
                        positiveValue = (int)char.GetNumericValue(positiveText[1]); //selects int item from string
                    }
                    else
                    {
                        WarningText1.text = "Positive hint must be between +1 and +4";
                        noHintError = false;
                    }
                }
            }
            else // no {+} character
            {
                WarningText1.text = "You have to put '+' character on positive hint";
                noHintError = false;
            }
        }
        else
        {
            WarningText1.text = "Positive hint must be empty or number between +1 and +4";
            noHintError = false;
        }


        if (negativeText == "")  //empty input field means 0
        {
            negativeValue = 0;
        }
        else if (negativeText.Length == 2)  //there must be {-} or null
        {
            if (negativeText[0] == '-')
            {
                bool isNumber = System.Char.IsNumber(negativeText[1]);  //checking if numbers or else
                if (isNumber == false)
                {
                    WarningText2.text = "Negative hint must be number between -4 and -1";
                    noHintError = false;
                }
                else if (isNumber == true)
                {
                    int numberValue = -(int)char.GetNumericValue(negativeText[1]);       //selects int item from string
                    if (-4 <= numberValue && numberValue < 0)
                    {
                        negativeValue = numberValue;
                    }
                    else
                    {
                        WarningText2.text = "Negative hint must be between -4 and -1";   //selects int item from string
                        noHintError = false;
                    }
                }
            }
            else  // no {-} character
            {
                WarningText2.text = "You have to put '-' character on negative hint";
                noHintError = false;
            }
        }
        else
        {
            WarningText2.text = "Negative hint must be empty or number between -4 and -1";
            noHintError = false;
        }

        string hintString = positiveValue.ToString() + "." + (-negativeValue).ToString();  //converting positive and negative hints to string

        if (positiveValue != 1000 && negativeValue != 1000)   //selected just different numbers to check errors if values are not changed
        {

            if ((positiveValue + (-negativeValue) > MaxDigits))  //hints totals can't be bigger than max digits 4
            {
                WarningText1.text = "Wrong hint numbers";
                noHintError = false;
            }
            //checking available hint options,
            else if (this.gameObject.GetComponent<LoadXmlFile>().checkAvailableResults(hintString)==false && hintString!="4.0")
            {
                WarningText1.text = "Don't cheat";   //impossible to give this hints
                noHintError = false;
            }
            else
            {
                positiveHintValue = positiveValue;   //correct
                negativeHintValue = negativeValue;
            }

        }
        if (noHintError == true)  //no hint error,
            useHints();           //hints can be used
    }

    public void checkPlayerGuess() //checking Player's guess numbers
    {
        string playerInput = PlayerGuessText.text;  //getting guess from input field

        //checks if guess has 4 unique digits
        if (!playerInput.All(System.Char.IsDigit) || playerInput.Length != 4 || !uniqueCharacters(playerInput))
        {
            ResultText.text = "You must give 4 unique numbers";
        }
        else  //they are 4 unique digits
        {
            int positiveValue = 0;  //hint values to be calculated
            int negativeValue = 0;

            //calculating hint values of PC
            for(int i=0; i<4; i++)
            {               
                if (PcPickNumber[i].ToString() == playerInput[i].ToString())   //if right digits are on right positions
                    positiveValue++;
                else if (playerInput.Contains(PcPickNumber[i].ToString()))     //if right digits are on wrong positions
                    negativeValue++;
            }

            if (positiveValue == 0 && negativeValue == 0)      //empty means 0
                ResultText.text = "Result:";
            else if (positiveValue != 0 && negativeValue == 0)   //only positive hint
                ResultText.text = "Result: +"+positiveValue;
            else if (positiveValue == 0 && negativeValue != 0)   //only negative hint
                ResultText.text = "Result: -"+negativeValue; 
            else if (positiveValue != 0 && negativeValue != 0)   //positive and negative hints together
                ResultText.text = "Result: +" + positiveValue+" -"+negativeValue;

            if (positiveValue == 4)  //You win
            {
                ResultText.text += ".You win.Click Enter to play Again";
                GameOver = true;  //Enter button will start the game again
            }
            else
            {
                ContinueGame = true;  //Enter button firstly showed hint values.2nd time it will change turn
                ResultText.text += " .Click Enter to continue";
            }
        }
    }
    bool uniqueCharacters(string str)  // checks if string has unique characters
    {

        // If at any time we encounter 2 same characters, return false 
        for (int i = 0; i < str.Length; i++)
            for (int j = i + 1; j < str.Length; j++)
                if (str[i] == str[j])
                    return false;

        // If no duplicate characters encountered, return true 
        return true;
    }

    public void GuessButton()    //Enter Button to guess
    {
        if (GameOver == true)
            SceneManager.LoadScene(0);  //starts the game again
        if (ContinueGame == true)
        {
            ContinueGame = false; //if false,Enter button shows the hints, else changes turns
            this.gameObject.GetComponent<UIManager>().changeTurn();
            Guess();
        }
        else
        {
            checkPlayerGuess();
        }
    }
    public void GiveHintsButton()  //Other Enter button to give hints
    {
        if (GameOver == true)
        {
            SceneManager.LoadScene(0);  //starts the game again
        }

         checkHintErrors();
        
    }
}
