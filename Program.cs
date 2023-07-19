namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> playerList = new List<Player>()
            {
                new Player(),
                new Player()
            };

            BlackJackGame blackJackGame = new BlackJackGame(playerList);

            blackJackGame.Play();
        }
    }

    class BlackJackGame
    {
        private Deck _deck = new Deck();
        private Dictionary<string, int> _cardPoints;
        private List<Player> _players;
        private int[] _score;
        private int[] _pointsCards;

        public BlackJackGame(List<Player> players)
        {
            MinPointsCards = 17;
            BlackJackNumber = 21;
            _players = players;
            _score = new int[players.Count()];
            _pointsCards = new int[players.Count()];

            _cardPoints = new Dictionary<string, int>()
            {
                ["2"] = 2,
                ["3"] = 3,
                ["4"] = 4,
                ["5"] = 5,
                ["6"] = 6,
                ["7"] = 7,
                ["8"] = 8,
                ["9"] = 9,
                ["10"] = 10,
                ["В"] = 10,
                ["Д"] = 10,
                ["К"] = 10,
                ["Т"] = 11
            };
        }

        public int BlackJackNumber { get; private set; }
        public int MinPointsCards { get; private set; }

        public void Play()
        {
            int numbersCardsInDeck;
            int minNumbersCardsForGames = 10;
            bool isGame = true;
            bool isFinish;

            while (isGame)
            {
                if (TryOutGame())
                {
                    isGame = false;
                    continue;
                }

                isFinish = false;
                numbersCardsInDeck = _deck.GetNumbersCards();

                if (numbersCardsInDeck < minNumbersCardsForGames)
                    _deck.CreateNew();

                HandOutFirstCards();
                ShowGameTable();

                if (PickWinner(isFinish))
                    continue;

                TurnPlayer();
                TurnCroupier();
                isFinish = true;

                if (PickWinner(isFinish))
                    continue;
            }
        }

        private void TransferCard(Player player)
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
                int point = _cardPoints[card.Rank];
                points += point;
            }

            return points;
        }

        private bool TryOutGame()
        {
            const string StartGame = "1";
            const string ExitGame = "2";

            bool isOut = false;
            string userInput;

            Console.Clear();
            Console.WriteLine($"{StartGame} - Начать игру.");
            Console.WriteLine($"{ExitGame} - Выйти из игры.");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case StartGame:
                    isOut = false;
                    break;

                case ExitGame:
                    isOut = true;
                    break;

                default:
                    ShowMessageError();
                    break;
            }

            return isOut;
        }

        private void ShowMessageError()
        {
            Console.WriteLine("Не корректный ввод.");
            Console.ReadKey();
        }

        private void ShowGameTable()
        {
            string border = new string('_', 60);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("    Добро пожаловать в игру Блек-Джек    ");
            Console.WriteLine($"Счет в игре: Казино {_score[0]} побед | Игрок {_score[1]} побед");
            Console.WriteLine(border);

            _pointsCards[0] = CalculatePoints(_players[0].GetCards());
            _pointsCards[1] = CalculatePoints(_players[1].GetCards());

            Console.WriteLine(border);

            _players[1].ShowCards();

            Console.WriteLine($"\nУ игрока {_pointsCards[1]} очков\n");
            Console.WriteLine($"У крупье ");

            _players[0].ShowCards();

            Console.WriteLine();
            Console.WriteLine(border);
            Console.WriteLine();
        }

        private void EarnPointsForWinPlayer()
        {
            _score[1]++;
        }

        private void EarnPointsForWinCroupier()
        {
            _score[0]++;
        }

        private bool PickWinner(bool isFinish)
        {
            bool isWin = false;

            if (_pointsCards[0] == BlackJackNumber && _pointsCards[1] == BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали Блек-Джек. Поздравляем!");
                _players[1].ShowCards();
                _players[0].ShowCards();
                isWin = true;
            }
            else if (_pointsCards[1] == BlackJackNumber)
            {
                ShowMassage("Игрок собрал Блек-Джек. Поздравляем, Вы победили!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (_pointsCards[0] == BlackJackNumber)
            {
                ShowMassage("Крупье получил Блек-Джек. Вы проиграли эту партию.");
                _players[0].ShowCards();
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (_pointsCards[1] > BlackJackNumber && _pointsCards[0] > BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали больше.");
                isWin = true;
            }
            else if (_pointsCards[1] > BlackJackNumber && isFinish == true)
            {
                ShowMassage("Крупье выиграл");
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (_pointsCards[0] > BlackJackNumber && isFinish == true)
            {
                ShowMassage("Игрок победил!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (_pointsCards[1] < _pointsCards[0] && isFinish == true)
            {
                ShowMassage("Крупье выиграл");
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (_pointsCards[1] > _pointsCards[0] && isFinish == true)
            {
                ShowMassage("Игрок победил!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (_pointsCards[1] == _pointsCards[0] && isFinish == true)
            {
                ShowMassage("Ничья!");
                Console.ReadKey();
                isWin = true;
            }

            return isWin;
        }

        private void TurnPlayer()
        {
            const string TakeCardMenu = "1";
            const string СheckMenu = "2";

            string userInput;
            bool isGamePlayer = true;

            while (isGamePlayer)
            {
                ShowGameTable();
                Console.WriteLine($"{TakeCardMenu} - Взять одну карту.");
                Console.WriteLine($"{СheckMenu} - Чек.");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case TakeCardMenu:
                        isGamePlayer = AddCardPlayer();
                        break;

                    case СheckMenu:
                        isGamePlayer = false;
                        break;

                    default:
                        ShowMessageError();
                        break;
                }
            }
        }

        private bool AddCardPlayer()
        {
            TransferCard(_players[1]);
            _pointsCards[1] = CalculatePoints(_players[1].GetCards());

            if (_pointsCards[1] > BlackJackNumber)
            {
                Console.WriteLine($"Сумма карт превысила {BlackJackNumber} ход передается Крупье.");
                return false;
            }
            else if (_pointsCards[1] == BlackJackNumber)
            {
                return false;
            }

            return true;
        }

        private void TurnCroupier()
        {
            ShowGameTable();

            while (_pointsCards[0] <= MinPointsCards)
            {
                TransferCard(_players[0]);
                _pointsCards[0] = CalculatePoints(_players[0].GetCards());
            }

            _players[0].ShowCards();
            Console.WriteLine($"\nУ Крупье {_pointsCards[0]} очков.\n");
        }

        private void ShowMassage(string massage)
        {
            Console.WriteLine(massage);
            Console.ReadKey();
        }

        private void HandOutFirstCards()
        {
            _pointsCards[1] = 0;
            _pointsCards[0] = 0;

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].ClearHand();
                TransferCard(_players[i]);
                TransferCard(_players[i]);
            }
        }
    }

    class Player
    {
        private List<Card> _cards = new List<Card>();

        public List<Card> GetCards()
        {
            return _cards.ToList();
        }

        public void TakeCard(Card card)
        {
            _cards.Add(card);
        }

        public void ShowCards()
        {
            Console.WriteLine("В руке");

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }

            Console.WriteLine();
        }

        public void ClearHand()
        {
            _cards.Clear();
        }
    }

    class Deck
    {
        private static Random _random = new Random();
        private Stack<Card> _cards = new Stack<Card>();

        public Deck()
        {
            CreateNew();
        }

        public void CreateNew()
        {
            List<Card> cards = CreateCards();
            ShuffleCards(cards);
            FoldCards(cards);
        }

        public bool TryGetCard(out Card card)
        {
            return _cards.TryPop(out card);
        }

        public int GetNumbersCards()
        {
            return _cards.Count;
        }

        private List<Card> CreateCards()
        {
            List<Card> cards = new List<Card>();
            string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "В", "Д", "К", "Т" };
            string[] suits = new string[] { "♠", "♣", "♦", "♥" };

            for (int i = 0; i < suits.Length; i++)
            {
                for (int j = 0; j < ranks.Length; j++)
                {
                    cards.Add(new Card(ranks[j], suits[i]));
                }
            }

            return cards;
        }

        private void ShuffleCards(List<Card> cards)
        {

            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = i;

                while (randomIndex == i)
                {
                    randomIndex = _random.Next(cards.Count);
                }

                Card card = cards[randomIndex];
                cards[randomIndex] = cards[i];
                cards[i] = card;
            }
        }

        private void FoldCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                _cards.Push(card);
            }
        }
    }

    class Card
    {
        public Card(string rank, string suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public string Rank { get; private set; }
        public string Suit { get; private set; }

        public void ShowInfo()
        {
            Console.Write($"{Rank}{Suit} ");
        }
    }
}