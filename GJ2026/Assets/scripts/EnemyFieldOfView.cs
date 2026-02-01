/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class EnemyFieldOfView : MonoBehaviour
{
    // True if the enemy found the player.
    public bool HasSeenPlayer { get; private set; }

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    public float fov = 90f;
    public float viewDistance = 50f;
    public int rayCount = 50;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void LateUpdate() {
        float angle = 0; //startingAngle; // FIXME!
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        Vector3 origin = transform.position;
        vertices[0] = origin;

        HashSet<GameObject> hitObjects = new();

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            var vectorFromAngle = UtilsClass.GetVectorFromAngle(angle);
            bool hit = Physics.Raycast(
                origin,
                vectorFromAngle,
                out RaycastHit hitInfo,
                viewDistance,
                layerMask);

            if (!hit || hitInfo.collider == null) {
                // No hit
                vertex = origin + vectorFromAngle * viewDistance;
                Debug.DrawRay(origin, vectorFromAngle * viewDistance, Color.red);
            } else {
                // Hit object
                vertex = hitInfo.point;
                Debug.DrawRay(origin, vectorFromAngle * hitInfo.distance, Color.red);

                var hitObject = hitInfo.transform.gameObject;
                hitObjects.Add(hitObject);
            }
            vertices[vertexIndex] = vertex;

            if (i > 0) {
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

        foreach (var hitObject in hitObjects)
        {
            if (hitObject.tag == "Player")
            {
                Debug.Log("Player was spotted by an enemy!");
                HasSeenPlayer = true;
                break;
            }
        }
    }
}
