using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    bool enemyVisible;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        Debug.Log("Collision!");
        if (collision.gameObject.CompareTag("Player"))
        {

            Debug.Log("Collision with PLAYER!");
            RaycastHit2D environmentCheck = Physics2D.Raycast(this.transform.position, (collision.transform.position - transform.position).normalized, 1200, ~LayerMask.GetMask("Enemy"));
            Debug.Log(environmentCheck.collider);
            if (environmentCheck.collider.CompareTag("Player"))
            {
                enemyVisible = true;
                transform.up = collision.gameObject.transform.position - transform.position;
            }
            else
            {
                enemyVisible = false;
            }
        }
    }
   
}
