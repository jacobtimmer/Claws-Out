using UnityEngine;

namespace CardScripts
{
    public class Card : ScriptableObject
    {
        public string cardName;
        public CardType cardType;
        public int health;
        public int damageMin;
        public int damageMax;
        //public DamageType damageType;

        public enum CardType
        {
            Basic,
            Claw,
            Fish
        }

        /*public enum DamageType
        {
            Physical,
            Magical,
            True
        }*/
    }
}
