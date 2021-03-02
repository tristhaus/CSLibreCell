using Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class Game
    {
        /// <summary>
        /// The ID of the game, if known, zero otherwise.
        /// </summary>
        private readonly uint id;
        private readonly Stack<Card>[] foundations = new Stack<Card>[4] { new Stack<Card>(), new Stack<Card>(), new Stack<Card>(), new Stack<Card>() };
        private readonly List<Card>[] columns = new List<Card>[8] { new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>() };

        private Card[] cells = new Card[4] { null, null, null, null };

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// filled with the game corresponding to the ID.
        /// </summary>
        /// <param name="gameId">The ID of the game to be created.</param>
        public Game(uint gameId)
        {
            this.id = gameId;
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
            this.foundations = other.foundations.Select(x => new Stack<Card>(x)).ToArray();
            this.columns = other.columns.Select(x => new List<Card>(x)).ToArray();
            this.cells = other.cells.ToArray();
        }

        private Game()
        {
            this.id = 0;
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

        /// <summary>
        /// Gets a value indicating whether the game is won.
        /// </summary>
        internal bool IsWon
        {
            get
            {
                return this.foundations.Sum(x => x.Count) == 52;
            }
        }

        internal static Game ParseFromUnicodeRepresentation(string unicodeRepresentation)
        {
            return ParseFromRepresentation(unicodeRepresentation, x => Card.ParseFromUnicodeRepresentation(x));
        }

        internal bool IsMoveLegal(Location source, Location destination)
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

            bool SimpleChecks()
            {
                // no move if source equals destination
                if (source == destination)
                {
                    return false;
                }

                // no move from foundation
                if (source == Location.Foundation)
                {
                    return false;
                }

                // no move from empty cell
                if ((source == Location.Cell0 ||
                    source == Location.Cell1 ||
                    source == Location.Cell2 ||
                    source == Location.Cell3)
                    && this.cells[(int)source - (int)Location.Cell0] == null)
                {
                    return false;
                }

                // no move from empty column
                if ((source == Location.Column0 ||
                    source == Location.Column1 ||
                    source == Location.Column2 ||
                    source == Location.Column3 ||
                    source == Location.Column4 ||
                    source == Location.Column2 ||
                    source == Location.Column5 ||
                    source == Location.Column6 ||
                    source == Location.Column7)
                    && this.columns[(int)source - (int)Location.Column0].Count == 0)
                {
                    return false;
                }

                // no move to filled cell
                if ((destination == Location.Cell0 ||
                    destination == Location.Cell1 ||
                    destination == Location.Cell2 ||
                    destination == Location.Cell3)
                    && this.cells[(int)destination - (int)Location.Cell0] != null)
                {
                    return false;
                }

                return true;
            }

            bool FoundationCheck()
            {
                if (destination == Location.Foundation)
                {
                    var card = GetSourceCard();

                    var foundation = this.FindFoundationFor(card.Suit);

                    if (card.Rank == Rank.Ace)
                    {
                        return foundation.Count == 0;
                    }
                    else
                    {
                        return foundation.Count != 0 && foundation.First().Rank + 1 == card.Rank;
                    }
                }

                return true;
            }

            bool ColumnCheck()
            {
                if (destination == Location.Column0 ||
                    destination == Location.Column1 ||
                    destination == Location.Column2 ||
                    destination == Location.Column3 ||
                    destination == Location.Column4 ||
                    destination == Location.Column2 ||
                    destination == Location.Column5 ||
                    destination == Location.Column6 ||
                    destination == Location.Column7)
                {
                    var destinationColumn = this.columns[(int)destination - (int)Location.Column0];

                    if (destinationColumn.Count != 0)
                    {
                        var sourceCard = GetSourceCard();

                        var destinationCard = destinationColumn.Last();

                        return sourceCard.IsBlack != destinationCard.IsBlack
                            && destinationCard.Rank == sourceCard.Rank + 1;
                    }

                }
            
                return true;
            }

            return SimpleChecks() && FoundationCheck() && ColumnCheck();
        }

        internal void MakeMove(Location source, Location destination)
        {
            Card card;

            switch (source)
            {
                case Location.Cell0:
                case Location.Cell1:
                case Location.Cell2:
                case Location.Cell3:
                    //todo: exception for when empty.
                    card = this.cells[(int)source - (int)Location.Cell0];
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
                    //todo: exception for when empty.
                    var column = this.columns[(int)source - (int)Location.Column0];
                    card = column.Last();
                    column.RemoveAt(column.Count - 1);
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
                    //todo: exception for when already occupied.
                    this.cells[(int)destination - (int)Location.Cell0] = card;
                    break;

                case Location.Column0:
                case Location.Column1:
                case Location.Column2:
                case Location.Column3:
                case Location.Column4:
                case Location.Column5:
                case Location.Column6:
                case Location.Column7:
                    //todo: exception for when not fitting.
                    var column = this.columns[(int)destination - (int)Location.Column0];
                    column.Add(card);
                    break;

                case Location.Foundation:
                    //todo: exception for when not fitting.
                    Stack<Card> foundation = this.FindFoundationFor(card.Suit);

                    foundation.Push(card);
                    break;

                default:
                    throw new Exception($"enum member '{destination}' missing in {nameof(MakeMove)}");
            }
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
                + "---------------------------------\r\n"
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
