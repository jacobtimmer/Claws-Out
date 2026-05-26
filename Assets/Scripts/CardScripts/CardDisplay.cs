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
        cardImage.sprite = cardData.cardSprite;
        cardText.text = cardData.cardText;

        //update type images
        //turn all type images off first
        for (int i = 0; i < typeImages.Length; i++)
        {
            typeImages[i].gameObject.SetActive(false);
        }

        //Turn on the correct type images
        foreach (Card.CardType type in cardData.cardType)
        {
            int typeIndex = (int)type;

            if (typeIndex >= 0 && typeIndex < typeImages.Length)
            {
                typeImages[typeIndex].gameObject.SetActive(true);
            }
        }
    }

}
