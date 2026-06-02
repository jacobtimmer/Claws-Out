using CardScripts;
using ClawsOut;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();


    [SerializeField] private int startingHandSize = 5;
    private int currentIndex = 0;
    [SerializeField] private int maxHandSize = 10;
    public int currentHandSize;
    private HandManager handManager;
    private DiscardManager discardManager;
    public TextMeshProUGUI drawPileCounter;

    private void Start()
    {
        handManager = FindAnyObjectByType<HandManager>();
    }

    private void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void MakeDrawPile(List<Card> cardsToAdd)
    {
        drawPile.AddRange(cardsToAdd);
        Utility.Shuffle(drawPile);
        UpdateDrawPileCount();
    }

    public void BattleSetup(int numberOfCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
        for(int i = 0; i < numberOfCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            RefilDeckFromDiscard();
        }

        if (currentHandSize < maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);
            if(drawPile.Count > 0)
            {
                currentIndex %= drawPile.Count; //sets current Index
            }
        }
    }

    private void UpdateDrawPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }

    private void RefilDeckFromDiscard()
    {
        if(discardManager == null)
        {
            discardManager = FindAnyObjectByType<DiscardManager>();
        }

        if(discardManager != null && discardManager.discardCardsCount > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utility.Shuffle(drawPile);
            currentIndex = 0;
        }
    }
}
