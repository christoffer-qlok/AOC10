using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC10
{
    internal struct Coord
    {
        public int X;
        public int Y;

        public Coord(int y, int x)
        {
            X = x;
            Y = y;
        }

        public Coord Move(Coord direction)
        {
            return new Coord(Y + direction.Y, X + direction.X);
        }

        public override string ToString()
        {
            return $"({Y},{X})";
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord(a.Y + b.Y, a.X + b.X);
        }
    }
}
