using System.Collections.Generic;
using UnityEngine;

public class GraphSearch {

    private List<List<int[]>> frontier;
    private List<int[]> explored;
    private FrameWork framework;
    private List<int[]> finalPath;

    private int problemType;

    // Constructor
    public GraphSearch(FrameWork problem, int type)
    {
        frontier = new List<List<int[]>>();
        frontier.Add(problem.getInitialStates());
        foreach (var element in problem.initialStates)
        {
            Debug.Log(element[0] + ", " + element[1] + " 3");
        }
        explored = new List<int[]>();
        framework = problem;
        finalPath = new List<int[]>();
    }

    // Algorithms here:
    private List<int[]> Bfs()
    {
        List<int[]> node = new List<int[]>();
        node = frontier[0];
        frontier.RemoveAt(0);
        return node;
    }

    private List<int[]> Dfs()
    {
        List<int[]> node = new List<int[]>();
        node = frontier[frontier.Count - 1];
        frontier.RemoveAt(frontier.Count - 1);
        return node;
    }

    private List<int[]> AStar()
    {
        List<int[]> node = new List<int[]>();
        node = frontier[frontier.Count - 1];
        frontier.RemoveAt(frontier.Count - 1);
        return node;
    }

    // Remove choise tells what algorithm will be used.
    private List<int[]> removeChoise()
    {
        List<int[]> node = new List<int[]>();

        switch (problemType)
        {
            case 1:
                // BfS
                node = Bfs();
                break;
            case 2:
                // DFS
                node = Dfs();
                break;
            case 3:
                // A* manhattan
                node = Bfs();
                break;
            case 4:
                // A* euclidean
                node = Bfs();
                break;
            default:
                // BDS as default if anything goes wrong
                node = Bfs();
                break;
        }

        return node;
    }

    private bool IsInList(List<int[]> list, int[] value)
    {
        foreach (var element in list)
        {
            if (element[0] == value[0] && element[1] == value[1])
            {
                return true;
            }
        }

        return false;
    }

    // JUST... DOOOO IIIIT!
    public List<int[]> JustDoIt()
    {
        explored = new List<int[]>();
        List<int[]> path = new List<int[]>();
        List<int[]> nextpath = new List<int[]>();
        int[] s = new int[2];
        while (true)
        {
            // There's something in the frontier
            if (frontier.Count > 0)
            {
                path = removeChoise();
                s = path[path.Count - 1];
                explored.Add(s);

                // Check for finish.
                if (framework.goalTest(s))
                {
                    return path;
                }

                // If not then explore frontier.
                List<char> actionss = framework.actions(s);
                int[] results = new int[2];
                foreach (var action in actionss)
                {
                    Debug.Log("Eureka! 1");
                    results = framework.result(s, action);

                    // Check if the result is explored
                    if (!IsInList(explored, results))
                    {
                        Debug.Log("Eureka! 2");
                        // If not then explore it.
                        nextpath = new List<int[]>();
                        foreach (var element in path)
                        {
                            nextpath.Add(element);
                        }
                        nextpath.Add(results);

                        // Check if it's already on the frontier.
                        if (!frontier.Contains(nextpath))
                        {
                            Debug.Log("Eureka! 3");
                            frontier.Add(nextpath);
                        }
                    }
                }
            }
        }
    }
	
}
