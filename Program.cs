namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Croupier croupier = new Croupier(17);

            Player player = new Player();
            croupier.PlayBlackJack(player);
        }
    }

    class Croupier
    {
        private Deck _deck = new Deck();
        private List<Card> _cards = new List<Card>();
        private Dictionary<string, int> _cardPoints;

        public Croupier(int minPointsCardsCroupier)
        {
            MinPointsCardsCroupier = minPointsCardsCroupier;
            BlackJackNumber = 21;
            PointsWinPlayer = 0;
            PointsWinCroupier = 0;
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
        public int MinPointsCardsCroupier { get; private set; }
        public int PointsCardsPlayer { get; private set; }
        public int PointsCardsCroupier { get; private set; }
        public int PointsWinPlayer { get; private set; }
        public int PointsWinCroupier { get; private set; }

        public void PlayBlackJack(Player player)
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

                HandOutFirstCards(player);
                ShoyGameTabel(player);

                if (PickWinner(player, isFinish))
                    continue;

                GamePlayer(player);
                GameCroupier(player);
                isFinish = true;

                if (PickWinner(player, isFinish))
                    continue;
            }
        }

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
                int point = _cardPoints[card.Rank];
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

        private void ShoyGameTabel(Player player)
        {
            string border = new string('_', 60);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("    Добро пожаловать в игру Блек-Джек    ");
            Console.WriteLine($"Счет в игре: Казино {PointsWinCroupier} побед | Игрок {PointsWinPlayer} побед");
            Console.WriteLine(border);

            PointsCardsCroupier = CalculatePoints(_cards);
            PointsCardsPlayer = CalculatePoints(player.GetCards());

            Console.WriteLine(border);

            player.ShowCards();

            Console.WriteLine($"\nУ игрока {PointsCardsPlayer} очков\n");
            Console.Write($"У крупье в руке: ");

            _cards[0].ShowInfo();
            Console.Write(" ##");

            Console.WriteLine();
            Console.WriteLine(border);
            Console.WriteLine();
        }

        private void EarnPointsForWinPlayer()
        {
            PointsWinPlayer++;
        }

        private void EarnPointsForWinCroupier()
        {
            PointsWinCroupier++;
        }

        private bool PickWinner(Player player, bool isFinish)
        {
            bool isWin = false;

            if (PointsCardsCroupier == BlackJackNumber && PointsCardsPlayer == BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали Блек-Джек. Поздравляем!");
                player.ShowCards();
                ShowCards();
                isWin = true;
            }
            else if (PointsCardsPlayer == BlackJackNumber)
            {
                ShowMassage("Игрок собрал Блек-Джек. Поздравляем, Вы победили!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (PointsCardsCroupier == BlackJackNumber)
            {
                ShowMassage("Крупье получил Блек-Джек. Вы проиграли эту партию.");
                ShowCards();
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (PointsCardsPlayer > BlackJackNumber && PointsCardsCroupier > BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали больше.");
                isWin = true;
            }
            else if (PointsCardsPlayer > BlackJackNumber && isFinish == true)
            {
                ShowMassage("Крупье выиграл");
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (PointsCardsCroupier > BlackJackNumber && isFinish == true)
            {
                ShowMassage("Игрок победил!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (PointsCardsPlayer < PointsCardsCroupier && isFinish == true)
            {
                ShowMassage("Крупье выиграл");
                EarnPointsForWinCroupier();
                isWin = true;
            }
            else if (PointsCardsPlayer > PointsCardsCroupier && isFinish == true)
            {
                ShowMassage("Игрок победил!");
                EarnPointsForWinPlayer();
                isWin = true;
            }
            else if (PointsCardsPlayer == PointsCardsCroupier && isFinish == true)
            {
                ShowMassage("Ничья!");
                Console.ReadKey();
                isWin = true;
            }

            return isWin;
        }

        private void GamePlayer(Player player)
        {
            const string TakeCardMenu = "1";
            const string СheckMenu = "2";

            string userInput;
            bool isGamePlayer = true;

            while (isGamePlayer)
            {
                ShoyGameTabel(player);
                Console.WriteLine($"{TakeCardMenu} - Взять одну карту.");
                Console.WriteLine($"{СheckMenu} - Чек.");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case TakeCardMenu:
                        isGamePlayer = AddCard(player, isGamePlayer);
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

        private bool AddCard(Player player, bool isMovePlayer)
        {
            TransferCardPlayer(player);
            PointsCardsPlayer = CalculatePoints(player.GetCards());

            if (PointsCardsPlayer > BlackJackNumber)
            {
                Console.WriteLine($"Сумма карт превысила {BlackJackNumber} ход передается Крупье.");
                return isMovePlayer = false;
            }

            return isMovePlayer = true;
        }
        private void GameCroupier(Player player)
        {
            ShoyGameTabel(player);

            while (PointsCardsCroupier <= MinPointsCardsCroupier)
            {
                TransferCardCroupier();
                PointsCardsCroupier = CalculatePoints(_cards);
            }

            ShowCards();
            Console.WriteLine($"\nУ Крупье {PointsCardsCroupier} очков.\n");
        }

        private void ShowMassage(string massage)
        {
            Console.WriteLine(massage);
            Console.ReadKey();
        }

        private void HandOutFirstCards(Player player)
        {
            PointsCardsPlayer = 0;
            player.ClearHand();
            TransferCardPlayer(player);
            TransferCardPlayer(player);

            _cards.Clear();
            PointsCardsCroupier = 0;
            TransferCardCroupier();
            TransferCardCroupier();
        }

        private void ShowCards()
        {
            Console.WriteLine("У Крупье в руке");

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].ShowInfo();
            }

            Console.WriteLine();
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