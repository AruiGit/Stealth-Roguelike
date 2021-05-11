using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        fieldofview = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        fieldofview.SetOrigin(transform.position);
        player = GameObject.Find("Player_Character");
       
    }

    private void Update()
    {
        fieldofview.SetAimDirection(lookingAngle);
        fieldofview.SetFoV(fov);
        fieldofview.SetViewDistance(viewDistance);
        fieldofview.SetOrigin(transform.position);

        FindPlayer();
    }


    private void AttackPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void FindPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        {
            lastKnowPlayerPosition = player.transform.position;
            AttackPlayer();
        } 
    }
}
