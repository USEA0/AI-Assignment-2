// Boris Au - 100660279
// INFR4320 Fall 2020
// AI manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // ai variables
    static private string marker;
    static private string opMarker;
    private string[,] boardCopy = new string[3, 3];
    private bool doMinMax = true;

    // determine best move
    public Vector2 Move(string[,] board, string aiMarker)
    {
        Vector2 bestMove = new Vector2();
        // set the ai's marker
        if (aiMarker == "X")
        {
            marker = aiMarker;
            opMarker = "O";
        }
        else
        {
            marker = aiMarker;
            opMarker = "X";
        }

        // call the countermove function and determine if a quick counter is possible
        Vector2 counterMove = counterLocation(board);
        if (counterMove == new Vector2(-1, -1))
        {
            doMinMax = true;
        }
        else
        {
            doMinMax = false;
            bestMove = counterMove;
        }

        if (doMinMax)
        {
            // minmax tree call
            int bestScore = -1000;
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    if (board[r, c] == " ")
                    {
                        // place marker at position
                        board[r, c] = marker;
                        // call minmax for min value
                        int score = minmax(board, 0, false);
                        // reset the board's value
                        board[r, c] = " ";
                        // return the highest score
                        if (score > bestScore)
                        {
                            // set return vector for optimal move
                            bestScore = score;
                            bestMove = new Vector2(r, c);
                        }
                    }
                }
            }
        }
        return bestMove;
    }

    // minmax tree recursive fuction
    static public int minmax(string[,] board, int depth, bool isMax)
    {
        // determine the boards condition
        int condition = boardCheck(board);
        // ai win
        if (condition == 1)
            return 1;
        // player win
        if (condition == -1)
            return -1;
        // draw
        if (draw(board))
            return 0;

        // if ai is playing go for max (min for the "player")
        if (isMax)
        {
            int bestScore = -1000;
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    // find empty spots
                    if (board[r, c] == " ")
                    {
                        // place marker on spot
                        board[r, c] = marker;
                        // call for a minmax with min value (player move)
                        int score = minmax(board, depth + 1, false);
                        // reset board
                        board[r, c] = " ";
                        // find max score for return
                        score = Mathf.Max(score, bestScore);
                        bestScore = score;
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = 1000;
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    // find empty spots
                    if (board[r, c] == " ")
                    {
                        // place marker on spot
                        board[r, c] = opMarker;
                        // call minmax with max value (ai move)
                        int score = minmax(board, depth + 1, true);
                        // reset board
                        board[r, c] = " ";
                        // find min score for return
                        score = Mathf.Min(score, bestScore);
                        bestScore = score;
                    }
                }
            }
            return bestScore;
        }



    }

    // test the board and return the conditions
    static public int boardCheck(string[,] b)
    {
        for (int r = 0; r < 3; r++)
        {
            if (b[r, 0] == b[r, 1] && b[r, 1] == b[r, 2])
            {
                if (b[r, 0] == marker)
                    return +1;
                else if (b[r, 0] == opMarker)
                    return -1;
            }
        }

        for (int c = 0; c < 3; c++)
        {
            if (b[0, c] == b[1, c] && b[1, c] == b[2, c])
            {
                if (b[0, c] == marker)
                    return +1;
                else if (b[0, c] == opMarker)
                    return -1;
            }
        }

        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == marker)
                return +1;
            else if (b[0, 0] == opMarker)
                return -1;
        }

        if (b[0, 2] == b[1, 1] && b[1, 1] == b[0, 2])
        {
            if (b[0, 2] == marker)
                return +1;
            else if (b[0, 2] == opMarker)
                return -1;
        }
        return 0;
    }

    // determine if the board is a draw
    static public bool draw(string[,] board)
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (board[r, c] == " ")
                    return false;
            }
        }
        return true;
    }

    // returns the location of the quick counter by calling the counter tester to see if there are any possible counters
    static public Vector2 counterLocation(string[,] board)
    {
        // if no counters are found, it will return a -1,-1 value which tells the ai to do the min max tree
        Vector2 pos = new Vector2(-1, -1);
        int loc = searchCounterLocation(board);
        if (loc == 0)
        {
            pos = new Vector2(0, 0);
        }
        else if (loc == 1)
        {
            pos = new Vector2(0, 1);
        }
        else if (loc == 2)
        {
            pos = new Vector2(0, 2);
        }
        else if (loc == 3)
        {
            pos = new Vector2(1, 0);
        }
        else if (loc == 4)
        {
            pos = new Vector2(1, 1);
        }
        else if (loc == 5)
        {
            pos = new Vector2(1, 2);
        }
        else if (loc == 6)
        {
            pos = new Vector2(2, 0);
        }
        else if (loc == 7)
        {
            pos = new Vector2(2, 1);
        }
        else if (loc == 8)
        {
            pos = new Vector2(2, 2);
        }
        else if (loc == 9)
        {
            pos = new Vector2(-1, -1);
        }
        return pos;
    }

    // returns the index of a quick counter location (if there is one)
    static public int searchCounterLocation(string[,] board)
    {
        List<int> state = new List<int>();

        // "translate" the 2d array to a 1d in array allowing for addition
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (board[r, c] == marker)
                    state.Add(1);
                else if (board[r, c] == opMarker)
                    state.Add(5);
                else
                    state.Add(0);

            }
        }

        // first test to see if theres an easy win for AI
        // adds up the 8 possible row combinations
        // any add up to 2 the value with a 0 is returned to the previous function to determine the position
        if (state[0] + state[1] + state[2] == 2)
        {
            if (state[0] == 0)
                return 0;
            else if (state[1] == 0)
                return 1;
            else if (state[2] == 0)
                return 2;
        }
        else if (state[3] + state[4] + state[5] == 2)
        {
            if (state[3] == 0)
                return 3;
            else if (state[4] == 0)
                return 4;
            else if (state[5] == 0)
                return 5;
        }
        else if (state[6] + state[7] + state[8] == 2)
        {
            if (state[6] == 0)
                return 6;
            else if (state[7] == 0)
                return 7;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[0] + state[3] + state[6] == 2)
        {
            if (state[0] == 0)
                return 0;
            else if (state[3] == 0)
                return 3;
            else if (state[6] == 0)
                return 6;
        }
        else if (state[1] + state[4] + state[7] == 2)
        {
            if (state[1] == 0)
                return 1;
            else if (state[4] == 0)
                return 4;
            else if (state[7] == 0)
                return 7;
        }
        else if (state[2] + state[5] + state[8] == 2)
        {
            if (state[2] == 0)
                return 2;
            else if (state[5] == 0)
                return 5;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[0] + state[4] + state[8] == 2)
        {
            if (state[0] == 0)
                return 0;
            else if (state[4] == 0)
                return 4;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[6] + state[4] + state[2] == 2)
        {
            if (state[6] == 0)
                return 6;
            else if (state[4] == 0)
                return 4;
            else if (state[2] == 0)
                return 2;
        }

        // then test for a counter on the player's positions
        // adds up the 8 possible row combinations
        // any add up to 10 the value with a 0 is returned to the previous function to determine the position
        if (state[0] + state[1] + state[2] == 10)
        {
            if (state[0] == 0)
                return 0;
            else if (state[1] == 0)
                return 1;
            else if (state[2] == 0)
                return 2;
        }
        else if (state[3] + state[4] + state[5] == 10)
        {
            if (state[3] == 0)
                return 3;
            else if (state[4] == 0)
                return 4;
            else if (state[5] == 0)
                return 5;
        }
        else if (state[6] + state[7] + state[8] == 10)
        {
            if (state[6] == 0)
                return 6;
            else if (state[7] == 0)
                return 7;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[0] + state[3] + state[6] == 10)
        {
            if (state[0] == 0)
                return 0;
            else if (state[3] == 0)
                return 3;
            else if (state[6] == 0)
                return 6;
        }
        else if (state[1] + state[4] + state[7] == 10)
        {
            if (state[1] == 0)
                return 1;
            else if (state[4] == 0)
                return 4;
            else if (state[7] == 0)
                return 7;
        }
        else if (state[2] + state[5] + state[8] == 10)
        {
            if (state[2] == 0)
                return 2;
            else if (state[5] == 0)
                return 5;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[0] + state[4] + state[8] == 10)
        {
            if (state[0] == 0)
                return 0;
            else if (state[4] == 0)
                return 4;
            else if (state[8] == 0)
                return 8;
        }
        else if (state[6] + state[4] + state[2] == 10)
        {
            if (state[6] == 0)
                return 6;
            else if (state[4] == 0)
                return 4;
            else if (state[2] == 0)
                return 2;
        }
        return 9;
    }
}
