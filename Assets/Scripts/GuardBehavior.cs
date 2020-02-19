using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




public class GuardBehavior : MonoBehaviour
{

    // STATE MACROS
    private static int STATE_GUARDING = 0;
    private static int STATE_PATROLLING = 1;
    private static int STATE_CHASING = 2;
    private static int STATE_FIGHTING = 3;
    private static int STATE_DYING = 4;

    // guards state variables
    private int guard_state;
    private int guard_health;
    private bool guard_is_investigating;
    private bool guard_can_see_player;
    private float guard_stopping_distance;
    private float guard_time_entered_guarding_state;
    private float guard_duration_of_stops;
    public bool guard_near_detection_cone_active;
    public bool guard_far_detection_cone_active;
    public GameObject guard_eye;
    public GameObject guard_sword_hand;
    public NavMeshAgent guard_nav_agent;
    public GameObject[] guard_patrol_points;
    private int patrol_point_index;
    public int patrol_point_count;
    private Vector3 point_of_interest;
    public GameObject player_bounty_hunter;
    public GameObject audible_disturbance;





    // FIGHTING VARIABLES
    // 0 = not swinging, 1 = swinging right, 2 = swinging left, 
    // 3 = waiting to swing right, 4 = waiting to swing left
    public int swingState;
    public float swingSpeed;
    public float swingDelay;
    private float swingDelayCounter;
    private float swingStartAngle = 250f;
    private float swingEndAngle = 100f;
    private Rigidbody rgdbdy;
    public float playerHitForce;
    private float life = 2;

    void Start()
    {
        guard_state = STATE_PATROLLING;
        guard_health = 3;
        guard_is_investigating = false;
        guard_can_see_player = false;
        guard_stopping_distance = 2.0f;
        guard_duration_of_stops = 2.0f;
        guard_time_entered_guarding_state = Time.time;
        point_of_interest = new Vector3(0f, 0f, 0f);

        rgdbdy = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
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
            case STATE_GUARDING:
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

            case STATE_PATROLLING:
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

            case STATE_CHASING:
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
                break;

            case STATE_FIGHTING:
                // WORK
                swingSword();
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

            case STATE_DYING:
                SetActive(false);
                break;

            default:
                break;
        }
    }

    private bool visualDetectionCheck()
    {
        if (guard_can_see_player)
        {
            if (guard_far_detection_cone_active)
            {
                toChasing(player_bounty_hunter.transform.position);
            }
            else if (guard_near_detection_cone_active)
            {
                toFighting();
            }
            return true;
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
        if ((Time.time - guard_time_entered_guarding_state) > guard_duration_of_stops)
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
        if (guard_nav_agent.remainingDistance <= guard_nav_agent.guard_stopping_distance)
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
        point_of_interest = position_to_investigate;
        if (need_to_investigate)
        {
            guard_is_investigating = true;
            return;
        }
        else
        {
            guard_is_investigating = false;
            patrol_point_index = ((patrol_point_index + 1) % patrol_point_count);
            return;
        }
    }

    public void toChasing(Vector3 position_to_investigate)
    {
        point_of_interest = position_to_investigate;
        guard_is_investigating = true;
    }

    public void toGuarding()
    {
        guard_time_entered_guarding_state = Time.time;
        guard_is_investigating = false;
    }

    public void toFighting()
    {
        guard_is_investigating = false;
    }

    public void toDeath()
    {
        guard_is_investigating = false;
    }

    private void checkLineOfSight()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player_bounty_hunter layer
        if (Physics.Raycast(guard_eye.transform.position, Vector3.Normalize(player_bounty_hunter.transform.position - guard_eye.transform.position), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "player")
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerSword"))
        {
            if (isPatroling)
            {
                Debug.Log("Guard Assassinated!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Guard Hit!");

                Vector3 hitDirection = (transform.position - other.transform.root.transform.position).normalized + Vector3.up;
                rgdbdy.AddForce(hitDirection * playerHitForce, ForceMode.Impulse);
                life--;
                if (life == 0)
                {
                    Debug.Log("Guard Killed!");
                    Destroy(gameObject);
                }
            }
        }
    }
}
