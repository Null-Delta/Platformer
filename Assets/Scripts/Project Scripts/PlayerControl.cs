using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Player CurrentPlayer;
    InputHandler ih;

    float[] time = new float[4];
    int ind;
    void Start()
    {
        ih = Camera.main.GetComponent<InputHandler>();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            if(Time.timeScale == 1f) {
                Time.timeScale = 0.1f;
            } else {
                Time.timeScale = 1f;
            }
        }
        
        if(ih.ButtonUp())
            ind = 0;
        else if(ih.ButtonRight())
            ind = 1;
        else if(ih.ButtonDown())
            ind = 2;
        else if(ih.ButtonLeft())
            ind = 3;
        else
        {
            ind = -1;
            for(int i = 0; i < 4; i++)
                if (i != ind) time[i] = 0;
        } 

        if (ind >= 0)
        {
            for(int i = 0; i < 4; i++)
                if (i != ind) time[i] = 0;
            
            if(time[ind] == 0) {
                Move(ind);
            }
            time[ind] += Time.deltaTime;
            if(time[ind] > 0.2f)
            {
                time[ind] = 0.0001f;
                //Move(ind);
            }
        }
        
    }

    public void Move(int dir)
    {
        movement move = new movement();
        move.isAnimate = true;

        switch(dir) {
            case 0:
            move.point = new Vector2Int(0,1);
            break;
            case 1:
            move.point = new Vector2Int(1,0);
            break;
            case 2:
            move.point = new Vector2Int(0,-1);
            break;
            case 3:
            move.point = new Vector2Int(-1,0);
            break;
        }

        CurrentPlayer.addMovement(move);
        //CurrentPlayer.addDirection(dir);
    }
}
