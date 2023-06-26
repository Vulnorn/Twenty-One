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
            deck.ShufflingDeskDividedIntoFourParts();
            deck.ShowCards();
            Console.ReadKey();
            //Console.WriteLine($"Игра 21 очко. игрок против крупье.");
            //Console.WriteLine($"Играется колодой из {deskCards._cards.Count}") ;
        }
    }

    //class Game
    //{
    //    public Croupier Croupier = new Croupier();
    //    //public Player Player = new Player();


    //    private void InputPlayerName()
    //    {
    //        Console.WriteLine($"Введите имя Игрока.");
    //        string name = Console.ReadLine();
    //    }
    //}

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
            //ShufflingDeskFisherYates();
            //ShufflingDeskDividedIntoFourParts();
            //ShufflingDeskFisherYates();
            //CreateDesk();
        }

        public void ShowCards()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
                //if (i/9) Сделать вывод в 4 строки.
            }

            Console.WriteLine(" ");
        }

        private void CreateCards()
        {
            string[] ranks = new string[] { "6", "7", "8", "9", "10", "В", "Д", "К", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < ranks.Length; i++)
            {
                for (int j = 0; j < suits.Length; j++)
                {
                    _cards.Add(new Card(ranks[i], suits[j]));
                }
            }
        }

        private void ShufflingDeskFisherYates()
        {
            Card card = null;
            Random random = new Random();

            for (int i = _cards.Count - 1; i > 0; --i)
            {
                int j = random.Next(i + 1);
                card = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = card;
            }
        }

        public void ShufflingDeskDividedIntoFourParts()
        {
            List<Card> _mixedCards = new List<Card>();

            int halfDesk = _cards.Count / 2;
            int oneThirdHalfDeck = halfDesk / 3;
            int twoThirdHalfDeck = halfDesk * 2 / 3;

            _mixedCards.AddRange(_cards.GetRange(oneThirdHalfDeck, twoThirdHalfDeck - oneThirdHalfDeck));
            _mixedCards.AddRange(_cards.GetRange(halfDesk, halfDesk));
            _mixedCards.AddRange(_cards.GetRange(0, oneThirdHalfDeck));
            _mixedCards.AddRange(_cards.GetRange(twoThirdHalfDeck, halfDesk - twoThirdHalfDeck));


            _cards = _mixedCards;
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