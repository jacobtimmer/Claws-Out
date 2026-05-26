using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardScripts;

public class CardDisplay : MonoBehaviour
{
    public Card cardData; //Where we will assign card data

    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text energyCostText;
    public TMP_Text cardText;
    public Image[] typeImages; //Array of images for the 5 card types

    void Start()
    {
        UpdateCardDisplay(); //when this card initializes, it will update the display
    }

    public void UpdateCardDisplay()
    {
        //.text grabs the text component of the TMP_Text
        nameText.text = cardData.cardName;
        energyCostText.text = cardData.energyCost.ToString();
        //cardImage.sprite = cardData.cardSprite;
    }

}
