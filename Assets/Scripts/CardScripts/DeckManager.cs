using System.Collections.Generic;
using UnityEngine;
using CardScripts;

public class DeckManager : MonoBehaviour
{
    //list of all cards in the deck, this will be populated from the resources folder
    public List<Card> allCards = new List<Card>();

    [SerializeField] private int startingHandSize = 5;
    //set hand size inside this script
    [SerializeField] private int maxHandSize = 10;
    public int currentHandSize;
    private HandManager handManager;
    private DrawPileManager drawPileManager;
    private bool startBattleRun = true;

    private void Start()
    {
        //load all card assets from the resources folder into the deck, all possible cards
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);
    }

    private void Awake()
    {
        if(drawPileManager == null)
        {
            drawPileManager = FindAnyObjectByType<DrawPileManager>();
        }
        if (handManager == null)
        {
            handManager = FindAnyObjectByType<HandManager>();
        }
    }

    private void Update()
    {
        if (startBattleRun)
        {
            BattleSetup();
        }
    }

    public void BattleSetup()
    {
        handManager.BattleSetup(maxHandSize);
        drawPileManager.MakeDrawPile(allCards);
        drawPileManager.BattleSetup(startingHandSize, maxHandSize);
        startBattleRun = false;
    }
}