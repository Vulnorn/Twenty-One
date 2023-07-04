namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            BlackJackGame blackJackGame = new BlackJackGame(17);

            List<Player> playerList = new List<Player>()
            {
                new Cropier(),
                new Player()
            };

            blackJackGame.PlayBlackJack(playerList);
        }
    }



    class BlackJackGame
    {
        private Deck _deck = new Deck();
        private Dictionary<string, int> _cardPoints;

        public BlackJackGame(int minPointsCards)
        {
            MinPointsCards = minPointsCards;
            BlackJackNumber = 21;

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
        public int PointsCardsPlayer { get; private set; }
        public int PointsCardsCroupier { get; private set; }
        public int PointsWinPlayer { get; private set; }
        public int PointsWinCroupier { get; private set; }

        private int[] CreatelistPlayers(List<Player> players)
        {
            int[] pointsWin = new int[players.Count()];

            for (int i = 0;i>pointsWin.Length;i++)
            {
                pointsWin[i] = 0;
            }

            return pointsWin;
        }

        public void PlayBlackJack(List<Player> players)
        {
            int numbersCardsInDeck = 0;
            int minNumbersCardsForGames = 10;
            bool isGame = true;
            bool isFinish;

            int[] pointsWin = CreatelistPlayers(players);

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

                HandOutFirstCards(players, pointsWin);
                ShoyGameTabel(players);

                if (PickWinner(player, isFinish))
                    continue;

                GamePlayer(player);
                GameCroupier(player);
                isFinish = true;

                if (PickWinner(player, isFinish))
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

        //private void TransferCardCroupier()
        //{
        //    if (_deck.TryGetCard(out Card card))
        //    {
        //        _cards.Add(card);
        //    }
        //}

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

        private void ShoyGameTabel(List<Player> players, int[] pointsWins)
        {
            string border = new string('_', 60);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("    Добро пожаловать в игру Блек-Джек    ");
            Console.WriteLine($"Счет в игре: Казино {pointsWins[0]} побед | Игрок {pointsWins[1]} побед");
            Console.WriteLine(border);

            PointsCardsCroupier = CalculatePoints(players[0].GetCards());
            PointsCardsPlayer = CalculatePoints(players[1].GetCards());

            Console.WriteLine(border);

            players[1].ShowCards();

            Console.WriteLine($"\nУ игрока {PointsCardsPlayer} очков\n");
            Console.Write($"У крупье в руке: ");

            players[0].ShowCards();
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

        private bool PickWinner(Player player, Cropier cropier, bool isFinish)
        {
            bool isWin = false;

            if (PointsCardsCroupier == BlackJackNumber && PointsCardsPlayer == BlackJackNumber)
            {
                ShowMassage("Ничья. Оба игрока собрали Блек-Джек. Поздравляем!");
                player.ShowCards();
                cropier.ShowCards();
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
                cropier.ShowCards();
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
            TransferCard(player);
            PointsCardsPlayer = CalculatePoints(player.GetCards());

            if (PointsCardsPlayer > BlackJackNumber)
            {
                Console.WriteLine($"Сумма карт превысила {BlackJackNumber} ход передается Крупье.");
                return isMovePlayer = false;
            }

            return isMovePlayer = true;
        }
        private void GameCroupier(Player player, Cropier cropier)
        {
            ShoyGameTabel(player, cropier);

            while (PointsCardsCroupier <= MinPointsCards)
            {
                TransferCard(cropier);
                PointsCardsCroupier = CalculatePoints(cropier.GetCards());
            }

            cropier.ShowCards();
            Console.WriteLine($"\nУ Крупье {PointsCardsCroupier} очков.\n");
        }

        private void ShowMassage(string massage)
        {
            Console.WriteLine(massage);
            Console.ReadKey();
        }

        private void HandOutFirstCards(List <Player> players, int[] pointsWin)
        {
            //PointsCardsPlayer = 0;
            //player.ClearHand();
            //TransferCard(player);
            //TransferCard(player);

            //cropier.ClearHand();
            //PointsCardsCroupier = 0;
            //TransferCard(cropier);
            //TransferCard(cropier);

            for (int i = 0;i<players.Count; i++)
            {
                players[i].ClearHand();

            }
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