using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{

    [SerializeField]
    private FieldOfView   fov;

    private float horizontal, vertical;
    float speed = 1f;

    private void Start()
    {
    }

    private void Update()
    {
        Movement();
        SetFoV();
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        fov.SetAimDirection(angle);

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        transform.Translate(new Vector2(horizontal, vertical) * speed * Time.deltaTime);
    }

    void SetFoV()
    {
        fov.SetOrigin(transform.position);
    }

    
    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
     Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
     Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    void Attack()
    {
        //attack animatiom
        Debug.Log("Im Attacking");
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

}
