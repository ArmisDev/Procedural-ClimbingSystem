using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Control.Climb
{
    public class ClimbLogic : MonoBehaviour
    {
        [Header("Cube For Space Check")]
        [SerializeField] private GameObject boxCollider;

        //Private / Non-Serializable Data
        private float objectHeight;
        private GameObject cloneCube;
        [HideInInspector] public float climbDistance;
        private GameObject currentGameObject;
        private bool hasSpawnedCube;

        // SpawnCheck
        private bool roomForPlayer;
        [HideInInspector] public Vector3 cubePosition;

        public void HeightCheck(RaycastHit hit)
        {
            objectHeight = hit.collider.bounds.size.y;
            float distanceToBottom = hit.point.y - hit.collider.bounds.min.y;
            climbDistance = objectHeight - distanceToBottom;

            if (hit.collider.gameObject == currentGameObject) return;
            else if (hit.collider.gameObject != currentGameObject)
            {
                SpawnCubeCheck(hit.collider.gameObject);
                hasSpawnedCube = false;
            }

            currentGameObject = hit.collider.gameObject;
        }

        public void SpawnCubeCheck(GameObject climbObject)
        {
            if (cloneCube == null)
            {
                Vector3 spaceCheckVector = new Vector3(climbObject.transform.position.x, objectHeight - 0.5f, climbObject.transform.position.z);
                GameObject clone = Instantiate(boxCollider, spaceCheckVector, climbObject.transform.rotation);
                hasSpawnedCube = true;
                cloneCube = clone;
                cubePosition = cloneCube.transform.position;
                Destroy(clone, 2f);
            }
            else return;
        }

        void CheckForPlayerSpace()
        {
            if (cloneCube != null)
            {
                cloneCube.TryGetComponent<ClimbColliderCheck>(out ClimbColliderCheck climbColliderCheck);
                if(climbColliderCheck != null)
                {
                    roomForPlayer = climbColliderCheck.roomForPlayer;
                }
            }
        }

        private void Update()
        {
            CheckForPlayerSpace();
        }
    }
}