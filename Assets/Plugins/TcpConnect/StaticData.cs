using System.Collections.Generic;
using System.Linq;
using TcpConnect.ServerInterface;

namespace TcpConnect
{
    public class Cache<T> : Stack<T>
    {
        public Cache()
        {

        }

        private static IEnumerable<T> Reverse(List<T> list)
        {
            var result = list.ToList();
            result.Reverse(0, result.Count);
            return result;
        }

        public Cache(List<T> data) 
            : base(Reverse(data))
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
