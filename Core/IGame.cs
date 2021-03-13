using System.Collections.Generic;

namespace Core
{
    public interface IGame
    {
        uint Id { get; }

        bool IsWon { get; }

        bool IsImpossibleToWin { get; }

        IReadOnlyList<Card> Cells { get; }
        
        IReadOnlyList<Card> Foundations { get; }

        IReadOnlyList<IReadOnlyList<Card>> Columns { get; }
    }
}
