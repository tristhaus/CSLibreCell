﻿/*
 * This file is part of CSLibreCell.
 * 
 * CSLibreCell is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * CSLibreCell is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with CSLibreCell.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

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

        private static readonly Dictionary<Rank, string> DefaultRankMap = new Dictionary<Rank, string>
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

        private readonly Dictionary<Rank, string> RankMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="suit">The suit of the card.</param>
        /// <param name="rank">The rank of the card.</param>
        /// <param name="rankMap">The map of rank to string representation to use, if not the default.</param>
        internal Card(Suit suit, Rank rank, Dictionary<Rank, string> rankMap = null)
        {
            this.Id = (uint)rank * 4 + (uint)suit;
            this.RankMap = rankMap ?? DefaultRankMap;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="id">The Id of the card as defined by the necessary order for FreeCell deals.</param>
        /// <param name="rankMap">The map of rank to string representation to use, if not the default.</param>
        internal Card(uint id, Dictionary<Rank, string> rankMap = null)
        {
            if (id >= 52)
            {
                throw new ArgumentException($"id is {id}, must be smaller than 52");
            }

            this.Id = id;
            this.RankMap = rankMap ?? DefaultRankMap;
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
        /// Gets the ASCII representation of the card.
        /// </summary>
        /// <remarks>
        /// Suits are denoted C, S, H, D. Ranks are denoted A, 2-9, T, J, Q, K.
        /// </remarks>
        internal string AsciiRepresentation
        {
            get
            {
                return this.RankMap[this.Rank] + SuitAsciiMap[this.Suit];
            }
        }

        /// <summary>
        /// Gets the Unicode representation of the card.
        /// </summary>
        /// <remarks>
        /// Suits are denoted by their symbols. Ranks are denoted A, 2-9, T, J, Q, K.
        /// </remarks>
        public string UnicodeRepresentation
        {
            get
            {
                return this.RankMap[this.Rank] + SuitUnicodeMap[this.Suit];
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object other)
        {
            if(!(other is Card otherCard))
            {
                return false;
            }

            return this.Equals(otherCard);
        }

        /// <summary>
        /// Indicates whether this instance and the specified card are equal.
        /// </summary>
        /// <param name="other">The other instance to compare.</param>
        /// <returns><c>true</c> if the <paramref name="other"/> card and this card represent the same.</returns>
        public bool Equals(Card other)
        {
            return this.Id == other.Id;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Id.ToString();
        }

        /// <summary>
        /// Parses a card from the ASCII representation as available via <see cref="AsciiRepresentation"/>.
        /// </summary>
        /// <param name="asciiRepresentation">The input ASCII representation.</param>
        /// <returns>The corresponding card.</returns>
        internal static Card ParseFromDefaultAsciiRepresentation(string asciiRepresentation)
        {
            return ParseFromDefaultRepresentation(asciiRepresentation, SuitAsciiMap);
        }

        /// <summary>
        /// Parses a card from the ASCII representation as available via <see cref="UnicodeRepresentation"/>.
        /// </summary>
        /// <param name="unicodeRepresentation">The input Unicode representation.</param>
        /// <returns>The corresponding card.</returns>
        internal static Card ParseFromDefaultUnicodeRepresentation(string unicodeRepresentation)
        {
            return ParseFromDefaultRepresentation(unicodeRepresentation, SuitUnicodeMap);
        }

        private static Card ParseFromDefaultRepresentation(string representation, Dictionary<Suit, string> suitMap)
        {
            if (representation.Length != 2)
            {
                throw new ArgumentException($"length of representation must be 2, is {representation.Length} for {representation}");
            }

            var rankString = representation.Substring(0, 1);
            var rank = DefaultRankMap.First(x => x.Value == rankString).Key;

            var suitString = representation.Substring(1, 1);
            var suit = suitMap.First(x => x.Value == suitString).Key;

            return new Card(suit, rank);
        }
    }
}
