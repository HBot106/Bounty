using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardBehavior : MonoBehaviour
{
    public GameObject player_bounty_hunter;
    public GameObject audible_disturbance;
    public GameObject guard_eye;
    public GameObject guard_sword_hand;
    public GameObject[] guard_patrol_points;
    public NavMeshAgent guard_nav_agent;
    public LayerMask ignorLayerMask;
    public bool guard_near_detection_cone_active;
    public bool guard_far_detection_cone_active;
    public bool guard_heard_disturbance;
    public bool guard_can_see_player;
    public int guard_health;

    // guards state variables
    public int guard_state;
    private bool guard_is_investigating;
    public float guard_stopping_distance;
    private float guard_time_entered_guarding_state;
    public float guard_duration_of_stops;

    private int patrol_point_index;

    private Vector3 point_of_interest;


    // FIGHTING VARIABLES
    // 0 = not swinging, 1 = swinging right, 2 = swinging left, 
    // 3 = waiting to swing right, 4 = waiting to swing left
    public int swingState;
    public float swingSpeed;
    public float swingDelay;
    private float swingDelayCounter;
    private float swingStartAngle = 250f;
    private float swingEndAngle = 100f;



    void Start()
    {
        guard_state = 1;
        guard_health = 3;
        guard_is_investigating = false;
        guard_can_see_player = false;
        guard_time_entered_guarding_state = Time.time;
        point_of_interest = new Vector3(0f, 0f, 0f);


    }

    void Update()
    {
        if (guard_health <= 0)
        {
            toDeath();
        }

        if (guard_nav_agent.pathPending)
        {
            return;
        }

        checkLineOfSight();

        switch (guard_state)
        {
            // STATE_GUARDING
            case 0:
                // WORK
                // STATE TRANSITION
                if (visualDetectionCheck())
                {
                    break;
                }
                else if (audibleDetectionCheck())
                {
                    break;
                }
                else if (timeoutCheck())
                {
                    break;
                }
                break;

            // STATE_PATROLLING
            case 1:
                // WORK
                if (guard_is_investigating)
                {
                    guard_nav_agent.SetDestination(point_of_interest);
                }
                else
                {
                    guard_nav_agent.SetDestination(guard_patrol_points[patrol_point_index].transform.position);
                }
                // STATE TRANSITION
                if (visualDetectionCheck())
                {
                    break;
                }
                else if (audibleDetectionCheck())
                {
                    break;
                }
                else if (targetReachedCheck())
                // target reached
                {
                    break;
                }
                break;

            // STATE_CHASING
            case 2:
                // WORK
                guard_nav_agent.SetDestination(point_of_interest);
                // STATE TRANSITION
                if (visualDetectionCheck())
                {
                    break;
                }
                else if (targetReachedCheck())
                // target reached
                {
                    break;
                }
                else
                {
                    toGuarding();
                }
                break;

            // STATE_FIGHTING
            case 3:
                // WORK
                swingSword();
                guard_nav_agent.SetDestination(point_of_interest);
                // STATE TRANSITION
                if (visualDetectionCheck())
                {
                    break;
                }
                else
                {
                    toGuarding();
                }
                break;

            // STATE_DYING
            case 4:
                gameObject.transform.parent.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    private bool visualDetectionCheck()
    {
        if (guard_can_see_player)
        {
            if (guard_near_detection_cone_active)
            {
                toFighting(player_bounty_hunter.transform.position);
                return true;
            }
            else if (guard_far_detection_cone_active)
            {
                toChasing(player_bounty_hunter.transform.position);
                return true;
            }
            return false;

        }
        else
        {
            return false;
        }
    }

    private bool audibleDetectionCheck()
    {
        if (guard_heard_disturbance)
        {
            toPatrolling(true, audible_disturbance.transform.position);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool timeoutCheck()
    {
        if ((Time.time - guard_time_entered_guarding_state) >= guard_duration_of_stops)
        {
            toPatrolling(false, transform.position);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool targetReachedCheck()
    {
        
        if (Vector3.Distance(transform.position, guard_patrol_points[patrol_point_index].transform.position) <= guard_nav_agent.stoppingDistance)
        {
            toGuarding();
            
            return true;
        }
        else
        {
            return false;
        }
    }


    public void toPatrolling(bool need_to_investigate, Vector3 position_to_investigate)
    {
        guard_state = 1;
        point_of_interest = position_to_investigate;
        if (need_to_investigate)
        {
            guard_is_investigating = true;
            return;
        }
        else
        {
            guard_is_investigating = false;
            patrol_point_index = ((patrol_point_index + 1) % guard_patrol_points.Length);
            return;
        }
    }

    public void toChasing(Vector3 position_to_investigate)
    {
        guard_state = 2;
        point_of_interest = position_to_investigate;
        guard_is_investigating = true;
    }

    public void toGuarding()
    {
        guard_state = 0;
        guard_time_entered_guarding_state = Time.time;
        guard_is_investigating = false;
    }

    public void toFighting(Vector3 position_to_investigate)
    {
        guard_state = 3;
        point_of_interest = position_to_investigate;
        guard_is_investigating = false;
    }

    public void toDeath()
    {
        guard_state = 4;
        guard_is_investigating = false;
    }
    

    public void setGuardActive()
    {
        return;
    }

    public void setPointOfInterest(Vector3 POI)
    {
        return;
    }

    private void checkLineOfSight()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player_bounty_hunter layer
        if (Physics.Raycast(guard_eye.transform.position, Vector3.Normalize(player_bounty_hunter.transform.position - guard_eye.transform.position), out hit, Mathf.Infinity, ignorLayerMask))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(guard_eye.transform.position, Vector3.Normalize(player_bounty_hunter.transform.position - guard_eye.transform.position) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
                guard_can_see_player = true;
            }
            else
            {
                Debug.DrawRay(guard_eye.transform.position, Vector3.Normalize(player_bounty_hunter.transform.position - guard_eye.transform.position) * 1000, Color.white);
                //Debug.Log("Did not Hit");
                guard_can_see_player = false;
                // isPatroling = true;
            }
        }
    }

    private void swingSword()
    {
        if (swingState == 1)
        {
            guard_sword_hand.transform.Rotate(Vector3.up, -swingSpeed);
            if (guard_sword_hand.transform.localEulerAngles.y >= swingEndAngle &&
                guard_sword_hand.transform.localEulerAngles.y <= swingStartAngle)
            {
                swingState = 4;
            }
        }
        else if (swingState == 2)
        {
            guard_sword_hand.transform.Rotate(Vector3.up, swingSpeed);
            if (guard_sword_hand.transform.localEulerAngles.y <= swingStartAngle &&
                guard_sword_hand.transform.localEulerAngles.y >= swingEndAngle)
            {
                swingState = 3;
            }
        }
        else if (swingState == 3 || swingState == 4)
        {
            swingDelayCounter++;
            if (swingDelayCounter >= swingDelay)
            {
                swingState -= 2;
                swingDelayCounter = 0;
            }
        }
        else
        {
            swingState = 1;
        } 
    }
}
