//проверять кол-во карт в колоде. 10 карт - новая колода. 3. закончить игру у игрока. Вывод информации козино / игрок кол-во побед. 
using System;
using System.IO;
using System.Numerics;
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
            Croupier croupier = new Croupier(21, 17);

            Player player = new Player();
            croupier.PlayBlackJack(player);
        }
    }

    class Croupier
    {
        private Deck _deck = new Deck();
        private List<Card> _cards = new List<Card>();
        private Dictionary<string, int> _cardPoints = new Dictionary<string, int>();

        public Croupier(int blackJackNumber, int minPointsCardsCroupier)
        {
            BlackJackNumber = blackJackNumber;
            MinPointsCardsCroupier = minPointsCardsCroupier;

            CreateTablePoint();
        }

        public int BlackJackNumber { get; private set; }
        public int MinPointsCardsCroupier { get; private set; }

        public int PointsCardsPlayer { get; private set; }
        public int PointsCardsCroupier { get; private set; }

        private void TransferCardPlayer(Player player)
        {
            if (_deck.TryGetCard(out Card card))
            {
                player.TakeCard(card);
            }
        }

        private int CalculatePoints(List<Card> cards)
        {
            int points = 0;

            foreach (Card card in cards)
            {
                int point = _cardPoints[card.Ranks];
                points += point;
            }

            return points;
        }

        private void TransferCardCroupier()
        {
            if (_deck.TryGetCard(out Card card))
            {
                _cards.Add(card);
            }
        }

        public void PlayBlackJack(Player player)
        {
            int numbersCardsInDeck = 0;
            int minNumbersCardsForGames = 10;
            int pointsPlayer = 0;
            int pointsCroupier = 0;
            bool isGame = true;

            numbersCardsInDeck = _deck.ShowNumbersCards(numbersCardsInDeck);

            while (isGame)
            {
                Console.Clear();
                FirstHandOutCards(player);

                PointsCardsCroupier = CalculatePoints(_cards);
                PointsCardsPlayer = CalculatePoints(player.GetCards());

                Console.WriteLine($"У игрока {PointsCardsPlayer} очков");
                Console.WriteLine($"У крупье {PointsCardsCroupier} очков");

                if (PickWinner(numbersCardsInDeck))
                    continue;

                MovePlayer(player);
                MoveCroupier();

            }
        }

        private bool PickWinner(int numbersCardsInDeck)
        {
            bool isWin = false;

            if (PointsCardsCroupier == BlackJackNumber && PointsCardsPlayer == BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали Блек Джек. Поздравляем!", numbersCardsInDeck);
                isWin = true;
            }

            if (PointsCardsPlayer == BlackJackNumber)
            {
                ShowMassage("Игрок собрал Блек Джек. Поздравляем, Вы победили!", numbersCardsInDeck);
                //pointsPlayer++;
                isWin = true;
            }

            if (PointsCardsCroupier == BlackJackNumber)
            {
                ShowMassage("Дилер получил Блек Джек. Вы проиграли эту партию.", numbersCardsInDeck);
                //pointsCroupier++;
                isWin = true;
            }

            if (PointsCardsPlayer > BlackJackNumber && PointsCardsCroupier > BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали больше.", numbersCardsInDeck);
                isWin = true;
            }

            if (PointsCardsPlayer > BlackJackNumber && PointsCardsCroupier <= BlackJackNumber || PointsCardsPlayer < BlackJackNumber && PointsCardsCroupier < BlackJackNumber && PointsCardsPlayer < PointsCardsCroupier)
            {
                ShowMassage("Дилер выиграл", numbersCardsInDeck);
                //pointsCroupier++;
                isWin = true;
            }

            if (PointsCardsPlayer <= BlackJackNumber && PointsCardsCroupier > BlackJackNumber || PointsCardsPlayer < BlackJackNumber && PointsCardsCroupier < BlackJackNumber && PointsCardsPlayer > PointsCardsCroupier)
            {
                ShowMassage("Игрок победил!", numbersCardsInDeck);
                //pointsPlayer++;
                isWin = true;
            }

            return isWin;
        }
        private void MovePlayer(Player player)
        {
            const string TakeCardMenu = "1";
            const string СheckMenu = "2";

            string userInput;
            bool isMovePlayer = true;

            while (isMovePlayer)
            {
                Console.WriteLine($"{TakeCardMenu} - Взять одну карту.");
                Console.WriteLine($"{СheckMenu} - Чек.");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case TakeCardMenu:
                        isMovePlayer = AddCard(player, isMovePlayer);
                        break;

                    case СheckMenu:
                        isMovePlayer = false;
                        break;

                    default:
                        DefaultCase(player);
                        break;
                }
            }
        }

        private bool AddCard(Player player, bool isMovePlayer)
        {
            TransferCardPlayer(player);
            PointsCardsPlayer = CalculatePoints(player.GetCards());
            player.ShowCards();

            if (PointsCardsPlayer > BlackJackNumber)
            {
                Console.WriteLine($"Сумма карт превысила {BlackJackNumber} ход передается крупье.");
                return isMovePlayer = false;
            }

            return isMovePlayer = true;
        }
        private void MoveCroupier()
        {
            while (PointsCardsCroupier <= MinPointsCardsCroupier)
            {
                TransferCardCroupier();
                PointsCardsCroupier = CalculatePoints(_cards);
            }

            ShowCards();
        }

        private int ShowMassage(string massage, int numbersCardsInDeck)
        {
            Console.WriteLine(massage);
            Console.ReadKey();
            return numbersCardsInDeck = _deck.ShowNumbersCards(numbersCardsInDeck);
        }

        private void DefaultCase(Player player)
        {
            Console.Clear();
            Console.WriteLine("Ошибка ввода команды.");
            Console.WriteLine();

            player.ShowCards();
            ShowCards();
        }

        private void FirstHandOutCards(Player player)
        {
            PointsCardsPlayer = 0;
            player.ClearHand();
            TransferCardPlayer(player);
            TransferCardPlayer(player);
            player.ShowCards();

            PointsCardsCroupier = 0;
            TransferCardCroupier();
            TransferCardCroupier();
            ShowCards();
        }

        private void ShowCards()
        {
            Console.WriteLine("У крупье в руке");

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }

            Console.WriteLine();
        }


        private void CreateTablePoint()
        {
            int pointsTwo = 2;
            int pointsThree = 3;
            int pointsFour = 4;
            int pointsFive = 5;
            int pointsSix = 6;
            int pointsSeven = 7;
            int pointsEight = 8;
            int pointsNine = 9;
            int pointsTen = 10;
            int pointsJack = 10;
            int pointsQueen = 10;
            int pointsKing = 10;
            int pointsAce = 11;

            _cardPoints.Add("2", pointsTwo);
            _cardPoints.Add("3", pointsThree);
            _cardPoints.Add("4", pointsFour);
            _cardPoints.Add("5", pointsFive);
            _cardPoints.Add("6", pointsSix);
            _cardPoints.Add("7", pointsSeven);
            _cardPoints.Add("8", pointsEight);
            _cardPoints.Add("9", pointsNine);
            _cardPoints.Add("10", pointsTen);
            _cardPoints.Add("В", pointsJack);
            _cardPoints.Add("Д", pointsQueen);
            _cardPoints.Add("К", pointsKing);
            _cardPoints.Add("Т", pointsAce);
        }

        private void ScoringPoints()
        {

        }
    }

    class Player
    {
        private List<Card> _gameHand = new List<Card>();

        public List<Card> GetCards()
        {
            return _gameHand.ToList();
        }


        public void TakeCard(Card card)
        {
            _gameHand.Add(card);
        }

        public void ShowCards()
        {
            Console.WriteLine("У вас в руке");

            for (int i = 0; i < _gameHand.Count; i++)
            {
                _gameHand[i].ShowInfo();
            }

            Console.WriteLine();
        }

        public void ClearHand()
        {
            _gameHand.Clear();
        }
    }

    class Deck
    {
        private List<Card> _cards = new List<Card>();
        private Stack<Card> _deskCards = new Stack<Card>();

        public Deck()
        {
            CreateCards();
            ShuffleCards();
            AddCardsInDesk();
        }

        public void AddDesk()
        {
            ShuffleCards();
            AddCardsInDesk();
        }

        public void ShowCards()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }

            Console.WriteLine();
        }

        private void CreateCards()
        {
            string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "В", "Д", "К", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    _cards.Add(new Card(ranks[j], suits[i]));
                }
            }
        }

        private void ShuffleCards()
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

        private void AddCardsInDesk()
        {
            foreach (Card element in _cards)
            {
                _deskCards.Push(element);
            }
        }

        public bool TryGetCard(out Card card)
        {
            card = null;

            if (_deskCards.Count > 0)
            {
                card = _deskCards.Pop();
                return true;
            }

            return false;
        }

        public int ShowNumbersCards(int numbersCards)
        {
            return numbersCards = _deskCards.Count;
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