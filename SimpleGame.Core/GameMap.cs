using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Core
{
    public class GameMap
    {
        private static Random _random = new Random();
        private readonly InnerMap _innerMap;
        public int Size => _innerMap.Size;
        public int DefaultItemsCount { get; }
        public int NewItemValue { get; }
        public int[] AvailableDefaultItemsValues { get; }
        public bool IsGameOver { get; private set; }

        public GameMap(int size, int defaultItemsCount, int newItemValue, int[] availableDefaultItemsValues)
        {
            _innerMap = new InnerMap(size);
            DefaultItemsCount = defaultItemsCount;
            NewItemValue = newItemValue;
            AvailableDefaultItemsValues = availableDefaultItemsValues;
        }

        public int this[int x, int y]
        {
            get => _innerMap[x, y];
        }

        public void Start()
        {
            if (!TryAddItems())
                throw new InvalidOperationException("Cannot initialize game");
        }

        private bool TryAddItems()
        {
            var cells = GetEmptyCells().ToList();

            if (cells.Count < DefaultItemsCount)
                return false;

            for (int added = 0; added <= DefaultItemsCount; added++)
            {
                var index = _random.Next(0, cells.Count);
                var (x, y) = cells[index];
                cells.RemoveAt(index);
                _innerMap[x, y] = AvailableDefaultItemsValues[_random.Next(0, AvailableDefaultItemsValues.Length)];
            }

            return true;
        }

        private IEnumerable<(int x, int y)> GetEmptyCells()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (_innerMap[x, y] == 0)
                        yield return (x, y);
                }
            }
        }

        public bool MakeMove(int x, int y)
        {
            if (IsGameOver)
                return false;
            if (_innerMap[x, y] != 0)
                return false;


            bool stirred = false;
            stirred |= Stir(x, y, -1, 0);
            stirred |= Stir(x, y, 0, -1);
            stirred |= Stir(x, y, +1, 0);
            stirred |= Stir(x, y, 0, +1);

            return stirred;
        }

        // only one of dx, dy +1 or -1
        private bool Stir(int x, int y, int dx, int dy)
        {
            Debug.Assert(dx == 0 || dy == 0);
            Debug.Assert(Math.Abs(dx) == 1 || Math.Abs(dy) == 1);

            _innerMap[x, y] = NewItemValue;

            bool stirred = false;
            // iterate each x, y
            for (int x1 = x,
                     x2 = x + dx,
                     y1 = y,
                     y2 = y + dy
                     ; ;
                     x1 += dx,
                     x2 += dx,
                     y1 += dy,
                     y2 += dy)
            {
                int currentValue = _innerMap[x1, y1];
                int nextValue = _innerMap[x2, y2];

                if (nextValue < 0)
                    break;

                if (nextValue == 0)
                {
                    _innerMap[x2, y2] = currentValue;
                }
                else if (nextValue == currentValue)
                {
                    // doubled
                    // todo: func as property
                    _innerMap[x2, y2] = nextValue * 2;
                }
                else
                {
                    break;
                }
                stirred = true;
                _innerMap[x1, y1] = 0;
            }

            return stirred;
        }
    }
}
