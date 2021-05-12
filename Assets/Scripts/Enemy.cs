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
    [SerializeField] private float speed = 0.05f;

    private GameObject player;
    private Vector3 lastKnowPlayerPosition;
    public Transform targetPosition;

    bool wasPlayerSeen = false;

    Seeker seeker;

    public float nextWaypointDistance = 0.5f;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;
    public Path path;

    private void Start()
    {
        fieldofview = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        fieldofview.SetOrigin(transform.position);
        player = GameObject.Find("Player_Character");
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        fieldofview.SetAimDirection(lookingAngle);
        fieldofview.SetFoV(fov);
        fieldofview.SetViewDistance(viewDistance);
        fieldofview.SetOrigin(transform.position);
        
        FindPlayer();
    }

    private void FindPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= viewDistance)
         {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.position, directionToPlayer) < fov)
            {
                lastKnowPlayerPosition = player.transform.position;
                MoveTowardsPlayer(player.transform.position);
                wasPlayerSeen = true;
            }
            
         }
          else if (wasPlayerSeen == true && Vector3.Distance(transform.position, player.transform.position) > viewDistance)
          {
              MoveTowardsPlayer(lastKnowPlayerPosition);
              //MoveToStartingPosition after end of path
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

    void MoveTowardsPlayer(Vector3 playerPosition)
    {

        seeker.StartPath(transform.position, playerPosition, OnPathComplete);



        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
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
                    break;
                }
            }
            else
            {
                break;
            }
        }

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed;
        transform.position += velocity * Time.deltaTime;



    }
}
