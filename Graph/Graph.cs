using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public interface IGraph<T>
    {
        List<List<string>> RoutesBetween(T source, T target, ILink<string>[] links);
    }

    public class Graph<T> : IGraph<T>
    {
        public Graph(IEnumerable<ILink<T>> links)
        {

        }


        public List<List<string>> RoutesBetween(T source, T target, ILink<string>[] links)
        {
            {
                List<string> routes = new List<string>();
                List<List<string>> listFinal = new List<List<string>>();

                foreach (var linha in links)
                {
                    linha.ToString().Replace(" -> ", ",");
                    string intermediario = linha.ToString().Replace(" -> ", ",");
                    routes.AddRange(intermediario.Split(','));
                }

                var index = Enumerable.Range(0, routes.Count)
                                        .Where(i => routes[i] == source.ToString())
                                        .ToList();
                foreach (int linha in index)
                {
                    if (linha % 2 == 0)
                    {
                        listFinal.Add(new List<string> { routes[linha], routes[linha + 1] });
                    }

                }
                index.Clear();
                bool looping = true;
                bool flgExit = false;

                int LastElement = 3;
                List<string> tempList = new List<string>();
                List<List<string>> InsertList = new List<List<string>>();

                do
                {
                    foreach (List<string> line in listFinal)
                    {
                        if (line.Last() == target.ToString())
                        {
                            InsertList.Add(line);
                        }
                        else
                        {
                            var exist = Enumerable.Range(0, line.Count)
                                            .Where(i => line[i] == line.Last())
                                            .ToList();
                            if (exist.Count() < 2)
                            {
                                exist.Clear();
                                exist = Enumerable.Range(0, routes.Count)
                                    .Where(i => routes[i] == line.Last())
                                    .ToList();
                                var countLoop = 0;
                                foreach (int lineLinks in exist)
                                {
                                    if (lineLinks % 2 == 0)
                                    {
                                        tempList.AddRange(line);
                                        tempList.Add(routes[lineLinks + 1]);
                                        countLoop++;
                                    }
                                }
                                exist.Clear();
                            }
                        }
                    }

                    if (tempList.Count != 0)
                    {
                        for (int i = 0; i < (tempList.Count / LastElement); i++)
                        {
                            if (i == 0)
                                InsertList.Add(tempList.GetRange(i, (LastElement)));
                            else
                                InsertList.Add(tempList.GetRange((i * LastElement), (LastElement)));
                        }
                    }
                    listFinal.Clear();
                    listFinal = new List<List<string>>(InsertList);
                    LastElement++;

                    if (flgExit || listFinal.Count == 0)
                        looping = false;

                    if (tempList.Count == 0)
                        flgExit = true;

                    InsertList.Clear();
                    tempList.Clear();
                } while (looping);

                return listFinal;
            }
        }
    }
}
