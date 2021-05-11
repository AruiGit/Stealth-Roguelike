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

    private void Start()
    {
        fieldofview = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        fieldofview.SetOrigin(transform.position);
        
       
    }

    private void Update()
    {
        fieldofview.SetAimDirection(lookingAngle);
        fieldofview.SetFoV(fov);
        fieldofview.SetViewDistance(viewDistance);
    }


    private void AttackPlayer()
    {


    }
}
