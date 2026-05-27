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
        if (cardData != null)
        {
            UpdateCardDisplay(); //when this card initializes, it will update the display
        }
    }

    public void Setup(Card newCard) //added so the HandManager can assign card data when drawing cards
    {
        cardData = newCard; //set this card display's data to the card that was drawn
        UpdateCardDisplay(); //update the display after assigning the new card data
    }

    public void UpdateCardDisplay()
    {
        if (cardData == null) //added so we don't get errors if cardData was not assigned yet
        {
            Debug.LogWarning("No card data assigned to this CardDisplay.");
            return;
        }

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