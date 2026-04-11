/*
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

// Modified thanks to https://discussions.unity.com/t/trouble-with-mesh-generation-for-a-field-of-view/910678/6

using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Components
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(AlertStateComponent))]
    public class FieldOfViewComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        public float fov = 90f;
        public float viewDistance = 50f;
        public int rayCount = 50;

        public Color colorWhenPatrolling = new(0f, 1f, 0f, 100f);
        public Color colorWhenSuspecting = new(0.9f, 0.9f, 0f, 100f);
        public Color colorWhenSpottedPlayer = new(1f, 0f, 0f, 100f);

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        public event Action<GameObject> OnActorDetection;

        private void Start()
        {
            // Always ignore objects that want to ignore raycasts.
            layerMask.value &= ~(1 << LayerMask.NameToLayer("Ignore Raycast"));

            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material.color = colorWhenPatrolling;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;

            meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter.mesh == null)
            {
                meshFilter.mesh = new Mesh();
            }

            var alertStateComponent = GetComponent<AlertStateComponent>();
            alertStateComponent.OnAlertStateChange += UpdateColorBasedOnAlertState;
        }

        private void Update()
        {
            MeshGenQuarternionCalcs();
        }

        private void UpdateColorBasedOnAlertState(AlertState newState)
        {
            Color newColor = Color.purple;
            switch (newState)
            {
                case AlertState.Unaware:
                    newColor = colorWhenPatrolling;
                    break;
                case AlertState.Suspecting:
                    newColor = colorWhenSuspecting;
                    break;
                case AlertState.SpottedPlayer:
                    newColor = colorWhenSpottedPlayer;
                    break;
            }

            meshRenderer.material.color = newColor;
        }

        private float AimDirectionStartingAngle
        {
            get
            {
                return UtilsClass.GetAngleFromVectorFloat(transform.rotation.eulerAngles)
                       - (fov / 2);
            }
        }

        // Credits to https://discussions.unity.com/t/trouble-with-mesh-generation-for-a-field-of-view/910678/6
        public void MeshGenQuarternionCalcs()
        {
            float meshAngle = AimDirectionStartingAngle + (fov / 2);
            float angleIncrease = fov / rayCount;

            Vector3[] vertices = new Vector3[rayCount + 1 + 1]; // positioning of points
            Vector2[] uv = new Vector2[vertices.Length]; // texture rendered - vector 2 as the image it references is flat 2d so it uses vector 2 only
            int[] triangles = new int[rayCount * 3]; // actual points of the mesh

            Vector3 rayOrigin = transform.position;
            vertices[0] = Vector3.zero; // same as above, mesh origin is at this transform's position

            HashSet<GameObject> hitObjects = new();

            int vertexIndex = 1; // 0 is the origin
            int triangleIndex = 0;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                if (Physics.Raycast(
                        rayOrigin,
                        GetWorldDirectionFromAngle(meshAngle),
                        out RaycastHit raycastHit,
                        viewDistance,
                        layerMask)
                   )
                {
                    // Hit!
                    Transform mainBodyTransform = transform;
                    vertex = mainBodyTransform.InverseTransformPoint(raycastHit.point);

                    hitObjects.Add(raycastHit.transform.gameObject);

                    // NOTE: Must enable Gizmos in editor view for this to appear!
                    Debug.DrawRay(
                        rayOrigin,
                        vertex,
                        Color.red
                    );
                }
                else
                {
                    // Miss!
                    vertex = GetLocalDirectionFromAngle(meshAngle) * viewDistance;

                    // NOTE: Must enable Gizmos in editor view for this to appear!
                    Debug.DrawRay(
                        rayOrigin,
                        GetWorldDirectionFromAngle(meshAngle) * viewDistance,
                        Color.green
                    );
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
                meshAngle -= angleIncrease; // goes counter clockwise if +, - for anti clockwise
            }

            var mesh = meshFilter.mesh;
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.bounds = new Bounds(rayOrigin, Vector3.one * 1000f);

            foreach (var hitObject in hitObjects)
            {
                if (hitObject.layer == LayerMask.NameToLayer("Actor"))
                {
                    OnActorDetection?.Invoke(hitObject);
                    break;
                }
            }
        }

        Vector3 GetLocalDirectionFromAngle(float angle)
        {
            Quaternion rotationQuat = transform.rotation * Quaternion.AngleAxis(angle, -Vector3.up);
            Vector3 rotationVec = rotationQuat * Vector3.forward;
            return rotationVec;
        }

        Vector3 GetWorldDirectionFromAngle(float angle)
        {
            Vector3 rotationVec = GetLocalDirectionFromAngle(angle);
            Vector3 vectorInWorldSpace = transform.TransformDirection(rotationVec);
            return vectorInWorldSpace;
        }
    }
}