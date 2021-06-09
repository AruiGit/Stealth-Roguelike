using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    private Enemy enemy;
    private Player player;
    [SerializeField] GameObject spherePrefab;
    GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player_Character").GetComponent<Player>();
        StartCoroutine(AutoDestruction());

        if(transform.rotation.eulerAngles.z == 335.947f)
        {
            sphere = Instantiate(spherePrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f));
        }
        else
        {
            sphere = Instantiate(spherePrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90f));
        }
            

        
        spherePrefab.transform.parent = gameObject.transform;
        Debug.Log(transform.rotation.eulerAngles.z);
        
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
        float destructionTime = player.GetAttackTime();
        if (destructionTime < 0.3f)
        {
            destructionTime = 0.3f;
        }
        yield return new WaitForSeconds(destructionTime);
        Destroy(sphere);
        Destroy(this.gameObject);

    }
}
