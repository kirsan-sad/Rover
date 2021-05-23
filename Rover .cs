using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoverApp
{
    public class Rover
    {
        private const string Path = "path-plan.txt";
        private static void PrintPathPlan(Stack<Node> stack)
        {
            int steps = -1;
            int fuel = 0;

            try
            {
                using (var sr = new StreamWriter(Path, false))
                {
                    for (int i = 0; stack.Count != 0; i++)
                    {
                        var node = stack.Pop();
                        steps++;

                        sr.Write($"[{node.X}][{node.Y}]");
                        if (stack.Count != 0)
                            sr.Write("->");

                        if (i != 0)
                            fuel += node.Cost;
                    }

                    sr.WriteLine("");
                    sr.WriteLine($"steps: {steps}");
                    sr.WriteLine($"fuel: {fuel}");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void ExceptionHandler(string message)
        {
            try
            {
                using (var sr = new StreamWriter(Path, false))
                {
                    sr.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            throw new Exception(message);
        }

        private static void Validation(string[,] map)
        {
            if (map.Length == 0)
            {
                ExceptionHandler("Array is empty");
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!int.TryParse(map[i, j], out int num) && map[i, j].ToUpper() != "X")
                    {
                        ExceptionHandler("Array is invalid. Only numbers and the letter 'X' are allowed");
                    }
                    if (map[0, 0].ToUpper() == "X" || map[(map.GetLength(0) - 1), (map.GetLength(1) - 1)].ToUpper() == "X")
                    {
                        ExceptionHandler("Invalid start or finish");
                    }
                }
            }
        }

        public static void CalculateRoverPath(string[,] map)
        {
            Validation(map);

            var start = new Node();
            start.Y = 0;
            start.X = 0;
            start.Cost = Convert.ToInt32(map[0, 0]);

            var finish = new Node();
            finish.Y = map.GetLength(1) - 1;
            finish.X = map.GetLength(0) - 1;

            start.SetDistance(finish.X, finish.Y);

            var activeNodes = new List<Node>();
            activeNodes.Add(start);
            var visitedNodes = new List<Node>();

            var pathPlan = new Stack<Node>();

            while (activeNodes.Any())
            {
                var checkNode = activeNodes.OrderBy(x => x.CostDistance).First();

                if (checkNode.X == finish.X && checkNode.Y == finish.Y)
                {
                    var node = checkNode;

                    while (true)
                    {
                        pathPlan.Push(node);

                        node = node.Parent;

                        if (node == null)
                        {
                            PrintPathPlan(pathPlan);
                            return;
                        }    
                    }
                }

                visitedNodes.Add(checkNode);
                activeNodes.Remove(checkNode);

                var neighborNodes = GetneighborNodes(map, checkNode, finish);

                foreach (var neighborNode in neighborNodes)
                {
                    if (visitedNodes.Any(x => x.X == neighborNode.X && x.Y == neighborNode.Y))
                        continue;

                    if (activeNodes.Any(x => x.X == neighborNode.X && x.Y == neighborNode.Y))
                    {
                        var existingTile = activeNodes.First(x => x.X == neighborNode.X && x.Y == neighborNode.Y);
                        if (existingTile.CostDistance > checkNode.CostDistance)
                        {
                            activeNodes.Remove(existingTile);
                            activeNodes.Add(neighborNode);
                        }
                    }
                    else
                    {
                        activeNodes.Add(neighborNode);
                    }
                }

                ExceptionHandler("Unable to complete route");
            }
        }

        private static List<Node> GetneighborNodes(string[,] map, Node current, Node target)
        {
            var neighbors = new List<Node>()
            {
                new Node { X = current.X, Y = current.Y - 1, Parent = current},
                new Node { X = current.X, Y = current.Y + 1, Parent = current},
                new Node { X = current.X - 1, Y = current.Y, Parent = current},
                new Node { X = current.X + 1, Y = current.Y, Parent = current},
                new Node { X = current.X + 1, Y = current.Y + 1, Parent = current, IsDiagonal = true},
                new Node { X = current.X - 1, Y = current.Y + 1, Parent = current, IsDiagonal = true},
                new Node { X = current.X + 1, Y = current.Y - 1, Parent = current, IsDiagonal = true},
                new Node { X = current.X - 1, Y = current.Y - 1, Parent = current, IsDiagonal = true}
            };

            neighbors.ForEach(node => node.SetDistance(target.X, target.Y));

            var maxX = map.GetLength(0) - 1;
            var maxY = map.GetLength(1) - 1;

            var possibleNodes = neighbors.Where(node => node.X >= 0 && node.X <= maxX)
                                                .Where(node => node.Y >= 0 && node.Y <= maxY)
                                                .Where(node => map[node.X, node.Y].ToUpper() != "X")
                                                .ToList();

            possibleNodes.ForEach(node => node.SetCost(map, node));

            return possibleNodes;
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Node Parent { get; set; }
        public bool IsDiagonal { get; set; }

        public void SetDistance(int targetX, int targetY)
        {
            Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }

        public void SetCost(string[,] map, Node current)
        {
            var step = 1;
            int parentCost;

            if (current.IsDiagonal)
                step = 2;

            if (current.Parent.IsDiagonal)
                step = 1;

            var currentCost = Convert.ToInt32(map[current.X, current.Y]);

            if (current.Parent != null)
                parentCost = Convert.ToInt32(map[current.Parent.X, current.Parent.Y]);
            else
                parentCost = currentCost;

            if (parentCost == currentCost)
                Cost = + step;
            else if (parentCost > currentCost)
                Cost = parentCost - currentCost + step;
            else
                Cost = currentCost - parentCost + step;
        }
    }
}