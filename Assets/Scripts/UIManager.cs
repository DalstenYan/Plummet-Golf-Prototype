using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    private Stack<char> tallyChars;
    [SerializeField]
    private TextMeshProUGUI tallyText;

    private GameObject bobbingPlayerArrow;


    void Awake()
    {
        Instance = this;
        tallyChars = new Stack<char>();
        bobbingPlayerArrow = GameObject.Find("Indicator Arrow");
    }

    public void UpdateTallyStrokes() 
    {
        tallyText.text = string.Empty;

        if (tallyChars.Count == 0 || tallyChars.Peek() == 'e')
        {
            tallyText.alignment = TextAlignmentOptions.Bottom;
            tallyChars.Push('a');
        }
        else 
        {
            int charValue = tallyChars.Pop();
            tallyChars.Push((char)(charValue + 1));   
        }

        char[] charArr = tallyChars.ToArray();
        Array.Reverse(charArr);

        foreach (char c in charArr) 
        {
            tallyText.text += c;
        }
    }

    public void EnableDisableArrow(bool canLaunch) 
    {
        bobbingPlayerArrow.SetActive(canLaunch);
    }
}
