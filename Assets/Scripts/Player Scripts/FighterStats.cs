using TMPro;
using UnityEngine;

public class FighterStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;

    private int currentHealth;
    private int armor;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        int damageAfterArmor = Mathf.Max(0, amount - armor);
        armor = Mathf.Max(0, armor - amount);
        currentHealth = Mathf.Max(0, currentHealth - damageAfterArmor);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Debug.Log(gameObject.name + " died!?!?");
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount); //either maxHealth or healed health, smaller one
        UpdateUI();
    }

    public void GainArmor(int amount)
    {
        armor += amount;
        UpdateUI();
    }

    public void ClearArmor()
    {
        armor = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }

        if (armorText != null)
        {
            armorText.text = armor.ToString();
        }
    }
}