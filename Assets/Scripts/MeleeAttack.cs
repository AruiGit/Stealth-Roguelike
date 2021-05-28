using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private Enemy enemy;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player_Character").GetComponent<Player>();
        StartCoroutine(AutoDestruction());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Trigger enter");
        if (collision.gameObject.tag == "Enemy")
        {

            enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(player.GetDamage());
        }
    }

    

    IEnumerator AutoDestruction()
    {

        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);

    }
}
