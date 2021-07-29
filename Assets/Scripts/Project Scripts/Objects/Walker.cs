using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MapObject
{
    public Vector2 linearMove;
    public float sum_time;
    public float move_delay;
    public float animation_time;
    bool in_animation = false;
    public Vector2 moving_vector;
    Vector2 start_position;
    public Vector2 tmpSpeed;
    bool is_ready = true;
    public bool fict_move = false;
    public List<Vector2> taked_points;
    public bool halfMove = false;
    public bool useMovingVector = true;
    public override string objectName => "Walker";

    float healthPoint= 100;
    float immortalTime = 0.2f;
    float immortalTimeNow = 0;
    
    public void getDamage(float hp)
    {
        if (immortalTimeNow <=0)
        {
            healthPoint -=hp;
            immortalTimeNow = immortalTime;
        }
    }


    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        isCollisiable = true;
        order = ObjectOrder.wall;
    }
    public virtual bool readyCheck()
    {
        return true;
    }

    public virtual void onWalkStart()
    {
        var tmpVector = position+linearMove;
        if(map.getMapObjects<MapObject>((int)tmpVector.x, (int)tmpVector.y, x => x.isCollisiable == true) != null)
        {
            linearMove.x = -linearMove.x;
            linearMove.y = -linearMove.y;
        }
        var tmpList =map.getMapObjects<MovingFloor>((int)tmpVector.x, (int)tmpVector.y, x => x is MovingFloor) ;
        if(tmpList != null)
        {
            tmpList[0].addWalkerOn(tmpVector, this);
        }
        
    }

    public virtual void onWalkAnimation(float time)
    {
    }

    public virtual void onWalkFinish()
    {
        
    }

    public override void updateObject(float time) {
        
        is_ready = readyCheck();
        sum_time+=time; //важно

        if (immortalTimeNow >0)
        {
            immortalTimeNow-=time;
        }
        if (healthPoint <=0)
        {
            is_ready = false;
            map.destroyObject(this);
        }
        
        if (sum_time >= move_delay && !in_animation && is_ready)
        {
            sum_time = move_delay;
            in_animation = true;
            fict_move = false;
            
            halfMove=true;
            onWalkStart();
            
            moving_vector = linearMove + position;
            if (moving_vector == position)
                fict_move = true;

            if (!fict_move) 
            {
                taked_points.Add(new Vector2((int)moving_vector.x ,(int) moving_vector.y ));
                map.insertMapObject(new Vector2((int)moving_vector.x, moving_vector.y ), this);
            }         
        }
        
        if (in_animation)
        {
            if(sum_time >= move_delay + animation_time) 
            {
                sum_time = 0f;
                in_animation = false;

                if (useMovingVector)
                {
                    position = moving_vector;
                    gameObject.transform.position = moving_vector;
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 2);
                }

                var tmpPress = map.getMapObjects<OnPressObject>((int)position.x,(int)position.y, x=> x is OnPressObject);
                if (tmpPress != null)
                {
                    var iterPress = tmpPress.GetEnumerator();
                    while (iterPress.MoveNext())
                    {
                        iterPress.Current.OnPress(this);         //нажимные объекты
                    }
                }   
                
                onWalkFinish();
            } 
            else 
            {
                if (halfMove && sum_time*2 >= move_delay + animation_time)
                {
                    if (!fict_move)
                    {
                        map.removeMapObject(taked_points[0], this);
                        taked_points.Remove(taked_points[0]); 
                    }
                    halfMove = false;
                }
                
                tmpSpeed =linearMove * (time / animation_time);
                position += tmpSpeed;
                gameObject.transform.position += new Vector3(tmpSpeed.x , tmpSpeed.y, 0);

                //gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 2);

                onWalkAnimation(time);
            }
            
        }
    }


    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        if(obj is Bullet)
        {
            this.getDamage(10);
        }
    }




    public Walker(float x, float y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();

        linearMove.x = Random.Range(-1,2);
            if (Mathf.Abs(linearMove.x) == 0)
                linearMove.y = Random.Range(0,2)*2-1;

        move_delay = Random.Range(0.1f,0.5f);
        animation_time = Random.Range(0.1f,0.5f);
    }
}
