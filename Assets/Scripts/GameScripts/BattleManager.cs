using CardScripts;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private int playerEnergy = 3;
    [SerializeField] private int maxPlayerEnergy = 3;
    [SerializeField] private int cardsDrawnPerTurn = 4;

    [SerializeField] private FighterStats playerStats;
    [SerializeField] private FighterStats enemyStats;

    [SerializeField] private TextMeshProUGUI energyText;

    [SerializeField] private int enemyArmorGain = 12;
    [SerializeField] private int enemyBigAttackDamage = 18;
    private int enemyTurnCounter = 0;

    [SerializeField] private DrawPileManager drawPileManager;
    private HandManager handManager;
    private DiscardManager discardManager;
    [SerializeField] private RewardManager rewardManager;

    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private TextMeshProUGUI battleResultText;
    private bool battleEnded = false;

    private int clawDamageBonusThisTurn;
    private int clawDamageBonusThisFight;

    private bool doubleDamageThisFight;
    private bool fishHealOnFishPlayed;
    private bool fishDrawOnFishPlayed;
    private bool clawEnergyOnClawPlayed;
    private bool riskEnergyOnRiskPlayed;
    private bool drawOnSelfDamage;
    private bool halveSelfDamage;

    private void Awake()
    {
        handManager = FindAnyObjectByType<HandManager>();
        discardManager = FindAnyObjectByType<DiscardManager>();
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (battleResultText != null)
        {
            battleResultText.gameObject.SetActive(false);
        }

        // Do NOT call StartPlayerTurn() here.
        // DeckManager.BattleSetup() already draws the starting hand.
        playerEnergy = maxPlayerEnergy;
        UpdateEnergyUI();

        if (playerStats != null)
        {
            playerStats.ClearArmor();
            if (GameManager.Instance != null && GameManager.Instance.IsRunActive())
            {
                playerStats.SetCurrentHealth(GameManager.Instance.GetPlayerHealth());
            }
        }
    }

    private void CheckBattleEnd()
    {
        if (battleEnded)
        {
            return;
        }

        if (enemyStats.IsDead)
        {
            battleEnded = true;
            rewardManager.ShowRewards();
        }
        else if (playerStats.IsDead)
        {
            EndBattle("You lose!");
        }
    }

    private void EndBattle(string message)
    {
        battleEnded = true;

        if (battleResultText != null)
        {
            battleResultText.text = message;
            battleResultText.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void StartPlayerTurn()
    {
        playerEnergy = maxPlayerEnergy;
        UpdateEnergyUI();
        playerStats.ClearArmor();

        for (int i = 0; i < cardsDrawnPerTurn; i++)
        {
            drawPileManager.DrawCard(handManager);
        }

        Debug.Log("Player turn started. Energy: " + playerEnergy);
    }

    public void EndPlayerTurn()
    {
        clawDamageBonusThisTurn = 0;
        DiscardHand();
        enemyStats.ClearArmor();
        EnemyTurn();

        if (!battleEnded)
        {
            StartPlayerTurn();
        }
    }

    private void EnemyTurn()
    {
        enemyTurnCounter++;

        if (enemyTurnCounter % 3 == 0)
        {
            enemyAnimator?.SetTrigger("Attack");
            playerStats.TakeDamage(enemyBigAttackDamage);
            CheckBattleEnd();
            Debug.Log("Enemy used a big attack for " + enemyBigAttackDamage);
        }
        else
        {
            enemyStats.GainArmor(enemyArmorGain);
            Debug.Log("Enemy gained " + enemyArmorGain + " armor.");
        }
    }

    private void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energy: " + playerEnergy + " / " + maxPlayerEnergy;
        }
    }

    private void DiscardHand()
    {
        for (int i = handManager.cardsInHand.Count - 1; i >= 0; i--)
        {
            GameObject cardObject = handManager.cardsInHand[i];

            if (cardObject != null && cardObject.TryGetComponent(out CardDisplay cardDisplay))
            {
                discardManager.AddToDiscard(cardDisplay.cardData);
            }

            Destroy(cardObject);
        }

        handManager.cardsInHand.Clear();
        handManager.UpdateHandVisuals();
    }

    public bool TryPlayCard(Card card, GameObject cardObject)
    {
        if (card == null)
        {
            return false;
        }

        if (playerEnergy < card.energyCost)
        {
            Debug.Log("Not enough energy.");
            return false;
        }

        playerEnergy -= card.energyCost;
        UpdateEnergyUI();

        ResolveCardEffect(card);

        handManager.cardsInHand.Remove(cardObject);
        handManager.UpdateHandVisuals();
        Debug.Log(card.cardName + " brittle? " + card.isBrittleCard);

        if (!card.isBrittleCard)
        {
            discardManager.AddToDiscard(card); //brittle ones go poof
        }

        Destroy(cardObject);

        return true;
    }

    private void ResolveCardEffect(Card card)
    {
        if (HasType(card, Card.CardType.Fish))
        {
            if (fishHealOnFishPlayed) playerStats.Heal(1);
            if (fishDrawOnFishPlayed) DrawCards(1);
        }

        if (HasType(card, Card.CardType.Claw) && clawEnergyOnClawPlayed)
        {
            GainEnergy(1);
        }

        if (HasType(card, Card.CardType.Risk) && riskEnergyOnRiskPlayed)
        {
            GainEnergy(1);
        }

        for (int i = 0; i < card.timesActivated; i++)
        {
            int damageDealt = 0;

            if (TryResolveSpecialCard(card, ref damageDealt))
            {
                if (card.isaLifestealCard && damageDealt > 0)
                {
                    playerStats.Heal(damageDealt);
                }

                CheckBattleEnd();
                continue;
            }

            if (card.damageMax > 0)
            {
                int damage = Random.Range(card.damageMin, card.damageMax + 1);
                damageDealt += DealDamage(card, damage);
            }

            if (card.healthGain > 0)
            {
                playerStats.Heal(card.healthGain);
            }

            if (card.armorGain > 0)
            {
                playerStats.GainArmor(card.armorGain);
            }

            if (card.selfDamage > 0)
            {
                TakeSelfDamage(card.selfDamage);
            }

            if (card.isaLifestealCard && damageDealt > 0)
            {
                playerStats.Heal(damageDealt);
            }

            if (card.energyGained > 0)
            {
                playerEnergy += card.energyGained;
                UpdateEnergyUI();
            }

            if (card.cardsDrawn > 0)
            {
                for (int j = 0; j < card.cardsDrawn; j++)
                {
                    drawPileManager.DrawCard(handManager);
                }
            }

            switch (card.cardName)
            {
                case "Dream of Fish":
                    playerStats.Heal(5);
                    fishHealOnFishPlayed = true;
                    break;

                case "Fishy Thoughts":
                    fishDrawOnFishPlayed = true;
                    break;

                case "Manicure":
                    clawDamageBonusThisFight += 2;
                    break;

                case "Sharpen Claws":
                    clawDamageBonusThisTurn += 2;
                    break;

                case "Scratching Post":
                    clawEnergyOnClawPlayed = true;
                    break;

                case "LimitlessInspiration":
                    doubleDamageThisFight = true;
                    break;

                case "Painful Inspiration":
                    drawOnSelfDamage = true;
                    break;

                case "Prepared Mind":
                    halveSelfDamage = true;
                    break;

                case "Sunk Cost Fallacy":
                    riskEnergyOnRiskPlayed = true;
                    break;
            }

            CheckBattleEnd();
        }

        Debug.Log("Played card: " + card.cardName);
    }

    private bool HasType(Card card, Card.CardType type)
    {
        return card.cardType != null && card.cardType.Contains(type);
    }

    private void GainEnergy(int amount)
    {
        playerEnergy += amount;
        UpdateEnergyUI();
    }

    private void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            drawPileManager.DrawCard(handManager);
        }
    }

    private int GetModifiedDamage(Card card, int damage)
    {
        if (HasType(card, Card.CardType.Claw))
        {
            damage += clawDamageBonusThisTurn + clawDamageBonusThisFight;
        }

        if (doubleDamageThisFight)
        {
            damage *= 2;
        }

        return damage;
    }

    private int DealDamage(Card card, int damage)
    {
        int finalDamage = GetModifiedDamage(card, damage);
        enemyStats.TakeDamage(finalDamage);
        return finalDamage;
    }

    private void TakeSelfDamage(int amount)
    {
        if (amount <= 0) return;

        if (halveSelfDamage)
        {
            amount = Mathf.CeilToInt(amount / 2f);
        }

        playerStats.TakeDamage(amount);

        if (drawOnSelfDamage)
        {
            DrawCards(1);
        }
    }

    private bool TryResolveSpecialCard(Card card, ref int damageDealt)
    {
        switch (card.cardName)
        {
            case "Blackjack":
                if (Random.value < 0.5f)
                {
                    damageDealt += DealDamage(card, 21);
                    Debug.Log("Blackjack hit!");
                }
                else
                {
                    Debug.Log("Blackjack missed.");
                }
                return true;

            case "Coin Flip":
                if (Random.value < 0.5f)
                {
                    playerStats.Heal(15);
                    GainEnergy(2);
                    Debug.Log("Coin Flip won!");
                }
                else
                {
                    Debug.Log("Coin Flip lost.");
                }
                return true;

            case "Dollar Bill":
                if (Random.value < 0.5f)
                {
                    damageDealt += DealDamage(card, 1);
                    playerStats.Heal(1);
                    playerStats.GainArmor(1);
                    DrawCards(1);
                    Debug.Log("Dollar Bill hit!");
                }
                else
                {
                    Debug.Log("Dollar Bill missed.");
                }
                return true;

            case "At All Costs":
                TakeSelfDamage(30);
                damageDealt += DealDamage(card, 40);
                return true;
        }

        return false;
    }
}