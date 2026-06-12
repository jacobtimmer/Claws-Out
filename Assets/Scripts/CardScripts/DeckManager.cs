using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CardScripts;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    [SerializeField] private int startingHandSize = 5;
    [SerializeField] private int maxHandSize = 10;

    public int currentHandSize;

    private HandManager handManager;
    private DrawPileManager drawPileManager;

    private bool startBattleRun = true;

    private void Awake()
    {
        FindBattleManagers();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LoadDeck();
    }

    private void Update()
    {
        if (startBattleRun)
        {
            BattleSetup();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This runs every time a new scene loads or restarts.
        // Since DeckManager persists through DontDestroyOnLoad,
        // we need to reset these references for the new scene.
        FindBattleManagers();

        // Let the deck rebuild when entering/restarting a battle scene.
        startBattleRun = true;
    }

    private void FindBattleManagers()
    {
        drawPileManager = FindAnyObjectByType<DrawPileManager>();
        handManager = FindAnyObjectByType<HandManager>();
    }

    private void LoadDeck()
    {
        allCards.Clear();

        if (GameManager.Instance != null && GameManager.Instance.HasRunDeck())
        {
            allCards.AddRange(GameManager.Instance.GetRunDeckCopy());
        }
        else
        {
            Card[] cards = Resources.LoadAll<Card>("Cards");
            allCards.AddRange(cards);
        }
    }

    public void BattleSetup()
    {
        if (handManager == null || drawPileManager == null)
        {
            FindBattleManagers();
            return;
        }

        LoadDeck();

        handManager.BattleSetup(maxHandSize);
        drawPileManager.MakeDrawPile(allCards);
        drawPileManager.BattleSetup(startingHandSize, maxHandSize);

        startBattleRun = false;
    }
}