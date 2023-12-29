using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC10
{
    internal class Map
    {
        public char[][] CharMap { get; set; }
        public Coord CurrentPos { get; set; }
        public Coord LastMove { get; set; }
        public HashSet<Coord> Path { get; set; } = new HashSet<Coord>();
        public HashSet<Coord> Visited { get; set; } = new HashSet<Coord>();

        public Map(string[] lines)
        {
            CharMap = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                CharMap[i] = lines[i].ToCharArray();
                for (int j = 0; j < CharMap[i].Length; j++)
                {
                    if (CharMap[i][j] == 'S')
                    {
                        CurrentPos = new Coord(i, j);
                        Path.Add(CurrentPos);
                    }
                }
            }
        }

        public int NavigatePipes()
        {
            int steps = 0;
            do
            {
                Move();
                steps++;
            } while (!IsAtStart());
            return steps;
        }

        public void Move()
        {
            Coord nextMove;
            switch(CharMap[CurrentPos.Y][CurrentPos.X])
            {
                case 'S':
                    nextMove = GetStartingPosMove();
                    break;
                case '|':
                    nextMove = LastMove;
                    break;
                case '-':
                    nextMove = LastMove;
                    break;
                case 'L':
                    nextMove = new Coord(LastMove.X, LastMove.Y);
                    break;
                case 'J':
                    nextMove = new Coord(LastMove.X * -1, LastMove.Y * -1);
                    break;
                case '7':
                    nextMove = new Coord(LastMove.X, LastMove.Y);
                    break;
                case 'F':
                    nextMove = new Coord(LastMove.X * -1, LastMove.Y * -1);
                    break;
                default:
                    throw new Exception($"Invalid tile: {CharMap[CurrentPos.Y][CurrentPos.X]} ({CurrentPos.Y},{CurrentPos.X})");
            }
            CurrentPos = CurrentPos.Move(nextMove);
            Path.Add(CurrentPos);
            LastMove = nextMove;
        }

        public bool IsAtStart()
        {
            return CharMap[CurrentPos.Y][CurrentPos.X] == 'S';
            
        }

        private Coord GetStartingPosMove()
        {
            if ("|7F".Contains(CharMap[CurrentPos.Y - 1][CurrentPos.X]))
            {
                return new Coord(-1, 0);
            }

            if ("|LJ".Contains(CharMap[CurrentPos.Y + 1][CurrentPos.X]))
            {
                return new Coord(1, 0);
            }

            if ("-J7".Contains(CharMap[CurrentPos.Y][CurrentPos.X - 1]))
            {
                return new Coord(0, -1);
            }

            if ("-LF".Contains(CharMap[CurrentPos.Y][CurrentPos.X + 1]))
            {
                return new Coord(0, 1);
            }

            throw new Exception("Bad starting position");
        }
    }
}
