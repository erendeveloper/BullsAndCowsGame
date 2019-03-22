using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

//loads values from the "bcwTreeXml" XML file.It contains tree created with crushBullsCows algorithm.
//Tree gets result up to 7 turns
//For more info http://slovesnov.users.sourceforge.net/index.php?bullscows

public class LoadXmlFile : MonoBehaviour
{
    public TextAsset xmlRawFile;                                          //XML file to assign
    private XmlNode currentNode;                                          //current node, updates on every turn
    private string currentGuess = "";                                     //Current guess seected from tree
    private List<string> availableResults = new List<string>();           //available hints to check on player cheats

    // Start is called before the first frame update
    void Start()
    {
        createRootNode();  //creates top node of tree                                  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string guess(string hintString)   //select guess from tree
    {
        getChildNode(hintString);    
        return currentGuess;

    }

    private void getChildNode(string hintString)   //creates children nodes
    {
        XmlNodeList newNodesList = currentNode.ChildNodes; //children nodes of current node
        foreach(XmlNode newNode in newNodesList)
        {
            if (newNode.Attributes["hint"].Value == hintString)  //select child node from tree by hints
            {
                currentNode.RemoveAll();  //removes current node and assign new node
                currentNode = newNode;
                currentGuess=currentNode.Attributes["guess"].Value;  //gets guess value from "guess" attribute on XML
                assignAvailableResults();  //assigns available hint results
            }
        }
        
    }

    private void createRootNode()   //creates top node of tree
    {
        string data = xmlRawFile.text;                                    //XML file data
        XmlDocument xmlDoc = new XmlDocument();                           //instantiates new XmlDocument
        xmlDoc.Load(new StringReader(data));                              //loads XML data
        currentNode = xmlDoc.GetElementsByTagName("ul")[0].ChildNodes[0]; //top node of tree
        currentGuess = currentNode.Attributes["guess"].Value;             //value of guess attribute on XML
        assignAvailableResults();                                         //assigns available hint results
    }

    private void assignAvailableResults() //assigns available hint results
    {
        availableResults.Clear();               //clears list, new will be added
        if (currentNode.HasChildNodes)          //checks if  it last item in tree
        {
            XmlNodeList resultList = currentNode.ChildNodes;
            for (int i = 0; i < resultList.Count; i++)
            {
                availableResults.Add(resultList[i].Attributes["hint"].Value);
            }
            Debug.Log("available " + availableResults[0]);
        }
        else                                  //gets last item in tree
        {
            availableResults.Add("4.0");      //PC wins.Hints must +4 
        }
    }
    public bool checkAvailableResults(string value)  //checking available hint options to prevent players to cheat
    {
        for (int i = 0; i < availableResults.Count; i++)
        {
            if (availableResults[i] == value)
                return true;
        }
        return false;  //player is cheating
    }
    public string getCurrentGuess()   //for Number scriptto access currentGuess
    {
        return currentGuess;
    }
}
