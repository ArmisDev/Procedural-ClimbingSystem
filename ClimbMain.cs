using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS.Control.Climb;

[RequireComponent(typeof(ClimbLogic))]
public class ClimbMain : MonoBehaviour
{
    //Climb Interaction
    [Header("Climb Components")]
    [SerializeField] private Transform r_climbFirePoint;
    [SerializeField] private float f_climbFirePointLength;
    [SerializeField] private LayerMask exclusionLayerMask;
    private RaycastHit r_climbFirePoint_hit;
    private ProjectileCalculator projectileCalculator;

    //Components
    private ClimbLogic climbLogic;
    private FirstPersonController firstPersonController;
    private Transform target;

    //Climb Function
    [Header("Climb Thresholds")]
    [SerializeField] private float climbMax;
    [SerializeField] private float climbMin;
    private float topDistance;
    private bool intialFPS_Char_Jump_bool;
    public float climbSpeed;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        projectileCalculator = GetComponent<ProjectileCalculator>();
        climbLogic = GetComponent<ClimbLogic>();
        firstPersonController = GetComponent<FirstPersonController>();
        intialFPS_Char_Jump_bool = firstPersonController.enableJump;
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast
        Physics.Raycast(r_climbFirePoint.position, r_climbFirePoint.forward, out r_climbFirePoint_hit, f_climbFirePointLength, ~exclusionLayerMask);

        if (r_climbFirePoint_hit.collider != null)
        {
            climbLogic.HeightCheck(r_climbFirePoint_hit);
        }
        
        DebugStuff();
        MoveToTop();
    }

    void MoveToTop()
    {
        Vector3 climbVector = new Vector3(0, climbLogic.climbDistance, 1f);
        targetPosition = gameObject.transform.position + climbVector;

        if (Input.GetKeyDown(KeyCode.Space) && CanClimb() && firstPersonController.isGrounded)
        {
            firstPersonController.enableJump = false;
            StartCoroutine(SmoothMove(targetPosition, climbSpeed));
        }
        else
        {
            firstPersonController.enableJump = intialFPS_Char_Jump_bool;
        }
    }

    IEnumerator SmoothMove(Vector3 targetPosition, float duration)
    {
        Vector3 initialPosition = gameObject.transform.position;
        float time = 0;

        while (time < duration)
        {
            firstPersonController.playerCanMove = false;
            time += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(initialPosition, climbLogic.cubePosition, time / duration);
            yield return null;
        }

        gameObject.transform.position =  climbLogic.cubePosition; // Ensure final position is set correctly
        firstPersonController.playerCanMove = true;
    }

    bool CanClimb()
    {
        return climbLogic.climbDistance >= climbMin && climbLogic.climbDistance <= climbMax && r_climbFirePoint_hit.collider != null;
    }

    void DebugStuff()
    {
        Debug.DrawRay(r_climbFirePoint.position, r_climbFirePoint.forward * f_climbFirePointLength, Color.green);
    }
}
