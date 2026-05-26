using System.Collections.Generic;
using UnityEngine;

namespace CardScripts
{
    //a way to make this card data in the unity editor
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]

    public class Card : ScriptableObject
    {
        public string cardName;
        public List<CardType> cardType;
        public Sprite cardSprite;
        public int energyCost;
        //public int damageMin;
        //public int damageMax;
        //public int healthGain;
        //public int armorGain;
        //public DamageType damageType;

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
