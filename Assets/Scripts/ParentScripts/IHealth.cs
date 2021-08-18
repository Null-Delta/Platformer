using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float hp { get; set;}

    public void getDamage(float damage, float timeToStan, int typeOfDamage);
    public void onGetDamage(float damage);
    public void onDeath();

}
