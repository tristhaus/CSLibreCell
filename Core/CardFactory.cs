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

namespace Core
{
    /// <summary>
    /// Helper class to create cards.
    /// </summary>
    internal class CardFactory
    {
        private static readonly Dictionary<Rank, string> RankMapTemplate = new Dictionary<Rank, string>
        {
            { Rank.Two, "2"},
            { Rank.Three, "3"},
            { Rank.Four, "4"},
            { Rank.Five, "5"},
            { Rank.Six, "6"},
            { Rank.Seven, "7"},
            { Rank.Eight, "8"},
            { Rank.Nine, "9"},
        };

        private readonly Dictionary<Rank, string> rankMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardFactory"/> class.
        /// </summary>
        /// <param name="config">The configuration of the rank representation (5 chars in length).</param>
        internal CardFactory(string config = null)
        {
            if (config != null)
            {
                if (config.Length != 5)
                {
                    throw new ArgumentException($"if {nameof(config)} is provided, it must be five characters long, it is: '{config}'");
                }

                this.rankMap = new Dictionary<Rank, string>(RankMapTemplate)
                {
                    { Rank.Ace, config.Substring(0, 1) },
                    { Rank.King, config.Substring(1, 1) },
                    { Rank.Queen, config.Substring(2, 1) },
                    { Rank.Jack, config.Substring(3, 1) },
                    { Rank.Ten, config.Substring(4, 1) }
                };
            }
        }

        /// <summary>
        /// Create a deck in the correct order.
        /// </summary>
        /// <returns>The newly created deck.</returns>
        /// <remarks>Cards in correct order by their defintion.</remarks>
        internal List<Card> CreateDeck() // todo: front-end config
        {
            var deck = new List<Card>(52);
            for (uint i = 0; i < 52; i++)
            {
                deck.Add(new Card(i, this.rankMap));
            }

            return deck;
        }
    }
}
