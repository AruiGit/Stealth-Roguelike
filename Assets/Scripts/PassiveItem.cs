using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public int damageChange, healthChange;
    public float damageMultipier, attackSpeedMultipier, movementSpeedMultipier;
    public int id;





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().ChangeHP(healthChange);
            collision.gameObject.GetComponent<Player>().ChangeDamage(damageChange, true);
            collision.gameObject.GetComponent<Player>().ChangeDamage((int)damageMultipier, false);
            collision.gameObject.GetComponent<Player>().ChangeAttackSpeed(attackSpeedMultipier);
            collision.gameObject.GetComponent<Player>().ChangeMovementSpeed(movementSpeedMultipier);
            collision.gameObject.GetComponent<Player>().AddPassiveItem(this.gameObject);

            this.gameObject.SetActive(false);
        }
    }



    //

}
