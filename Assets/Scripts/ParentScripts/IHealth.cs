using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float hp { get; set;}
    float immortalTime { set; get;}
    float immortalTimeForHit {get;set;}

    public void getDamage(float damage);
    public void onGetDamage(float damage);
    public void onDeath();

}
