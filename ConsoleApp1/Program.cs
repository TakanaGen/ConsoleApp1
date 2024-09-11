using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            // создаёт граф, представленный в виде словаря, где ключи — это вершины графа (вершина -> список ребер (сосед, вес(взяты произвольные числа веса)))
            Dictionary<int, List<(int, int)>> graph = new Dictionary<int, List<(int, int)>>()
        {
            {1, new List<(int, int)>{(2, 7), (6, 20)}},
            {2, new List<(int, int)>{(1, 7), (3, 10), (4, 15),(6, 12)}},
            {3, new List<(int, int)>{(2, 10), (4, 11), (5, 2)}},
            {4, new List<(int, int)>{(2, 15), (3, 11), (5, 6),(6,1)}},
            {5, new List<(int, int)>{(4, 6), (6, 9),(3,7)}},
            {6, new List<(int, int)>{(4, 1), (2, 12), (5, 9)}}
        };

            // Вызов алгоритма Дейкстры
            var shortestPath = Dijkstra(graph, 1, 6);
            //сохранение на рабочий стол
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "log.txt");
            using (StreamWriter log = new StreamWriter(filePath))
            {
                log.WriteLine("Кратчайший путь из вершины 1 в вершину 6:");
                foreach (var node in shortestPath)
                {
                    log.Write(node + " ");
                }
            }
        }

        static List<int> Dijkstra(Dictionary<int, List<(int, int)>> graph, int start, int end)
        {
            //priorityQueue - очеред с приоритетом
            //previous - хранит информацию о том, какая вершина была перед текущей вершиной
            //distances -  используется для хранения кратчайших расстояний от начальной вершины до каждой другой вершины в графе
            var distances = new Dictionary<int, int>();
            var previous = new Dictionary<int, int>();
            var priorityQueue = new SortedSet<(int, int)>();

            // реализует основную часть алгоритма Дейкстры для поиска кратчайшего пути в графе от одной вершины до другой
            foreach (var vertex in graph.Keys)
            {
                distances[vertex] = int.MaxValue;
                previous[vertex] = -1;
            }
            distances[start] = 0;
            priorityQueue.Add((0, start));

            while (priorityQueue.Count > 0)
            {
                var (currentDistance, currentVertex) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (currentVertex == end)
                    break;

                foreach (var (neighbor, weight) in graph[currentVertex])
                {
                    int distance = currentDistance + weight;

                    if (distance < distances[neighbor])
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor));
                        distances[neighbor] = distance;
                        previous[neighbor] = currentVertex;
                        priorityQueue.Add((distance, neighbor));
                    }
                }
            }

            // отвечает за восстановление кратчайшего пути от начальной вершины (start) до конечной вершины (end) после завершения алгоритма Дейкстры.
            var path = new List<int>();
            for (int at = end; at != -1; at = previous[at])
                path.Add(at);
            path.Reverse();

            return path;
        }
    }
}


