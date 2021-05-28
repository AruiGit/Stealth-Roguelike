using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{


    [SerializeField]
    private Player player;

    [SerializeField] private GameObject prefabMeleeAttack;
    private MeleeAttack meleeAttack;
    bool canAttack = true;

    private float horizontal, vertical;
    float speed = 1f;

    Vector3 mousePosition;

    private void Start()
    {
    }

    private void Update()
    {
        Movement();
        if (canAttack == true)
        {
            CheckForAttack();
        }
        

        

    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        transform.Translate(new Vector2(horizontal, vertical) * speed * Time.deltaTime);
    }


    void CheckForAttack()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Attack(-1, 1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Attack(1, 1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Attack(1, -1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Attack(-1, -1);
            canAttack = false;
        }
    }

    void Attack(int attackDirection, int orientatnion)
    {

        

        if (orientatnion > 0)
        {
            Vector3 attackDir = new Vector3(transform.position.x + attackDirection*player.GetAttackRange(), transform.position.y, -5);
            if (attackDirection > 0)
            {
                meleeAttack = Instantiate(prefabMeleeAttack, attackDir, Quaternion.Euler(0f, 0f, -24.053f)).GetComponent<MeleeAttack>();
            }
            else
            {
                meleeAttack = Instantiate(prefabMeleeAttack, attackDir, Quaternion.Euler(0f, 180f, -24.053f)).GetComponent<MeleeAttack>();
            }

            
        }
        else
        {
            Vector3 attackDir = new Vector3(transform.position.x, transform.position.y + attackDirection * player.GetAttackRange(), -5);
            if (attackDirection > 0)
            {
                meleeAttack = Instantiate(prefabMeleeAttack, attackDir, Quaternion.Euler(0f, 0f, 90f - 24.053f)).GetComponent<MeleeAttack>();
            }
            else
            {
                meleeAttack = Instantiate(prefabMeleeAttack, attackDir, Quaternion.Euler(0f, 0f, -90f - 24.053f)).GetComponent<MeleeAttack>();
            }
            
            
        }
       

         Debug.Log("Im Attacking");
        StartCoroutine(AttackSpeedCheck());
        
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    IEnumerator AttackSpeedCheck()
    {
        
        yield return new WaitForSeconds(1/player.GetAttackSpeed());
        canAttack = true;
    }

}
