using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Enemy : MonoBehaviour
{
    //Field Of View variables
    [SerializeField] Transform prefabFieldOfView;
    FieldOfView fieldofview;
    [SerializeField] private float fov = 90;
    [SerializeField] private float viewDistance = 2f;
    [SerializeField] private float followingViewDistance = 5f;
    [SerializeField] private float speed = 0.05f;
    private Vector3 lastKnowPlayerPosition;
    public Transform targetPosition;
    private Vector3 dir;
    [SerializeField] private List<Transform> PatrolPoints = new List<Transform>();
    private int patrolWaypoint = 0;
    private bool isTurningAround = false;
    bool wasPlayerSeen = false;
    Seeker seeker;
    public float nextWaypointDistance = 0.07f;
    private int currentWaypoint = 0;
    bool reachedEndOfPath;
    public Path path;

    //Player variables
    private GameObject player;
    private Player playerScript;

    //Enemy statistic variables
    public int healthPoints = 3;
    private int damage = 1;
    private float attackRange = 0.2f;
    private float attackSpeed = 1f;

    private bool isAttacking = false;


    private void Start()
    {
        fieldofview = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        fieldofview.tag = "Enemy";
        fieldofview.SetOrigin(transform.position);
        player = GameObject.Find("Player_Character");
        playerScript = player.GetComponent<Player>();
        seeker = GetComponent<Seeker>();
        fieldofview.SetFoV(fov);
        fieldofview.SetViewDistance(viewDistance);

    }

    private void Update()
    {
        fieldofview.SetOrigin(transform.position);


        if (wasPlayerSeen == false && fieldofview.IsLookingAtPlayer() == false && isTurningAround == false) Patrol();
        else FindPlayer();

        if (fieldofview.IsLookingAtPlayer() == true)
        {
            ChangeViewDistance(followingViewDistance);
        }
        else ChangeViewDistance(viewDistance);

        if (fieldofview.IsLookingAtPlayer() == true && attackRange >= Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position))
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                StartCoroutine(AttackPlayer(attackSpeed));
            }
        }
        else if (attackRange < Vector3.Distance(gameObject.transform.position, player.gameObject.transform.position))
        {
            isAttacking = false;
        }

    }

    private void FindPlayer()
    {
        if (fieldofview.IsLookingAtPlayer() == true)
        {
            lastKnowPlayerPosition = player.transform.position;
            MoveTowardsPoint(player.transform.position);
            wasPlayerSeen = true;

        }
        else if (wasPlayerSeen == true && fieldofview.IsLookingAtPlayer() == false)
        {
            MoveTowardsPoint(lastKnowPlayerPosition);
            if (reachedEndOfPath == true)
            {
                wasPlayerSeen = false;
                isTurningAround = true;
                StartCoroutine(LookAround(fieldofview, 0.01f, 360));
            }
        }
    }

    private void DealDamage()
    {
        playerScript.TakeDamage(damage);
    }

    public void TakeDamage(int value)
    {
        healthPoints -= value;
        if (healthPoints <= 0)
        {
            Destroy(fieldofview.gameObject);
            playerScript.EnemyKilled();
            Destroy(gameObject);
        }
    }


    public void OnPathComplete(Path p)
    {

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }

    }

    void MoveTowardsPoint(Vector3 pathPosition)
    {
        fieldofview.SetAimDirection(GetEnemyDirection());

        seeker.StartPath(transform.position, pathPosition, OnPathComplete);




        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (!reachedEndOfPath)
        {

            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 2 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;


                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (currentWaypoint != 0)
        {
            dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        }

        Vector3 velocity = dir * speed;
        transform.position += velocity * Time.deltaTime;



    }
    void MoveTowardsPoint(Vector3 pathPosition, int patrolWaypoint, out int patrolWaypoints2)
    {
        fieldofview.SetAimDirection(GetEnemyDirection());

        seeker.StartPath(transform.position, pathPosition, OnPathComplete);
        patrolWaypoints2 = patrolWaypoint;



        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (!reachedEndOfPath)
        {

            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;

                    Debug.Log(patrolWaypoints2 + " CO DO KURWY");

                    if (patrolWaypoints2 == 0)
                    {
                        patrolWaypoints2 = 1;
                    }
                    else if(patrolWaypoints2 == 1)
                    {
                        patrolWaypoints2 = 2;
                    }
                    else if (patrolWaypoints2 == 2)
                    {
                        patrolWaypoints2 = 0;
                    }
                    



                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (currentWaypoint != 0)
        {
            dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        }

        Vector3 velocity = dir * speed;
        transform.position += velocity * Time.deltaTime;



    }

    Vector3 GetEnemyDirection()
    {
        if (dir != null)
        {
            return dir;
        }
        else return Vector3.zero;

    }

    void ChangeViewDistance(float distance)
    {
        fieldofview.SetViewDistance(distance);
    }

    public IEnumerator LookAround(FieldOfView fieldOfView, float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            if (fieldofview.IsLookingAtPlayer() == true) yield break;
            fieldOfView.SetAimDirection(GetEnemyDirection(), i);
            if (invokeCount - 1 == i) isTurningAround = false;

            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator AttackPlayer(float attackSpeed)
    {
        while (isAttacking == true)
        {
            DealDamage();
            yield return new WaitForSeconds(attackSpeed);

        }

    }

    void Patrol()
    {
        MoveTowardsPoint(PatrolPoints[patrolWaypoint].position, patrolWaypoint, out patrolWaypoint);
    }



}