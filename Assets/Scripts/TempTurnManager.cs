using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CardScripts;

public class TurnManager : MonoBehaviour
{
    [Header("Characters")]
    public CharacterStats player;
    public CharacterStats enemy;

    [Header("Enemy Settings")]
    public int enemyDamage = 3;

    [Header("Game Over UI")]
    public GameObject gameOverScreen; //panel that appears when either the player or enemy reaches 0 health
    public TMP_Text gameOverText; //text that says whether the player won or lost

    private bool playerHasPlayedCard = false; //tracks if the player already played one card this turn
    private bool turnInProgress = false; //tracks if the enemy turn is currently happening
    private bool gameIsOver = false; //tracks if the game has ended

    private void Start()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false); //hide the game over screen when the game starts
        }
    }

    public bool CanPlayCard()
    {
        return !playerHasPlayedCard && !turnInProgress && !gameIsOver; //player can only play if game is still going
    }

    public void PlayCard(Card card)
    {
        if (!CanPlayCard())
        {
            return;
        }

        if (card == null)
        {
            Debug.LogWarning("Tried to play a card, but the Card ScriptableObject was missing.");
            return;
        }

        playerHasPlayedCard = true; //player has now used their one card for this turn

        ResolveCard(card); //apply damage, healing, or armor from this card

        if (enemy.IsDead())
        {
            EndGame(true); //enemy died, so player wins
            return;
        }

        StartCoroutine(EnemyTurn()); //after the player plays a card, enemy takes its turn
    }

    private void ResolveCard(Card card)
    {
        int damage = GetCardDamage(card); //get the damage value from the card

        if (damage > 0)
        {
            enemy.TakeDamage(damage); //deal damage to the enemy
            Debug.Log("Player used " + card.cardName + " and dealt " + damage + " damage.");
        }

        if (card.healthGain > 0)
        {
            player.Heal(card.healthGain); //heal the player
            Debug.Log("Player used " + card.cardName + " and healed " + card.healthGain + " health.");
        }

        if (card.armorGain > 0)
        {
            player.GainArmor(card.armorGain); //give armor to the player
            Debug.Log("Player used " + card.cardName + " and gained " + card.armorGain + " armor.");
        }
    }

    private int GetCardDamage(Card card)
    {
        if (card.damageMax <= 0)
        {
            return 0; //if the card has no damage, return 0
        }

        if (card.damageMin == card.damageMax)
        {
            return card.damageMin; //if min and max are the same, just use that number
        }

        return Random.Range(card.damageMin, card.damageMax + 1); //otherwise pick random damage between min and max
    }

    private IEnumerator EnemyTurn()
    {
        turnInProgress = true; //enemy turn is starting, so player should not be able to play cards

        yield return new WaitForSeconds(0.75f); //small delay so the card effect happens first

        if (!gameIsOver && enemy.currentHealth > 0)
        {
            player.TakeDamage(enemyDamage); //enemy deals damage to the player
            Debug.Log("Enemy dealt " + enemyDamage + " damage.");
        }

        if (player.IsDead())
        {
            EndGame(false); //player died, so player loses
            yield break;
        }

        yield return new WaitForSeconds(0.25f); //small delay before player can act again

        StartPlayerTurn(); //reset so the player can play another card
    }

    private void StartPlayerTurn()
    {
        if (gameIsOver)
        {
            return;
        }

        player.ResetArmor(); //clear the player's armor at the start of the next player turn

        playerHasPlayedCard = false; //player can play one card again
        turnInProgress = false; //enemy turn is done

        Debug.Log("Player turn started.");
    }

    private void EndGame(bool playerWon)
    {
        gameIsOver = true;
        turnInProgress = false;
        playerHasPlayedCard = true;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        if (gameOverText != null)
        {
            if (playerWon)
            {
                gameOverText.text = "Victory!";
            }
            else
            {
                gameOverText.text = "Game Over";
            }
        }

        Debug.Log(playerWon ? "Victory!" : "Game Over");
    }
}