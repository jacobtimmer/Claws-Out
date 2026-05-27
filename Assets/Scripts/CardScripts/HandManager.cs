using UnityEngine;
using CardScripts;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

public class HandManager : MonoBehaviour
{
    [SerializeField] int startingHandSize = 5;

    public DeckManager deckManager; //Assign the DeckManager in the inspector
    public GameObject cardPrefab; //Assign card prefab in inspector

    public Transform handTransform; //Center of the hand position
    public float fanSpread = -7.5f; //Degrees to spread cards in a fan shape
    public float cardSpacing = 150f;
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>(); //List to keep track of card objects in hand
    void Start()
    {

    }

    private void FixedUpdate()
    {
        //UpdateHandVisuals(); //can be used for testing
    }

    //public so we can call it elsewhere
    public void AddCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard); //adds the card to our list

        //set the card data of the instantiated card to the data from the deck
        CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>(); //added so we can update the card display after assigning card data

        if (cardDisplay != null)
        {
            cardDisplay.Setup(cardData); //changed from directly setting cardData so the UI updates right away
        }
        else
        {
            Debug.LogWarning("Card prefab does not have a CardDisplay script."); //added warning if the prefab is missing CardDisplay
        }

        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count; //length of the list

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            //fanspread calculation, result depends on position in list
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            //spacing
            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalizes position to range from -1 to 1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);
            //set card positions
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }

}