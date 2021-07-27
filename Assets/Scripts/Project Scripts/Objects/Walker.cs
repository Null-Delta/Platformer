using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MapObject
{
    public Vector2 linearMove;
    float sum_time;
    public float move_delay;
    public float animation_time;
    bool in_animation = false;
    Vector2 moving_vector;
    Vector2 start_position;
    bool is_ready = true;
    bool fict_move = false;
    public List<Vector2> taked_points;
    public override string objectName => "Walker";
    
    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;

        linearMove.x = Random.Range(-1,2);
            if (Mathf.Abs(linearMove.x) == 0)
                linearMove.y = Random.Range(0,2)*2-1;

        move_delay = Random.Range(0.1f,0.5f);
        animation_time = Random.Range(0.1f,0.5f);
    }
    public virtual bool readyCheck()
    {
        return true;
    }

    public virtual void onWalkStart()
    {
        if(map.getMapObjects<MapObject>((int)(position.x + linearMove.x),
             (int)(position.y + linearMove.y), x => x.isCollisiable == true || x as Walker != null) != null)
        {
            linearMove.x = -linearMove.x;
            linearMove.y = -linearMove.y;
        }
    }

    public virtual void onWalkAnimation()
    {
    }

    public virtual void onWalkFinish()
    {
        
    }

    public override void updateObject(float time) {
        
        is_ready = readyCheck();
        sum_time+=time; //важно
        
        if (sum_time >= move_delay && !in_animation && is_ready)
        {
            sum_time = move_delay;
            in_animation = true;
            fict_move = false;
            
            onWalkStart();

            moving_vector =new Vector2(linearMove.x,linearMove.y);
            start_position = position;

            if (moving_vector + start_position == position)
                fict_move = true;

            if (!fict_move) 
            {
                taked_points.Add(new Vector2((int)moving_vector.x + start_position.x,(int) moving_vector.y + start_position.y));
                map.insertMapObject(new Vector2((int)moving_vector.x + start_position.x,(int) moving_vector.y + start_position.y), this);
            }         
        }
        
        if (in_animation)
        {
            if(sum_time >= move_delay + animation_time) {
                sum_time = 0f;
                in_animation = false;
                position = moving_vector + start_position;

                if (!fict_move)
                {
                    map.removeMapObject(taked_points[0], this);
                    taked_points.Remove(taked_points[0]);
                }

                var tmpPress = map.getMapObjects<OnPressObject>((int)position.x,(int)position.y, x=> x is OnPressObject);
                if (tmpPress != null)
                {
                    var iterPress = tmpPress.GetEnumerator();
                    while (iterPress.MoveNext())
                    {
                        iterPress.Current.OnPress(this);
                    }
                }   

                onWalkFinish();

            } else {
                position = start_position + moving_vector * ((sum_time - move_delay) / animation_time);
            }
            onWalkAnimation();
            gameObject.transform.position = position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 2);
        }
    }

    public Walker(float x, float y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();
    }
}
