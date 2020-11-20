// Boris Au - 100660279
// INFR4320 Fall 2020
// Tic Tac Toe Game Manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // game variables
    private string meMarker;
    private string aiMarker;
    private string[,] gameState = new string[3, 3];
    private bool win = false;
    private string winner;
    private int turn = 1;
    private bool meTurn = false;
    private bool aiTurn = false;
    private bool gameActive = false;
    private Vector2 aiPos;

    // game object pieces
    public GameObject[] markers = new GameObject[9];

    // ui elements
    public Image begin;
    public Image end;
    public Text endText;
    public Button[] buttons = new Button[9];

    AI aiMove;

    // Start is called before the first frame update
    void Start()
    {
        aiMove = GameObject.FindObjectOfType<AI>();
        // populate array with non filled characters
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                gameState[r, c] = " ";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameActive)
        {
            // run ai if turn is < 10 and its their turn (skip first move)
            if (aiTurn && turn < 10 && turn != 1)
            {
                // call ai for next move position
                aiPos = aiMove.Move(gameState, aiMarker);
                gameState[(int)aiPos.x, (int)aiPos.y] = aiMarker;
                removeButton(aiPos);
                placeMarker(aiPos, turn, 2);
                turn++;
                meTurn = true;
                aiTurn = false;
            }
            // skip tree traversal if AI goes first
            else if(aiTurn && turn == 1)
            {
                // the optinmal move in an empty board is top left
                gameState[0, 0] = aiMarker;
                placeMarker(new Vector2(0,0), turn, 2);
                removeButton(new Vector2(0, 0));
                turn++;
                meTurn = true;
                aiTurn = false;
            }
            gameCheck();
        }

        // exit condition
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // allow user to pick side
    public void sidePick(int side)
    {
        // set marker types of each player and determine who goes first
        if (side == 1)
        {
            meMarker = "X";
            aiMarker = "O";
            meTurn = true;
            aiTurn = false;
        }
        else if (side == 2)
        {
            meMarker = "O";
            aiMarker = "X";
            meTurn = false;
            aiTurn = true;
        }
        // hide start screen
        begin.gameObject.SetActive(false);
        gameActive = true;
    }

    // handle user position input
    public void positionPick(string pos)
    {
        if(meTurn)
        {
            int b = (int)char.GetNumericValue(pos[0]);
            int r = (int)char.GetNumericValue(pos[1]);
            int c = (int)char.GetNumericValue(pos[2]);

            // draw marker depending on player setting
            if (meMarker == "O")
            {
                gameState[r, c] = "O";
            }
            else if (meMarker == "X")
            {
                gameState[r, c] = "X";
            }

            // call to place marker over position
            placeMarker(new Vector2(r, c), turn, 0);

            // hide used button
            buttons[b].gameObject.SetActive(false);

            // set ai turn
            aiTurn = true;
            meTurn = false;
            turn++;
        }
    }

    // test for all possible win conditions of tic tac toe
    void gameCheck()
    {
        for (int r = 0; r < 3; r++)
        {
            if (gameState[r, 0] == gameState[r, 1] && gameState[r, 1] == gameState[r, 2] && gameState[r, 0] != " ")
            {
                win = true;
                winner = gameState[r, 0];
            }
        }

        for (int c = 0; c < 3; c++)
        {
            if (gameState[0, c] == gameState[1, c] && gameState[1, c] == gameState[2, c] && gameState[0, c] != " ")
            {
                win = true;
                winner = gameState[0, c];
            }
        }

        if (gameState[0, 0] == gameState[1, 1] && gameState[1, 1] == gameState[2, 2] && gameState[0, 0] != " ")
        {
            win = true;
            winner = gameState[0, 0];
        }

        if (gameState[2, 0] == gameState[1, 1] && gameState[1, 1] == gameState[0, 2] && gameState[2, 0] != " ")
        {
            win = true;
            winner = gameState[2, 0];
        }

        if (win == true)
        {
            for(int x=0;x<9;x++)
            {
                buttons[x].gameObject.SetActive(false);
            }
            end.gameObject.SetActive(true);
            endText.text = winner + " Wins!";
        }
        else if (win == false && turn == 10)
        {
            end.gameObject.SetActive(true);
            endText.text = "DRAW";
        }

    }

    // positions the game marker on the position on the board
    void placeMarker(Vector2 pos, int turn, int player)
    {
        turn = turn - 1;
        
        if(pos.x == 0 && pos.y == 0)
        {
            markers[turn].transform.position = new Vector3(-3.3f, 2 + player, 3.3f);
        }
        else if(pos.x == 0 && pos.y == 1)
        {
            markers[turn].transform.position = new Vector3(0, 2 + player, 3.3f);
        }
        else if (pos.x == 0 && pos.y == 2)
        {
            markers[turn].transform.position = new Vector3(3.3f, 2 + player, 3.3f);
        }
        else if (pos.x == 1 && pos.y == 0)
        {
            markers[turn].transform.position = new Vector3(-3.3f, 2 + player, 0);
        }
        else if (pos.x == 1 && pos.y == 1)
        {
            markers[turn].transform.position = new Vector3(0, 2 + player, 0);
        }
        else if (pos.x == 1 && pos.y == 2)
        {
            markers[turn].transform.position = new Vector3(3.3f, 2 + player, 0);
        }
        else if (pos.x == 2 && pos.y == 0)
        {
            markers[turn].transform.position = new Vector3(-3.3f, 2 + player, -3.3f);
        }
        else if (pos.x == 2 && pos.y == 1)
        {
            markers[turn].transform.position = new Vector3(0, 2 + player, -3.3f);
        }
        else if (pos.x == 2 && pos.y == 2)
        {
            markers[turn].transform.position = new Vector3(3.3f, 2 + player, -3.3f);
        }
    }

    // removes button clicked by user or on positions ai places marker on
    void removeButton(Vector2 pos)
    {
        if (pos.x == 0)
        {
            if (pos.y == 0)
                buttons[0].gameObject.SetActive(false);
            else if (pos.y == 1)
                buttons[1].gameObject.SetActive(false);
            else if (pos.y == 2)
                buttons[2].gameObject.SetActive(false);
        }
        else if (pos.x == 1)
        {
            if (pos.y == 0)
                buttons[3].gameObject.SetActive(false);
            else if (pos.y == 1)
                buttons[4].gameObject.SetActive(false);
            else if (pos.y == 2)
                buttons[5].gameObject.SetActive(false);
        }
        else if (pos.x == 2)
        {
            if (pos.y == 0)
                buttons[6].gameObject.SetActive(false);
            else if (pos.y == 1)
                buttons[7].gameObject.SetActive(false);
            else if (pos.y == 2)
                buttons[8].gameObject.SetActive(false);
        }
    }
}
