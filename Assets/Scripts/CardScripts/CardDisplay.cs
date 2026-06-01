using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardScripts;

public class CardDisplay : MonoBehaviour
{
    public Card cardData; //Where we get our card data from

    //variables, these are connected to UI elements in the inspector
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text energyCostText;
    [SerializeField] private TMP_Text cardText;
    [SerializeField] private Image[] typeImages; //Array of images for the 5 card types (fish, claw, etc)

    void Start()
    {
        UpdateCardDisplay(); //when this card initializes (called at start), it will update the display
    }

    public void UpdateCardDisplay()
    {
        //.text grabs the text component of the TMP_Text
        //here we take our card data and set our variables, which connect to UI elements
        nameText.text = cardData.cardName;
        energyCostText.text = cardData.energyCost.ToString();
        cardImage.sprite = cardData.cardSprite;
        cardText.text = cardData.cardText;

        //updating type images
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
