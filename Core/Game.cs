using Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class Game
    {
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

        /// <summary>
        /// Gets the AsciiRepresentation of the game.
        /// </summary>
        /// <seealso cref="Card.AsciiRepresentation"/>
        internal string AsciiRepresentation
        {
            get
            {
                string GetCellRepresentations()
                {
                    string GetCellRepresentation(Card cell)
                    {
                        return cell != null ? cell.AsciiRepresentation : "..";
                    }

                    return " " + string.Join("  ", this.cells.Select(GetCellRepresentation));
                }

                string GetFoundationRepresentations()
                {
                    string GetFoundationRepresentation(Stack<Card> foundation)
                    {
                        return foundation.Any() ? foundation.Peek().AsciiRepresentation : "..";
                    }

                    return " " + string.Join("  ", this.foundations.Select(GetFoundationRepresentation));
                }

                string GetColummRepresentations()
                {
                    string GetColumnLineRepresentation(List<Card> column, uint line)
                    {
                        return column.Count > line ? column[(int)line].AsciiRepresentation : "  ";
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

                return GetCellRepresentations() + " ||" + GetFoundationRepresentations() + "\r\n"
                    + "---------------------------------\r\n"
                    + GetColummRepresentations();
            }
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
                    Stack<Card> foundation;
                    switch (card.Suit)
                    {
                        case Suit.Clubs:
                            foundation = this.foundations[0];
                            break;
                        case Suit.Diamonds:
                            foundation = this.foundations[3];
                            break;
                        case Suit.Hearts:
                            foundation = this.foundations[2];
                            break;
                        case Suit.Spades:
                            foundation = this.foundations[1];
                            break;

                        default:
                            throw new Exception($"enum member '{card.Suit}' missing in {nameof(MakeMove)}");
                    }

                    foundation.Push(card);
                    break;

                default:
                    throw new Exception($"enum member '{destination}' missing in {nameof(MakeMove)}");
            }
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
    }
}
