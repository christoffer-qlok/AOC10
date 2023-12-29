using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
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
            CharMap = new char[lines.Length + 2][];
            for (int i = -1; i <= lines.Length; i++)
            {
                if(i == -1 || i == lines.Length)
                {
                    CharMap[i + 1] = new string('.', lines[0].Length + 2).ToCharArray();
                    continue;
                }
                CharMap[i+1] = ('.' + lines[i] + '.').ToCharArray();
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

        private Map() {}

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

        public Map GetZoomedMap()
        {
            var zY = new char[CharMap.Length * 2][];

            for (int y = 0; y < zY.Length; y++)
            {
                if(y % 2 == 0)
                {
                    zY[y] = CharMap[y/2];
                    continue;
                }

                int origY = (y - 1) / 2;
                int nextY = (y + 1) / 2;
                zY[y] = new char[CharMap[origY].Length];
                for (int x = 0; x < CharMap[origY].Length; x++)
                {
                    var c = CharMap[origY][x];
                    if ("|7F".Contains(c))
                    {
                        zY[y][x] = '|';
                    } else if(c == 'S' && "|LJ".Contains(CharMap[nextY][x]))
                    {
                        zY[y][x] = '|';
                    } else
                    {
                        zY[y][x] = '.';
                    }
                }
            }

            var zX = new char[zY.Length][];
            for (int y = 0; y < zX.Length; y++)
            {
                zX[y] = new char[zY[y].Length * 2];
                for (int x = 0; x < zX[y].Length; x++)
                {
                    if(x % 2 == 0)
                    {
                        zX[y][x] = zY[y][x / 2];
                        continue;
                    }

                    int origX = (x - 1) / 2;
                    int nextX = (x + 1) / 2;

                    var c = zY[y][origX];

                    if("-FL".Contains(c))
                    {
                        zX[y][x] = '-';
                    } 
                    else if(c == 'S' && "-J7".Contains(zY[y][nextX]))
                    {
                        zX[y][x] = '-';
                    } else
                    {
                        zX[y][x] = '.';
                    }
                }
            }

            return new Map() { CharMap = zX, CurrentPos = new Coord(CurrentPos.Y * 2, CurrentPos.X * 2) };

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

        public void NavigateConnected(Coord start)
        {
            var stack = new Stack<Coord>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (Visited.Contains(current)) continue;
                Visited.Add(current);
                foreach (var neighbour in GetNeighbours(current))
                {
                    if(!Visited.Contains(neighbour))
                    {
                        stack.Push(neighbour);
                    }
                }
            }
        }

        private Coord[] GetNeighbours(Coord pos)
        {
            int maxY = CharMap.Length - 1;
            int maxX = CharMap[0].Length - 1;

            var directions = new Coord[] { new Coord(1, 0), new Coord(-1, 0), new Coord(0, 1), new Coord(0, -1) };
            var res = new List<Coord>();
            foreach (var direction in directions)
            {
                var newCoord = pos.Move(direction);

                if (newCoord.X < 0 || newCoord.X > maxX || newCoord.Y < 0 || newCoord.Y > maxY)
                {
                    continue; 
                }

                if(!Path.Contains(newCoord))
                {
                    res.Add(newCoord);
                }
            }
            return res.ToArray();
        }

        public void PrintMap()
        {
            for (int y = 0; y < CharMap.Length; y++)
            {
                for (int x = 0; x < CharMap[0].Length; x++)
                {
                    var pos = new Coord(y, x);
                    if (CharMap[y][x] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (Path.Contains(pos))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (Visited.Contains(pos))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    } 
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write(CharMap[y][x]);
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}
