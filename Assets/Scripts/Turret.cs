﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Sprite turretON, turretOFF, turretIDLE;
    bool enemyVisible;
    bool isTurretON = true;
    bool canShoot = true;
    float bulletSpeed = 1f;
    float attackSpeed = 1f;
    public Transform bulletSpawn, bulletSpawn1;


    public GameObject bulletPrefab;
    bool cannonSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyVisible == false && isTurretON == true)
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite = turretIDLE;
        }
        if(enemyVisible==true && isTurretON==true && canShoot == true)
        {
            AttackPlayer();
            Debug.Log("Attacking Player");
            Debug.Log("enemyVisible: " + enemyVisible + " isTurretON: " + isTurretON + " canShoot: " + canShoot);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player")&& isTurretON==true)
        {

            RaycastHit2D environmentCheck = Physics2D.Raycast(this.transform.position, (collision.transform.position - transform.position).normalized, 1200, ~LayerMask.GetMask("Enemy"));
            Debug.Log(environmentCheck.collider);
            if (environmentCheck.collider.CompareTag("Player"))
            {
                enemyVisible = true;
                transform.up = collision.gameObject.transform.position - transform.position;
                Sprite sprite = GetComponent<SpriteRenderer>().sprite = turretON;
            }
            else
            {
                enemyVisible = false;
                
            }
        }
    }

    void AttackPlayer()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        Debug.Log("In Shoot IEnumerator");
        canShoot = false;
        if (cannonSwitch == true)
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            cannonSwitch = false;
        }
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletSpawn1.position, bulletSpawn1.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(newBullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            cannonSwitch = true;
        }
         // We use the right (or up in some cases) transform because forward in a 2D space is into the screen.
        yield return new WaitForSeconds(attackSpeed);
        Debug.Log("after waitforsecnd");
        canShoot = true;
    }

}
