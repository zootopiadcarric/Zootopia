using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.GroundFitter;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    public bool isMoving;
    public bool isSprinting;
    public FGroundFitter_Input finput;
    public bool sprint;
    public Vector3 moveVector;
    public float rotationOffset;
    NavMeshAgent agent;
    public float distStopFromFood = 0.25f;
    public float walkRadius;
    Vector3 randomDirection;
    public SceneAnimatorController SAC;
    public float agentSpeed=0.3f;
    public enum IdleList
    {
        Idle1
    }
    enum MyStatus
    {
        status_move,
        status_idle,
        status_eat
    }
    public bool FoodInRange= false;
    private MyStatus myStatus;
    public  float singleStep;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeed;
        StartCoroutine("AnimalState");
    }
    IEnumerator IdlePattern()
    {
        yield return null;
    }
    IEnumerator AnimalState()
    {
        while (true)
        {
            if(FoodInRange)
            {
                myStatus = MyStatus.status_eat;
            }
            else
            {
                myStatus = Random.value < 0.7f ? MyStatus.status_idle : MyStatus.status_move;
            }
            yield return StartCoroutine(myStatus.ToString());
        }
    }
    //bool CheckObstacle(Vector3 loca)
    //{
    //    Collider[] colls = Physics.OverlapSphere(loca, 0.1f, maskLayer);
    //    if (colls.Length > 0)
    //        //mat.color = Color.red;
    //        return true;
    //    else
    //        //mat.color = Color.white;
    //        return false;
    //}
    void UpdateGoal()
    {
        float x, y, r;
        Vector3 goal;
        
        do
        {
            NavMeshHit hit;
            randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            goal = hit.position;

            //} while (CheckObstacle(goal));
        } while (false);
        //this.goal.position = goal;
        agent.SetDestination(goal);
        Vector3.RotateTowards(transform.forward, goal, singleStep, 0.0f);

    }
    IEnumerator status_move()
    {
        isMoving = true;
        SAC.UpdateAnim(isMoving);
        yield return new WaitForSeconds(3f);

        agent.isStopped = false;
        UpdateGoal();

        yield return new WaitUntil(check);
        isMoving = false;
        SAC.UpdateAnim(isMoving, isSprinting);
    }
    IEnumerator status_idle()
    {
        agent.isStopped = true;
        isMoving = false;
        SAC.UpdateAnim(isMoving);
        yield return new WaitForSeconds(Random.Range(0, 3));
    }
    IEnumerator status_eat()
    {
        agent.isStopped = true;
        while (FoodInRange)
        {
            //anim.SetBool("Eat", true);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            //anim.SetBool("Eat", false);
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            yield return StartCoroutine(specialAct());
        }

    }
    protected virtual IEnumerator specialAct()
    {
        yield return null;
    }
    bool check()
    {
        float stopDist = FoodInRange ? distStopFromFood : 0.1f;
        return agent.remainingDistance < stopDist && agent.remainingDistance > 0f;
    }
    // Update is called once per frame
    void Update()
    {
        finput.SetControllerValue(sprint, moveVector, rotationOffset);
    }
}
