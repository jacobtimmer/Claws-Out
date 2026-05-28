using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 20; //maximum health this character can have
    public int currentHealth; //current health this character has

    public int armor; //armor blocks damage before health is reduced

    public Slider healthSlider; //optional health slider for UI

    private void Awake()
    {
        currentHealth = maxHealth; //start at full health
        UpdateHealthUI(); //update the health bar at the start
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        int damageAfterArmor = Mathf.Max(damage - armor, 0); //damage left after armor blocks some of it

        armor -= damage; //reduce armor by the incoming damage amount
        armor = Mathf.Max(armor, 0); //make sure armor does not go below 0

        currentHealth -= damageAfterArmor; //take leftover damage from health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //keep health between 0 and max health

        UpdateHealthUI(); //update health bar after taking damage

        Debug.Log(gameObject.name + " took " + damageAfterArmor + " damage.");

        if (currentHealth <= 0)
        {
            Debug.Log(gameObject.name + " died.");
        }
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth += amount; //increase health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //don't let health go above max health

        UpdateHealthUI(); //update health bar after healing

        Debug.Log(gameObject.name + " healed " + amount + " health.");
    }

    public void GainArmor(int amount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        armor += amount; //increase armor

        Debug.Log(gameObject.name + " gained " + amount + " armor.");
    }

    public void ResetArmor()
    {
        armor = 0; //remove armor
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

        }
    }
}