using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Core
{
    internal class InnerMap
    {
        private int[,] _map;
        public int Size { get; }

        public InnerMap(int size)
        {
            Size = size;
            _map = new int[size, size];
        }

        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Size || y < 0 || y >= Size)
                    return -1;
                return _map[x, y];
            }
            set
            {
                if (x < 0 || x >= Size || y < 0 || y >= Size)
                    return;
                _map[x, y] = value;
            }
        }
    }
}
