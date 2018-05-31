using UnityEngine;
using System.Collections.Generic;

public class FrameWork {
    private char[,] matrix;
    private List<int[]> goalStates;
    public List<int[]> initialStates;
    private List<char> actionList;

    private int n;

    // Constructor
    public FrameWork(char[,] colorsMatrix, int matrixN)
    {
        matrix = colorsMatrix;
        goalStates = new List<int[]>();
        initialStates = new List<int[]>();
        actionList = new List<char>();
        n = matrixN;

        int[] state = new int[2];

        // Get initial and goal states
        for (var y = 0; y < n; y++)
        {
            for (var x = 0; x < n; x++)
            {
                state = new int[2];
                // Initial state
                if (matrix[x, y] == 'r')
                {
                    //Debug.Log("Eureka!");
                    state[0] = x;
                    state[1] = y;
                    initialStates.Add(state);
                }
                // Goal state(s)
                if (matrix[x, y] == 'g')
                {
                    //Debug.Log(x + ", " + y);
                    state[0] = x;
                    state[1] = y;
                    goalStates.Add(state);
                }
            }
        }
    }

    // actions(s) → {a1,a2 , .., an-1, an}
    public List<char> actions(int[] state)
    {
        // state is [x,y] of ints.
        int x = state[0];
        int y = state[1];

        // Clear any previous actions.
        actionList = new List<char>();
        // Check for each posible action base on the states
        if (y > 0 && matrix[x, y - 1] != 'b')
        {
            // You can move up.
            actionList.Add('u');
        }
        if (y < (n - 1) && matrix[x, y + 1] != 'b')
        {
            // You can move down.
            actionList.Add('d');
        }
        if (x > 0 && matrix[x - 1, y] != 'b')
        {
            // You can move left.
            actionList.Add('l');
        }
        if (x < (n - 1) && matrix[x + 1, y] != 'b')
        {
            // You can move right.
            actionList.Add('r');
        }

        return actionList;
    }

    // result(s, a) → s’
    public int[] result(int[] state, char action)
    {
        switch (action)
        {
            case 'u':
                state[1] += 1;
                break;
            case 'd':
                state[1] -= 1;
                break;
            case 'l':
                state[0] += 1;
                break;
            case 'r':
                state[0] -= 1;
                break;
            default:
                return state;
        }

        return state;
    }

    // goalTest(s) → {True, False}
    public bool goalTest(int[] state)
    {
        if (matrix[state[0], state[1]] == 'g')
        {
            //Debug.Log("Eureka! FINISH");
            return true;
        }
        return false;
    }

    // Helper state cost function
    public int stateCost(int[] state)
    {
        int cost = 0;

        switch (matrix[state[0], state[1]])
        {
            case 'r':
                cost = -1000;
                break;
            case 'g':
                cost = 0;
                break;
            case 'w':
                cost = 1;
                break;
            case 'b':
                cost = 1000;
                break;
            default:
                cost = 1000;
                break;
        }

        return cost;
    }

    // stepCost(s, a, s’) → R
    public int stepCost(int[] state, char action, int[] nextState)
    {
        int R = 0;

        return R = stateCost(state) + stateCost(nextState);
    }

    // pathCost(s1, s2 .. , sn) → R , para si = result(si-1, ai)
    public int pathCost(List<int[]> path)
    {
        int cost = 0;

        foreach (var state in path)
        {
            cost += stateCost(state);
        }

        return cost;
    }

    // Getters
    public char[,] getColorMatrix()
    {
        return matrix;
    }
    public List<int[]> getInitialStates()
    {
        return initialStates;
    }
    public List<int[]> getGoalStates()
    {
        return goalStates;
    }
    public List<char> getActionList()
    {
        return actionList;
    }
    public int getNSize()
    {
        return n;
    }
}
