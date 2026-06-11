using System.Collections.Generic;
using UnityEngine;

namespace CardScripts
{
    //a way to make this card data in the unity editor
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]

    public class Card : ScriptableObject
    {
        public string cardName;
        public string cardText;
        public List<CardType> cardType;
        public Sprite cardSprite;
        public int energyCost;
        public int damageMin;
        public int damageMax;
        public int healthGain;
        public int armorGain;
        public int timesActivated = 1;
        public bool isBrittleCard;
        public bool isaLifestealCard;
        public int cardsDrawn;
        public int energyGained;
        public int selfDamage;
        public AudioSource cardSoundEffectPlayer;
        public AudioClip cardSoundEffect;

        public enum CardType
        {
            Basic,
            Claw,
            Fish,
            Risk,
            Magic
        }
    }
}
