/*
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
using Terminal.Gui;

namespace CSLibreCell.Internal
{
    /// <summary>
    /// Extended <see cref="Label"/> to allow injecting of actions
    /// to take on certain mouse events beyond the single click.
    /// </summary>
    internal class ColumnLabel : Label
    {
        private readonly Action onSingleClick;
        private readonly Action onMultipleClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnLabel"/> class.
        /// </summary>
        /// <param name="text">The starting text of the label, confer <see cref="Label.Label(NStack.ustring)"/></param>
        /// <param name="onSingleClick">Action to take when a single left click happened.</param>
        /// <param name="onMultipleClick">Action to take when a multiple left click happened.</param>
        public ColumnLabel(NStack.ustring text, Action onSingleClick, Action onMultipleClick) : base(text)
        {
            this.onSingleClick = onSingleClick;
            this.onMultipleClick = onMultipleClick;
        }

        /// <inheritdoc />
        public override bool OnMouseEvent(MouseEvent mouseEvent)
        {
            if (mouseEvent.Flags.HasFlag(MouseFlags.Button1Clicked))
            {
                this.onSingleClick();
                return true;
            }
            else if (mouseEvent.Flags.HasFlag(MouseFlags.Button1DoubleClicked) || mouseEvent.Flags.HasFlag(MouseFlags.Button1TripleClicked))
            {
                this.onMultipleClick();
                return true;
            }
 
            return base.OnMouseEvent(mouseEvent);
        }
    }
}
