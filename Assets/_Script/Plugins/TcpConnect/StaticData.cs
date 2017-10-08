using System.Collections.Generic;
using TcpConnect.ServerInterface;

namespace TcpConnect
{
    public class Cache<T> : Stack<T>
    {
        public int Start { set; get; } = 0;

        public int Stop => Start + Count - 1;
    }

    public static class StaticData
    {
        public static Cache<Eat> Eats;
        public static Cache<Diaper> Diapers;
    }
}
