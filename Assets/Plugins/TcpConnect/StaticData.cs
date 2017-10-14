using System.Collections.Generic;
using TcpConnect.ServerInterface;

namespace TcpConnect
{
    public class Cache<T> : Stack<T>
    {
        public Cache()
        {

        }

        public Cache(List<T> data)
            :base(data)
        {
            
        }

        public int Stop
        {
            get { return Start + Count - 1; }
        }

        public int Start { set; get; }
    }

    public static class StaticData
    {
        public static Cache<Eat> Eats = new Cache<Eat>();
        public static Cache<Diaper> Diapers = new Cache<Diaper>();
    }
}
