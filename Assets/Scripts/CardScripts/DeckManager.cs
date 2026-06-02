using System.Collections.Generic;
using UnityEngine;
using CardScripts;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private int startingHandSize = 5;
    [SerializeField] private int maxHandSize = 12;
    public int currentHandSize;

    //list of all cards in the deck, this will be populated from the resources folder
    public List<Card> allCards = new List<Card>();
    private int currentIndex = 0;
    private HandManager handManager;

    private void Start()
    {
        //load all card assets from the resources folder into the deck, all possible cards
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);

        handManager = FindAnyObjectByType<HandManager>();
        maxHandSize = handManager.maxHandSize;
        for(int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }

    private void Update()
    {
        if(handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if(allCards.Count == 0)
        {
            return;
        }

        if (currentHandSize < maxHandSize)
        {
            Card nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count; //wraps around to beginning of deck when it reaches the end
        }
    }



    private void Awake()
    {
        Debug.Log($"I am a deck manager and I am awake!");
    }
}
