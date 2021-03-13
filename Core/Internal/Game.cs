using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Internal
{
    internal class Game : IGame
    {
        private static readonly uint[] ImpossibleDeals = { 11982, 146692, 186216, 455889, 495505, 512118, 517776, 781948 };

        /// <summary>
        /// The ID of the game, if known, zero otherwise.
        /// </summary>
        private readonly uint id;
        private readonly Stack<Card>[] foundations = new Stack<Card>[4] { new Stack<Card>(), new Stack<Card>(), new Stack<Card>(), new Stack<Card>() };
        private readonly List<Card>[] columns = new List<Card>[8] { new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>() };
        private readonly Card[] cells = new Card[4] { null, null, null, null };

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// filled with the game corresponding to the ID.
        /// </summary>
        /// <param name="gameId">The ID of the game to be created.</param>
        public Game(uint gameId)
        {
            this.id = gameId;
            this.IsImpossibleToWin = ImpossibleDeals.Contains(this.id);
            this.InitGame(gameId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// as a copy of the provided <paramref name="other"/> game.
        /// </summary>
        /// <param name="other">The game to copy.</param>
        internal Game(Game other)
        {
            this.id = other.id;
            this.IsImpossibleToWin = other.IsImpossibleToWin;

            // apparently, the reverse is by design of the C# Stack<>
            this.foundations = other.foundations.Select(x => new Stack<Card>(x.Reverse())).ToArray();
            this.columns = other.columns.Select(x => new List<Card>(x)).ToArray();
            this.cells = other.cells.ToArray();
        }

        private Game()
        {
            this.id = 0;
        }

        public uint Id => this.id;

        /// <summary>
        /// Gets a value indicating whether the game is won.
        /// </summary>
        public bool IsWon
        {
            get
            {
                return this.foundations.Sum(x => x.Count) == 52;
            }
        }

        public bool IsImpossibleToWin { get; }

        public IReadOnlyList<Card> Cells
        {
            get
            {
                return this.cells;
            }
        }

        public IReadOnlyList<Card> Foundations
        {
            get
            {
                return this.foundations.Select(x => x.Count != 0 ? x.Peek() : null).ToArray();
            }
        }

        public IReadOnlyList<IReadOnlyList<Card>> Columns
        {
            get
            {
                return this.columns;
            }
        }

        /// <summary>
        /// Gets the AsciiRepresentation of the game.
        /// </summary>
        /// <seealso cref="Card.AsciiRepresentation"/>
        internal string AsciiRepresentation
        {
            get
            {
                return this.CreateRepresentation(x => x.AsciiRepresentation);
            }
        }

        /// <summary>
        /// Gets the UnicodeRepresentation of the game.
        /// </summary>
        /// <seealso cref="Card.UnicodeRepresentation"/>
        internal string UnicodeRepresentation
        {
            get
            {
                return this.CreateRepresentation(x => x.UnicodeRepresentation);
            }
        }

        internal static Game ParseFromUnicodeRepresentation(string unicodeRepresentation)
        {
            return ParseFromRepresentation(unicodeRepresentation, x => Card.ParseFromUnicodeRepresentation(x));
        }

        internal static List<uint> GetWinnableGames(uint first, uint last)
        {
            var list = new List<uint>();
            for (uint i = first; i <= last; i++)
            {
                if (!ImpossibleDeals.Contains(i))
                {
                    list.Add(i);
                }
            }

            return list;
        }

        internal uint GetLegalMoveSize(Location source, Location destination)
        {
            Card GetSourceCard()
            {
                Card card;

                if (source == Location.Cell0 ||
                    source == Location.Cell1 ||
                    source == Location.Cell2 ||
                    source == Location.Cell3)
                {
                    card = this.cells[(int)source - (int)Location.Cell0];
                }
                else
                {
                    card = this.columns[(int)source - (int)Location.Column0].Last();
                }

                return card;
            }

            uint? SimpleChecks()
            {
                // no move if source equals destination
                if (source == destination)
                {
                    return 0;
                }

                // no move from foundation
                if (source == Location.Foundation)
                {
                    return 0;
                }

                // no move from empty cell
                if ((source == Location.Cell0 ||
                    source == Location.Cell1 ||
                    source == Location.Cell2 ||
                    source == Location.Cell3)
                    && this.cells[(int)source - (int)Location.Cell0] == null)
                {
                    return 0;
                }

                // no move from empty column
                if (IsColumnLocation(source)
                    && this.columns[(int)source - (int)Location.Column0].Count == 0)
                {
                    return 0;
                }

                // no move to filled cell
                if ((destination == Location.Cell0 ||
                    destination == Location.Cell1 ||
                    destination == Location.Cell2 ||
                    destination == Location.Cell3)
                    && this.cells[(int)destination - (int)Location.Cell0] != null)
                {
                    return 0;
                }

                return null;
            }

            uint? FoundationCheck()
            {
                if (destination == Location.Foundation)
                {
                    var card = GetSourceCard();

                    var foundation = this.FindFoundationFor(card.Suit);

                    if (card.Rank == Rank.Ace)
                    {
                        return foundation.Count == 0 ? 1u : 0u;
                    }
                    else
                    {
                        return foundation.Count != 0 && foundation.Peek().Rank + 1 == card.Rank ? 1u : 0u;
                    }
                }

                return null;
            }

            uint? ColumnCheck()
            {
                if (IsColumnLocation(destination))
                {
                    var destinationColumn = this.columns[(int)destination - (int)Location.Column0];

                    if (IsColumnLocation(source))
                    {
                        var sourceColumn = this.columns[(int)source - (int)Location.Column0];

                        // get maximum move size according to cells and empty columns
                        var numberEmptyColumns = this.columns.Count(x => x.Count == 0) - (destinationColumn.Count == 0 ? 1 : 0);
                        var numberEmptyCells = this.cells.Count(x => x == null);

                        uint maxMoveSize = Convert.ToUInt32(numberEmptyCells) + 1;

                        for (int i = 0; i < numberEmptyColumns; i++)
                        {
                            maxMoveSize *= 2;
                        }

                        maxMoveSize = Math.Min(13, maxMoveSize);

                        // look for a continuous run in the source column
                        var run = 1;

                        for (int i = 0; i < sourceColumn.Count - 1; i++)
                        {
                            var upperCard = sourceColumn[sourceColumn.Count - (2 + i)];
                            var lowerCard = sourceColumn[sourceColumn.Count - (1 + i)];

                            if (upperCard.IsBlack != lowerCard.IsBlack && upperCard.Rank == lowerCard.Rank + 1)
                            {
                                run++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        maxMoveSize = Math.Min(Convert.ToUInt32(run), maxMoveSize);

                        // now cut down move such that it fits onto the destination bottom card
                        var destinationCard = destinationColumn.LastOrDefault();

                        if (destinationCard == null)
                        {
                            return maxMoveSize;
                        }
                        else
                        {
                            uint moveSize = 1;
                            bool foundLegalMove = false;
                            for (; moveSize <= maxMoveSize; moveSize++)
                            {
                                var topCardMoved = sourceColumn[sourceColumn.Count - (int)moveSize];

                                if (topCardMoved.IsBlack != destinationCard.IsBlack
                                && destinationCard.Rank == topCardMoved.Rank + 1)
                                {
                                    foundLegalMove = true;
                                    break;
                                }
                            }

                            return foundLegalMove ? moveSize : 0;
                        }
                    }
                    else
                    {
                        if (destinationColumn.Count != 0)
                        {
                            var sourceCard = GetSourceCard();

                            var destinationCard = destinationColumn.Last();

                            return sourceCard.IsBlack != destinationCard.IsBlack
                                && destinationCard.Rank == sourceCard.Rank + 1 ? 1u : 0u;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }

                return null;
            }

            return SimpleChecks() ?? FoundationCheck() ?? ColumnCheck() ?? 1u;
        }

        internal void MakeMove(Location source, Location destination, uint moveSize)
        {
            if (moveSize == 0)
            {
                throw new ArgumentException($"moveSize '{moveSize}' is not valid");
            }

            if (moveSize > 1 && (!IsColumnLocation(source) || !IsColumnLocation(destination)))
            {
                throw new ArgumentException($"moveSize '{moveSize}' is impossible for source '{source}', destination '{destination}'");
            }

            List<Card> cards = new List<Card>(13);

            switch (source)
            {
                case Location.Cell0:
                case Location.Cell1:
                case Location.Cell2:
                case Location.Cell3:
                    cards.Add(this.cells[(int)source - (int)Location.Cell0]);
                    this.cells[(int)source - (int)Location.Cell0] = null;
                    break;

                case Location.Column0:
                case Location.Column1:
                case Location.Column2:
                case Location.Column3:
                case Location.Column4:
                case Location.Column5:
                case Location.Column6:
                case Location.Column7:
                    var column = this.columns[(int)source - (int)Location.Column0];
                    cards.AddRange(column.Skip(column.Count - (int)moveSize));
                    column.RemoveRange(column.Count - (int)moveSize, (int)moveSize);
                    break;

                case Location.Foundation:
                    throw new ArgumentException($"source location cannot be '{Location.Foundation}'");

                default:
                    throw new Exception($"enum member '{source}' missing in {nameof(MakeMove)}");
            }

            switch (destination)
            {
                case Location.Cell0:
                case Location.Cell1:
                case Location.Cell2:
                case Location.Cell3:
                    {
                        if (cards.Count != 1)
                        {
                            throw new InvalidOperationException($"cells can only hold one card, attempting '{cards.Count}'");
                        }

                        this.cells[(int)destination - (int)Location.Cell0] = cards.First();
                        break;
                    }

                case Location.Column0:
                case Location.Column1:
                case Location.Column2:
                case Location.Column3:
                case Location.Column4:
                case Location.Column5:
                case Location.Column6:
                case Location.Column7:
                    var column = this.columns[(int)destination - (int)Location.Column0];
                    column.AddRange(cards);
                    break;

                case Location.Foundation:
                    {
                        if (cards.Count != 1)
                        {
                            throw new InvalidOperationException($"foundations accept one card at a time, attempting '{cards.Count}'");
                        }

                        var card = cards.First();
                        Stack<Card> foundation = this.FindFoundationFor(card.Suit);

                        foundation.Push(card);
                        break;
                    }

                default:
                    throw new Exception($"enum member '{destination}' missing in {nameof(MakeMove)}");
            }
        }

        /// <summary>
        /// Check for and perform just one move to the foundations.
        /// </summary>
        /// <returns><c>true</c> if a move happened.</returns>
        internal bool AutoMoveToFoundation()
        {
            bool Check(Card card)
            {
                if (card.Rank == Rank.Ace)
                {
                    return true;
                }
                else
                {
                    var ownFoundation = this.FindFoundationFor(card.Suit);
                    if (ownFoundation.Count != 0 && ownFoundation.Peek().Rank == card.Rank - 1)
                    {
                        Stack<Card> otherFoundationSameColor;

                        switch (card.Suit)
                        {
                            case Suit.Clubs:
                                otherFoundationSameColor = this.FindFoundationFor(Suit.Spades);
                                break;

                            case Suit.Spades:
                                otherFoundationSameColor = this.FindFoundationFor(Suit.Clubs);
                                break;

                            case Suit.Hearts:
                                otherFoundationSameColor = this.FindFoundationFor(Suit.Diamonds);
                                break;

                            case Suit.Diamonds:
                                otherFoundationSameColor = this.FindFoundationFor(Suit.Hearts);
                                break;

                            default:
                                throw new Exception($"enum member '{card.Suit}' missing in {nameof(AutoMoveToFoundation)}");
                        }

                        var otherFoundationSameColorRank = otherFoundationSameColor.Count != 0 ? (int)otherFoundationSameColor.Peek().Rank : -1;

                        Stack<Card>[] foundationsOtherColor;
                        if (card.IsBlack)
                        {
                            foundationsOtherColor = new Stack<Card>[]
                            {
                                this.FindFoundationFor(Suit.Hearts),
                                this.FindFoundationFor(Suit.Diamonds),
                            };
                        }
                        else
                        {
                            foundationsOtherColor = new Stack<Card>[]
                            {
                                this.FindFoundationFor(Suit.Clubs),
                                this.FindFoundationFor(Suit.Spades),
                            };
                        }

                        var otherMinRank = foundationsOtherColor
                            .Select(x => x.Count != 0 ? (int)x.Peek().Rank : -1)
                            .Min();

                        var ownFoundationRank = (int)ownFoundation.Peek().Rank;
                        if (ownFoundationRank - otherMinRank < 2 && (ownFoundationRank <= otherMinRank || otherMinRank - otherFoundationSameColorRank < 2))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            for (int columnIndex = 0; columnIndex < 8; columnIndex++)
            {
                var card = this.columns[columnIndex].LastOrDefault();

                if (card == null)
                {
                    continue;
                }

                bool result = Check(card);

                if (result)
                {
                    this.MakeMove(Location.Column0 + columnIndex, Location.Foundation, moveSize: 1);
                    return true;
                }
            }

            for (int cellIndex = 0; cellIndex < 4; cellIndex++)
            {
                var card = this.cells[cellIndex];

                if (card == null)
                {
                    continue;
                }

                bool result = Check(card);

                if (result)
                {
                    this.MakeMove(Location.Cell0 + cellIndex, Location.Foundation, moveSize: 1);
                    return true;
                }
            }

            return false;

        }

        private static bool IsColumnLocation(Location location)
        {
            return location == Location.Column0 ||
                location == Location.Column1 ||
                location == Location.Column2 ||
                location == Location.Column3 ||
                location == Location.Column4 ||
                location == Location.Column5 ||
                location == Location.Column6 ||
                location == Location.Column7;
        }

        private static Game ParseFromRepresentation(string representation, Func<string, Card> parseFunc)
        {
            void ParseCellsFoundationsInto(Game target, string input, Func<string, Card> parser)
            {
                for (int cellIndex = 0; cellIndex < 4; cellIndex++)
                {
                    int index = 1 + cellIndex * 4;
                    var cardRepresentation = input.Substring(index, 2);
                    if (cardRepresentation != "..")
                    {
                        target.cells[cellIndex] = parser(cardRepresentation);
                    }
                }

                for (int foundationIndex = 0; foundationIndex < 4; foundationIndex++)
                {
                    int index = 19 + foundationIndex * 4;
                    var cardRepresentation = input.Substring(index, 2);
                    if (cardRepresentation != "..")
                    {
                        var card = parser(cardRepresentation);
                        var foundation = target.foundations[foundationIndex];

                        for (Rank rank = 0; rank <= card.Rank; rank++)
                        {
                            foundation.Push(new Card(card.Suit, rank));
                        }
                    }
                }
            }

            void ParseColumnsInto(Game target, string[] inputs, Func<string, Card> parser)
            {
                foreach (var input in inputs)
                {
                    for (int columnIndex = 0; columnIndex < 8; columnIndex++)
                    {
                        int index = 2 + columnIndex * 4;
                        var cardRepresentation = input.Substring(index, 2);
                        if (cardRepresentation != "  ")
                        {
                            target.columns[columnIndex].Add(parser(cardRepresentation));
                        }
                    }
                }
            }

            var game = new Game();
            var lines = representation.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            ParseCellsFoundationsInto(game, lines[0], parseFunc);
            ParseColumnsInto(game, lines.Skip(2).ToArray(), parseFunc);

            return game;
        }

        private void InitGame(uint gameId)
        {
            // create deck. Cards in correct order by their defintion
            var deck = new List<Card>(52);
            for (uint i = 0; i < 52; i++)
            {
                deck.Add(new Card(i));
            }

            int columnIndex = 0;
            var prng = new Prng();
            prng.Initialize(gameId);

            while (deck.Any())
            {
                var index = (int)(prng.GetNext() % deck.Count);

                // swap and pop, add to column
                this.columns[columnIndex].Add(deck[index]);
                deck[index] = deck.Last();
                deck.RemoveAt(deck.Count - 1);

                columnIndex = (columnIndex + 1) % 8;
            }
        }

        private string CreateRepresentation(Func<Card, string> cardRepresentation)
        {
            string GetCellRepresentations(Func<Card, string> representation)
            {
                string GetCellRepresentation(Card cell)
                {
                    return cell != null ? representation(cell) : "..";
                }

                return " " + string.Join("  ", this.cells.Select(GetCellRepresentation));
            }

            string GetFoundationRepresentations(Func<Card, string> representation)
            {
                string GetFoundationRepresentation(Stack<Card> foundation)
                {
                    return foundation.Any() ? representation(foundation.Peek()) : "..";
                }

                return " " + string.Join("  ", this.foundations.Select(GetFoundationRepresentation));
            }

            string GetColummRepresentations(Func<Card, string> representation)
            {
                string GetColumnLineRepresentation(List<Card> column, uint line)
                {
                    return column.Count > line ? representation(column[(int)line]) : "  ";
                }

                var maxCount = this.columns.Max(column => column.Count);

                var sb = new StringBuilder();

                for (uint line = 0; line < maxCount; line++)
                {
                    for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                    {
                        sb.Append($"  {GetColumnLineRepresentation(this.columns[columnIndex], line)}");
                    }

                    if (line + 1 < maxCount)
                    {
                        sb.Append("\r\n");
                    }
                }

                return sb.ToString();
            }

            return GetCellRepresentations(cardRepresentation) + " ||" + GetFoundationRepresentations(cardRepresentation) + "\r\n"
                + " --------------------------------\r\n"
                + GetColummRepresentations(cardRepresentation);
        }

        private Stack<Card> FindFoundationFor(Suit suit)
        {
            switch (suit)
            {
                case Suit.Clubs:
                    return this.foundations[0];
                case Suit.Diamonds:
                    return this.foundations[3];
                case Suit.Hearts:
                    return this.foundations[2];
                case Suit.Spades:
                    return this.foundations[1];

                default:
                    throw new Exception($"enum member '{suit}' missing in {nameof(FindFoundationFor)}");
            }
        }

    }
}
