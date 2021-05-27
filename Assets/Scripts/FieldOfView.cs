using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    LayerMask layerMask2;
    private Mesh mesh;
    Vector3 origin = Vector3.zero;
    private float startingAngle;
    private float fov;
    private float viewDistance;
    private float viewDistance2 = 5;
    int rayCount = 50;
    private bool sawPlayer, seeingPlayer;

    private float distanceToPlayer, distanceToObejctive;

    private bool targetingPlayer = false;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 45f;
        viewDistance = 1f;
    }
    private void LateUpdate()
    {
        CheckFieldOfView();
        
        
    }

  void CheckFieldOfView()
    {
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;


        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D;

            if (this.gameObject.tag == "Enemy")
            {
                raycastHit2D = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance, LayerMask.GetMask("Mask", "Objects"));
            }
            else
            {
                raycastHit2D = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance, LayerMask.GetMask("Objects"));
            }

            RaycastHit2D raycastHit2DPlayer = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance + 1, LayerMask.GetMask("Mask"));
            if (raycastHit2D.collider == null)
            {
                vertex = origin + VectorFromAngle(angle) * viewDistance;

                if (raycastHit2DPlayer.collider == null && sawPlayer == false)
                {
                    seeingPlayer = false;

                }

                if (raycastHit2DPlayer.collider != null && sawPlayer == true)
                {

                    seeingPlayer = true;

                }


            }
            else
            {

                vertex = raycastHit2D.point;
                if (raycastHit2D.transform.tag == "Player")
                {
                    sawPlayer = true;
                    seeingPlayer = true;
                }

            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
            if (i == rayCount)
            {
                sawPlayer = false;
            }

            
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
        targetingPlayer = seeingPlayer;
        sawPlayer = false;
        seeingPlayer = false;

    }


    Vector3 VectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    float AngleFromVector(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(float angle)
    {
        startingAngle = angle + fov / 2f;
    }

    public void SetAimDirection(Vector3 angle)
    {
        startingAngle = AngleFromVector(angle)+ fov/2f;
    }

    public void SetAimDirection(Vector3 vecAngle, float angle)
    {
        startingAngle = AngleFromVector(vecAngle) + angle + fov / 2f;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float ViewDistance)
    {
        this.viewDistance = ViewDistance;
    }

    public bool IsLookingAtPlayer()
    {
        return targetingPlayer;
    }
}

