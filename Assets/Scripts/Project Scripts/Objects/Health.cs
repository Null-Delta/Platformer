using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    float hp { get; set;}
    float immortalTime { set; get;}
    float nowImmortalTime { set; get;}

    public void getDamage(float damage);
    public void onGetDamage(float damage);
    public void onDeath();

}
