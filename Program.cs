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

            Console.WriteLine($"Игра 21 очко. игрок против крупье.");
            //Console.WriteLine($"Играется колодой из {deskCards._cards.Count}") ;
        }
    }

    class Game
    {
        public Croupier Croupier = new Croupier();
        public Player Player = new Player();


        public void TakeTwoCards()
        {
            Croupier
        }

        private void InputPlayerName()
        {
            Console.WriteLine($"Введите имя Игрока.");
            string name = Console.ReadLine();
        }
    }

    class Croupier
    {
        private DeskCards _deskCards = new DeskCards();

        public bool GiveOutCards(out Card cards)
        {
            cards = null;

            if (_deskCards.DealOneCards(out Card card))
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

    class DeskCards
    {
        private List<Card> _cards = new List<Card>();
        private Stack<Card> _DeskCards = new Stack<Card>();

        public DeskCards()
        {
            const int CountDiamondsSuits = 0;
            const int CountHeartsSuits = 1;
            const int CountClubsSuits = 2;
            const int CountSpadesSuits = 3;

            string meaningCards = @"C:\Program Files (x86)\VSProect\Twenty-One\DeckCards.txt";
            string cardSuitsDiamonds = "буби";
            string cardSuitsHearts = "черви";
            string cardSuitsClubs = "крести";
            string cardSuitsSpades = "пики";
            int countSuits = 4;

            using (StreamReader reader = new StreamReader(meaningCards))
            {
                string ranks;

                while ((ranks = reader.ReadLine()) != null)
                {
                    for (int i = 0; i < countSuits; i++)
                    {
                        switch (i)
                        {
                            case CountDiamondsSuits:
                                _cards.Add(new Card(ranks, cardSuitsDiamonds));
                                break;

                            case CountHeartsSuits:
                                _cards.Add(new Card(ranks, cardSuitsHearts));
                                break;

                            case CountClubsSuits:
                                _cards.Add(new Card(ranks, cardSuitsClubs));
                                break;

                            case CountSpadesSuits:
                                _cards.Add(new Card(ranks, cardSuitsSpades));
                                break;
                        }
                    }
                }
            }

            ShufflingDeskFisherYates();
            ShufflingDeskDividedIntoFourParts();
            ShufflingDeskFisherYates();
            CreateDesk();
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

        private void ShufflingDeskDividedIntoFourParts()
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
        public Card(string ranks, string cardSuits)
        {
            Ranks = ranks;
            CardSuits = cardSuits;
        }
        public string Ranks { get; private set; }
        public string CardSuits { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"{Ranks} {CardSuits}");
        }
    }
}