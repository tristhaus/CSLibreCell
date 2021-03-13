using System.Collections.Generic;

namespace Core.Internal
{
    public interface IJourney
    {
        Stage Stage { get; }

        List<uint> Games { get; }
    }
}
