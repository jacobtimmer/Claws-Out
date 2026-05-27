using UnityEngine;

public class GameManager : MonoBehaviour
{
    //setting up GameManager as a singleton, meaning there will only be one instance of it in the game, and it can be accessed from other scripts without needing a reference to it
    //will let us transfer data between scenes, and manage game state across the entire game
    public static GameManager Instance { get; private set; } //saying that this is a public variable, and it can be acessed from other scripts, but can only be set by the gamemanager script

    //player variables
    private int playerHealth;
    private int playerWealth;

    public AudioManager audioManager { get; private set; }
    public DeckManager deckManager { get; private set; }

    public OptionsManager optionsManager { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; //if there is no instance of the gamemanager, set it to this instance
            DontDestroyOnLoad(gameObject); //make sure the gamemanager persists across scene changes
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject); //if there is already an instance of the gamemanager, destroy this one to ensure there is only one instance
        }
    }

    private void InitializeManagers()
    {
        optionsManager = GetComponent<OptionsManager>();
        audioManager = GetComponent<AudioManager>();
        deckManager = GetComponent<DeckManager>();

        //audiomanager backup
        if(audioManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if(prefab == null)
            {
                Debug.Log($"AUdio Prefab not found!");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                audioManager = GetComponentInChildren<AudioManager>();
            }
        }
        //deckmanager backup
        if (deckManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/DeckManager");
            if (prefab == null)
            {
                Debug.Log($"Deck Prefab not found!");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                deckManager = GetComponentInChildren<DeckManager>();
            }
        }
        //optionsmanager backup
        if (optionsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (prefab == null)
            {
                Debug.Log($"options Prefab not found!");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                optionsManager = GetComponentInChildren<OptionsManager>();
            }
        }
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerWealth(int wealth)
    {
        playerWealth = wealth;
    }
    public int GetPlayerWealth()
    {
        return playerWealth;
    }


}
