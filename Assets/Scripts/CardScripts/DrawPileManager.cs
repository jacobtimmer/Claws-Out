using CardScripts;
using ClawsOut;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();

    private int currentIndex = 0;

    [SerializeField] private int maxHandSize = 10;

    public int currentHandSize;

    private HandManager handManager;
    private DiscardManager discardManager;

    public TextMeshProUGUI drawPileCounter;

    private void Start()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();

        currentIndex = 0;
        UpdateDrawPileCount();
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
        drawPile.Clear();

        if (cardsToAdd != null)
        {
            drawPile.AddRange(cardsToAdd);
        }

        Utility.Shuffle(drawPile);
        currentIndex = 0;
        UpdateDrawPileCount();
    }

    public void BattleSetup(int numberOfCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;

        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (handManager == null)
        {
            Debug.LogWarning("Cannot draw card because HandManager is missing.");
            return;
        }

        currentHandSize = handManager.cardsInHand.Count;

        if (currentHandSize >= maxHandSize)
        {
            return;
        }

        if (drawPile.Count == 0)
        {
            RefillDeckFromDiscard();
        }

        if (drawPile.Count == 0)
        {
            Debug.LogWarning("Cannot draw card because draw pile and discard pile are both empty.");
            UpdateDrawPileCount();
            return;
        }

        currentIndex = Mathf.Clamp(currentIndex, 0, drawPile.Count - 1);

        Card nextCard = drawPile[currentIndex];
        handManager.AddCardToHand(nextCard);
        drawPile.RemoveAt(currentIndex);

        if (drawPile.Count > 0)
        {
            currentIndex %= drawPile.Count;
        }
        else
        {
            currentIndex = 0;
        }

        UpdateDrawPileCount();
    }

    private void UpdateDrawPileCount()
    {
        if (drawPileCounter != null)
        {
            drawPileCounter.text = drawPile.Count.ToString();
        }
    }

    private void RefillDeckFromDiscard()
    {
        if (discardManager == null)
        {
            discardManager = FindAnyObjectByType<DiscardManager>();
        }

        if (discardManager != null && discardManager.discardCardsCount > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utility.Shuffle(drawPile);
            currentIndex = 0;
        }
    }
}