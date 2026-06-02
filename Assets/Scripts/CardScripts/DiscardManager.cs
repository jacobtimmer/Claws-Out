using CardScripts;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscardManager : MonoBehaviour
{
    [SerializeField] public List<Card> discardCards = new List<Card>();

    public TextMeshProUGUI discardCount; //reference to TextMeshPro
    public int discardCardsCount;

    private void Awake()
    {
        UpdateDiscardCount();
    }

    private void UpdateDiscardCount()
    {
        discardCount.text = discardCards.Count.ToString(); //grabs data from our discard list
        discardCardsCount = discardCards.Count;
    }

    public void AddToDiscard(Card card)
    {
        if(card != null)
        {
            discardCards.Add(card);
            UpdateDiscardCount();
        }
    }

    //grabs and returns top card of discard pile
    public Card PullFromDiscard()
    {
        if(discardCards.Count > 0)
        {
            Card cardToReturn = discardCards[discardCardsCount - 1];
            discardCards.RemoveAt(discardCardsCount - 1);
            UpdateDiscardCount();
            return cardToReturn;
        }
        else
        {
            return null;
        }
    }

    //removes specific card from discard, might be errors for multiple instances of cards, will test
    public bool PullSelectCardFromDiscard(Card card)
    {
        if(discardCards.Count > 0 && discardCards.Contains(card))
        {
            discardCards.Remove(card);
            UpdateDiscardCount();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Card> PullAllFromDiscard()
    {
        if(discardCards.Count > 0)
        {
            List<Card> cardsToReturn = new List<Card>(discardCards);
            discardCards.Clear();
            UpdateDiscardCount();
            return cardsToReturn;
        }
        else
        {
            Debug.Log("nothing in discard?");
            return new List<Card>();
        }
    }

}
