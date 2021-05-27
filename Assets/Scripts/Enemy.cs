using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform prefabFieldOfView;
    FieldOfView fieldofview;

    [SerializeField] private float lookingAngle = 0;
    [SerializeField] private float fov = 90;
    [SerializeField] private float viewDistance = 2f;
    [SerializeField] private float followingViewDistance = 5f;
    [SerializeField] private float speed = 0.05f;

    private GameObject player;
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

    private void Start()
    {
        fieldofview = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        fieldofview.tag = "Enemy";
        fieldofview.SetOrigin(transform.position);
        player = GameObject.Find("Player_Character");
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
    }

     private void FindPlayer()
     {
         if(fieldofview.IsLookingAtPlayer()==true)
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
                 Debug.Log("Patrze naokoło");
             }
           }
     }

  


    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);

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
                    Debug.Log(reachedEndOfPath);
                    
                    
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if(currentWaypoint!= 0)
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
                if (currentWaypoint + 2 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    Debug.Log(reachedEndOfPath);
                    if (patrolWaypoints2 < 2)
                    {
                        patrolWaypoints2++;
                    }
                    else patrolWaypoints2 = 0;
                    


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
            fieldOfView.SetAimDirection(GetEnemyDirection(),i);
            if (invokeCount - 1 == i) isTurningAround = false;

            yield return new WaitForSeconds(interval);
        }
    }

    void Patrol()
    {
        MoveTowardsPoint(PatrolPoints[patrolWaypoint].position, patrolWaypoint, out patrolWaypoint);
    }
    
}
