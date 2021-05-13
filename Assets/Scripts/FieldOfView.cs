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
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance, layerMask);
            RaycastHit2D raycastHit2DPlayer = Physics2D.Raycast(origin, VectorFromAngle(angle), viewDistance2, layerMask2);
            if (raycastHit2D.collider == null)
            {
                vertex = origin + VectorFromAngle(angle) * viewDistance;
                if (raycastHit2DPlayer.collider != null)
                {

                    if (raycastHit2DPlayer.transform.tag == "Player")
                    {
                        targetingPlayer = true;
                        Debug.Log("Patrze na gracza");
                    }
                    else targetingPlayer = false;
                }
                
               
            }
            else
            {

                vertex = raycastHit2D.point;

                if (raycastHit2DPlayer.collider != null)
                {


                    if (raycastHit2DPlayer.transform.tag == "Player")
                    {
                        distanceToObejctive = Vector3.Distance(origin, vertex);
                        distanceToPlayer = Vector3.Distance(origin, raycastHit2DPlayer.point);
                        if (distanceToObejctive > distanceToPlayer)
                        {
                            targetingPlayer = true;
                            Debug.Log("Patrze na gracza");
                        }
                        else targetingPlayer = false;

                    }
                    else targetingPlayer = false;
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
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
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

