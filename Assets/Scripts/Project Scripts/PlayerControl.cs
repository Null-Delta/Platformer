using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Player CurrentPlayer;
    public bool ControlActive = true;
    InputHandler ih;

    
    string moveDirection;
    void Start()
    {
        ih = Camera.main.GetComponent<InputHandler>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0.1f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }


        if (ControlActive)
        {
            if (ih.ButtonRight())
            {   
                Move("Right");
            }  
            else if (ih.ButtonUp())
            {
                Move("Up");
            } 
            else if (ih.ButtonLeft())
            {
                Move("Left");
            }  
            else if (ih.ButtonDown())
            {
                Move("Down");
            }

            else if (Input.GetKeyDown(KeyCode.J))
            {   
                Attack("Right");
            }  
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                Attack("Up");
            } 
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Attack("Left");
            }  
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Attack("Down");
            }
                
        }
    }

    public void Move(string moveDirection)
    {
        movement move = new movement();
        move.isAnimate = true;

        switch (moveDirection)
        {
            case "Right":
                move.point = new Vector2Int(1, 0);
                break;
            case "Up":
                move.point = new Vector2Int(0, 1);
                break;
            case "Left":
                move.point = new Vector2Int(-1, 0);
                break;
            case "Down":
                move.point = new Vector2Int(0, -1);
                break;
        }

        CurrentPlayer.addMovement(move);
    }
    public void Attack(string moveDirection)
    {
        switch (moveDirection)
        {
            case "Right":
                CurrentPlayer.doAttack(new Vector2Int(1, 0));
                break;
            case "Up":
                CurrentPlayer.doAttack(new Vector2Int(0, 1));
                break;
            case "Left":
                CurrentPlayer.doAttack(new Vector2Int(-1, 0));
                break;
            case "Down":
                CurrentPlayer.doAttack(new Vector2Int(0, -1));
                break;
        }
    }
}
