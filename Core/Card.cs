using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Representation of a card in a standard 52 card deck.
    /// </summary>
    public class Card : IEquatable<Card>
    {
        private static readonly Dictionary<Suit, string> SuitAsciiMap = new Dictionary<Suit, string>
        {
            { Suit.Clubs, "C"},
            { Suit.Diamonds, "D"},
            { Suit.Hearts, "H"},
            { Suit.Spades, "S"},
        };

        private static readonly Dictionary<Suit, string> SuitUnicodeMap = new Dictionary<Suit, string>
        {
            { Suit.Clubs, "♣"},
            { Suit.Diamonds, "♦"},
            { Suit.Hearts, "♥"},
            { Suit.Spades, "♠"},
        };

        private static readonly Dictionary<Rank, string> RankMap = new Dictionary<Rank, string>
        {
            { Rank.Ace, "A"},
            { Rank.Two, "2"},
            { Rank.Three, "3"},
            { Rank.Four, "4"},
            { Rank.Five, "5"},
            { Rank.Six, "6"},
            { Rank.Seven, "7"},
            { Rank.Eight, "8"},
            { Rank.Nine, "9"},
            { Rank.Ten, "T"},
            { Rank.Jack, "J"},
            { Rank.Queen, "Q"},
            { Rank.King, "K"},
        };

        /// <summary>
        /// The Id of the card as defined by the necessary order for FreeCell deals.
        /// </summary>
        private readonly uint Id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="suit">The suit of the card.</param>
        /// <param name="rank">The rank of the card.</param>
        internal Card(Suit suit, Rank rank)
        {
            this.Id = (uint)rank * 4 + (uint)suit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="id">The Id of the card as defined by the necessary order for FreeCell deals.</param>
        internal Card(uint id)
        {
            if (id >= 52)
            {
                throw new ArgumentException($"id is {id}, must be smaller than 52");
            }

            this.Id = id;
        }

        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        public Suit Suit => (Suit)(this.Id % 4);

        /// <summary>
        /// Gets the rank of the card.
        /// </summary>
        public Rank Rank => (Rank)(this.Id / 4);

        /// <summary>
        /// Gets a value indicating whether the color of the card is black.
        /// </summary>
        public bool IsBlack => this.Suit == Suit.Clubs || this.Suit == Suit.Spades;

        /// <summary>
        /// Gets the AsciiRepresentation of the card.
        /// </summary>
        /// <remarks>
        /// Suits are denoted C, S, H, D. Ranks are denoted A, 2-9, T, J, Q, K.
        /// </remarks>
        internal string AsciiRepresentation
        {
            get
            {
                return RankMap[this.Rank] + SuitAsciiMap[this.Suit];
            }
        }

        public string UnicodeRepresentation
        {
            get
            {
                return RankMap[this.Rank] + SuitUnicodeMap[this.Suit];
            }
        }

        public override bool Equals(object other)
        {
            if(!(other is Card otherCard))
            {
                return false;
            }

            return this.Equals(otherCard);
        }

        public bool Equals(Card other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }

        /// <summary>
        /// Parses a card from the ASCII representation as available via <see cref="AsciiRepresentation"/>.
        /// </summary>
        /// <param name="asciiRepresentation">The input ASCII representation.</param>
        /// <returns>The corresponding card.</returns>
        internal static Card ParseFromAsciiRepresentation(string asciiRepresentation)
        {
            return ParseFromRepresentation(asciiRepresentation, SuitAsciiMap);
        }

        /// <summary>
        /// Parses a card from the ASCII representation as available via <see cref="UnicodeRepresentation"/>.
        /// </summary>
        /// <param name="unicodeRepresentation">The input Unicode representation.</param>
        /// <returns>The corresponding card.</returns>
        internal static Card ParseFromUnicodeRepresentation(string unicodeRepresentation)
        {
            return ParseFromRepresentation(unicodeRepresentation, SuitUnicodeMap);
        }

        private static Card ParseFromRepresentation(string representation, Dictionary<Suit, string> suitMap)
        {
            if (representation.Length != 2)
            {
                throw new ArgumentException($"length of representation must be 2, is {representation.Length} for {representation}");
            }

            var rankString = representation.Substring(0, 1);
            var rank = RankMap.First(x => x.Value == rankString).Key;

            var suitString = representation.Substring(1, 1);
            var suit = suitMap.First(x => x.Value == suitString).Key;

            return new Card(suit, rank);
        }
    }
}
