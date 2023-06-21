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
            DeskCards deskCards = new DeskCards();
            Card card = new Card(null,null);

            deskCards.CreateCard();
            deskCards.ShowAllCards();
            deskCards.ShufflingDeskFisherYates(card);
            Console.WriteLine(" ");
            deskCards.ShowAllCards();
            Console.ReadKey();
        }
    }

    class DeskCards
    {
        private List<Card> _cards = new List<Card>();

        public void CreateCard()
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
        }

        public void ShufflingDeskFisherYates(Card card)
        {
            Random random = new Random() ;

            for (int i = _cards.Count - 1; i > 0; --i)
            {
                int j = random.Next(i + 1);
                card  = _cards[i];
                _cards[i] = _cards[j];
                _cards[j] = card;
            }
        }

        public void ShufflingDeskDividedIntoFourParts(Card card)
        {
            int halfDesk = _cards.Count/2 ;
            int oneThirdHalfDeck = halfDesk/ 3;
            int twoThirdHalfDeck = (halfDesk - oneThirdHalfDeck) / 2;
            int thirdPartHalfDeck = halfDesk - twoThirdHalfDeck - oneThirdHalfDeck;

            for(int i )

        }


        public void ShowAllCards()
        {

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }
        }

    }

    class Croupier
    {

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