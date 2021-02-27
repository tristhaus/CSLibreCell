using Core.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class Game
    {
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
            this.InitGame(gameId);
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
