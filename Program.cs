namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> playerList = new List<Player>()
            {
                new Cropier(),
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
            int numbersCardsInDeck = 0;
            int minNumbersCardsForGames = 10;
            bool isGame = true;
            bool isFinish;

            while (isGame)
            {
                if (OutGame())
                {
                    isGame = false;
                    continue;
                }

                isFinish = false;
                numbersCardsInDeck = _deck.ShowNumbersCards(numbersCardsInDeck);

                if (numbersCardsInDeck < minNumbersCardsForGames)
                    _deck.AddDeck();

                HandOutFirstCards();
                ShoyGameTabel();

                if (PickWinner( isFinish))
                    continue;

                GamePlayer();
                GameCroupier();
                isFinish = true;

                if (PickWinner( isFinish))
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

        private bool OutGame()
        {
            const string GameMenu = "1";
            const string OutGameMenu = "2";

            bool isOut = false;
            string userInput;

            Console.Clear();
            Console.WriteLine($"{GameMenu} - Играть.");
            Console.WriteLine($"{OutGameMenu} - Выйти из игры.");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case GameMenu:
                    isOut = false;
                    break;

                case OutGameMenu:
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

        private void ShoyGameTabel()
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
            Console.Write($"У крупье в руке: ");

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

        private bool PickWinner( bool isFinish)
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

        private void GamePlayer()
        {
            const string TakeCardMenu = "1";
            const string СheckMenu = "2";

            string userInput;
            bool isGamePlayer = true;

            while (isGamePlayer)
            {
                ShoyGameTabel();
                Console.WriteLine($"{TakeCardMenu} - Взять одну карту.");
                Console.WriteLine($"{СheckMenu} - Чек.");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case TakeCardMenu:
                        isGamePlayer = AddCardPlayer(isGamePlayer);
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

        private bool AddCardPlayer(bool isGamePlayer)
        {
            TransferCard(_players[1]);
            _pointsCards[1] = CalculatePoints(_players[1].GetCards());

            if (_pointsCards[1] > BlackJackNumber)
            {
                Console.WriteLine($"Сумма карт превысила {BlackJackNumber} ход передается Крупье.");
                return isGamePlayer = false;
            }
            else if (_pointsCards[1] == BlackJackNumber)
            {
                return isGamePlayer = false;
            }

            return isGamePlayer = true;
        }
        private void GameCroupier()
        {
            ShoyGameTabel();

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
        private List<Card> _gameHand = new List<Card>();

        public int PointsWin { get; private set; }

        public void TakePointsWin()
        {

        }

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
            Console.WriteLine("В руке");

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

    class Cropier : Player
    {

    }



    class Deck
    {
        private static Random _random = new Random();
        private Stack<Card> _cards = new Stack<Card>();

        public Deck()
        {
            AddDeck();
        }

        public void AddDeck()
        {
            List<Card> cards = CreateCards();
            cards = ShuffleCards(cards);
            AddCardsInDeck(cards);
        }

        public bool TryGetCard(out Card card)
        {
            card = null;

            if (_cards.Count > 0)
            {
                card = _cards.Pop();
                return true;
            }

            return false;
        }

        public int ShowNumbersCards(int numbersCards)
        {
            return numbersCards = _cards.Count;
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

        private List<Card> ShuffleCards(List<Card> cards)
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

            return cards;
        }

        private void AddCardsInDeck(List<Card> cards)
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