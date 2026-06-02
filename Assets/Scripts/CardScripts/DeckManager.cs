using System.Collections.Generic;
using UnityEngine;
using CardScripts;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private int startingHandSize = 5;

    //list of all cards in the deck, this will be populated from the resources folder
    public List<Card> allCards = new List<Card>();
    private int currentIndex = 0;

    private void Start()
    {
        //load all card assets from the resources folder into the deck, all possible cards
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);

        HandManager handManager = FindAnyObjectByType<HandManager>();
        for(int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if(allCards.Count == 0)
        {
            return;
        }

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count; //wraps around to beginning of deck when it reaches the end
    }

    private void Awake()
    {
        Debug.Log($"I am a deck manager and I am awake!");
    }
}
