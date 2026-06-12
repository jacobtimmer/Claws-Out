using System.Collections.Generic;
using UnityEngine;

namespace CardScripts
{
    [CreateAssetMenu(fileName = "New Starter Deck", menuName = "Decks/Starter Deck")]
    public class StarterDeckData : ScriptableObject
    {
        public string deckName;

        [TextArea]
        public string deckDescription;

        public Sprite deckIcon;
        public List<Card> startingCards = new List<Card>();
    }
}