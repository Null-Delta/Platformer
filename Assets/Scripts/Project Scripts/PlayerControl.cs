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
            
            if(time[ind] == 0) Move(ind);
            time[ind] += Time.deltaTime;
            if(time[ind] > CurrentPlayer.animation_time)
            {
                time[ind] = 0.0001f;
                //Move(ind);
            }
        }
        
    }

    public void Move(int dir)
    {
        CurrentPlayer.addDirection(dir);
    }
}
