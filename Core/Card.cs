﻿using System;
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
        /// <param name="id">The Id of the card as defined by the necessary order for FreeCell deals.</param>
        public Card(uint id)
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
            if (asciiRepresentation.Length != 2)
            {
                throw new ArgumentException($"length of representation must be 2, is {asciiRepresentation.Length} for {asciiRepresentation}");
            }

            var rankString = asciiRepresentation.Substring(0, 1);
            var rank = RankMap.First(x => x.Value == rankString).Key;

            var suitString = asciiRepresentation.Substring(1, 1);
            var suit = SuitAsciiMap.First(x => x.Value == suitString).Key;

            var id = (uint)rank * 4 + (uint)suit;

            return new Card(id);
        }
    }
}
