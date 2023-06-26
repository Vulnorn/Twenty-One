using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Xml.Linq;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck();
            deck.ShowCards();
            deck.ShufflingCards();
            deck.ShowCards();
            Console.ReadKey();
            //Console.WriteLine($"Игра 21 очко. игрок против крупье.");
            //Console.WriteLine($"Играется колодой из {deskCards._cards.Count}") ;
        }
    }

    class Croupier
    {
        private Deck _deck = new Deck();
        private List<Card> _cards = new List<Card>();


        public bool GiveOutCards(out Card cards)
        {
            cards = null;

            if (_deck.DealOneCards(out Card card))
            {

                return true;
            }
            else
            {
                Console.WriteLine($"В колоде не осталось карт.");
                Console.ReadKey();
                return false;
            }
        }

        public void PlayBlackJack(Player player)
        {

        }

        public void FirstHandOutCards()
        {

        }
    }

    class Player
    {
        private List<Card> _gameHand = new List<Card>();

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }



    }

    class Deck
    {
        private List<Card> _cards = new List<Card>();
        private Stack<Card> _DeskCards = new Stack<Card>();

        public Deck()
        {
            CreateCards();
            ShufflingCards();
            CreateDesk();
        }

        public void ShowCards()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
                if ((i+1) % 9 == 0)
                    Console.WriteLine($"\n");
            }

            Console.WriteLine();
        }

        private void CreateCards()
        {
            string[] ranks = new string[] { "6", "7", "8", "9", "10", "В", "Д", "К", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    _cards.Add(new Card(ranks[j], suits[i]));
                }
            }
        }

        public void ShufflingCards()
        {
            Random random = new Random();
            for (int i = 0; i < _cards.Count; i++)
            {
                int randomIndex = i;

                while (randomIndex == i)
                {
                    randomIndex = random.Next(_cards.Count);
                }

                Card card = _cards[randomIndex];
                _cards[randomIndex] = _cards[i];
                _cards[i] = card;
            }
        }

        private void CreateDesk()
        {
            foreach (Card element in _cards)
            {
                _DeskCards.Push(element);
            }
        }

        public bool DealOneCards(out Card card)
        {
            card = null;

            if (_DeskCards.Count > 0)
            {
                card = _DeskCards.Pop();
                return true;
            }

            return false;
        }
    }

    class Card
    {
        public Card(string ranks, string suits)
        {
            Ranks = ranks;
            Suits = suits;
        }

        public string Ranks { get; private set; }
        public string Suits { get; private set; }

        public void ShowInfo()
        {
            Console.Write($"{Ranks}{Suits} ");
        }
    }
}