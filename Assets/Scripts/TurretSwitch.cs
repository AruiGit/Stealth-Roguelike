using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSwitch : MonoBehaviour
{
    [SerializeField] private List<Turret> turretList = new List<Turret>();
    public Sprite leverON, leverOFF;
    bool isLeverON = true;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach(Turret turret in turretList)
                {
                    turret.TurretOnOff();
                }

                if (isLeverON == true)
                {
                    sprite.sprite = leverOFF;
                    isLeverON = false;
                }
                else
                {
                    sprite.sprite = leverON;
                    isLeverON = true;
                }
            }
        }
    }
}
