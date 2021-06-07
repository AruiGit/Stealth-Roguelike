using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public Animator playerAnimator;
    public SpriteRenderer playerSprite;

    [SerializeField]
    private Player player;

    [SerializeField] private GameObject prefabMeleeAttack;
    private MeleeAttack meleeAttack;
    bool canAttack = true;

    private float horizontal, vertical;
    bool isAttacking = false;

    Vector3 mousePosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isAttacking == false)
        {
            Movement();
        }
        
        if (canAttack == true)
        {
            CheckForAttack();
        }
        

        

    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        transform.Translate(new Vector2(horizontal, vertical) * player.GetMovementSpeed() * Time.deltaTime);
        if(horizontal!=0 || vertical != 0)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
        if (horizontal < 0)
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (horizontal > 0)
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        else if (vertical < 0)
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (vertical > 0)
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        
    }


    void CheckForAttack()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isAttacking = true;
            Attack(-1, 1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isAttacking = true;
            Attack(1, 1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isAttacking = true;
            Attack(1, -1);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isAttacking = true;
            Attack(-1, -1);
            canAttack = false;
        }
    }

    void Attack(int attackDirection, int orientatnion)
    {

        playerAnimator.SetBool("isAttacking",true);
        playerAnimator.SetFloat("meleeAttackSpeed", player.GetAttackSpeed());

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
        StartCoroutine(Attacking());
        yield return new WaitForSeconds(1/player.GetAttackSpeed());
        canAttack = true;
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(player.GetAttackTime());
        playerAnimator.SetBool("isAttacking",false);
        isAttacking = false;
    }

    

}
