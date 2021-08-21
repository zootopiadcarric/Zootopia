using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using FIMSpace.GroundFitter;
using UnityEngine.AI;

public class AnimalAI2 : Agent
{
    NavMeshAgent agent;

    public bool isMoving;
    public bool isSprinting;
    public FGroundFitter_Input finput;
    public bool sprint;
    public Vector3 moveVector;
    public float rotationOffset;
    public float distStopFromFood = 0.25f;
    public float walkRadius;
    Vector3 randomDirection;
    public SceneAnimatorController SAC;
    public float agentSpeed = 0.3f;
    Vector3 goal;
    public bool ignore;
    enum MyStatus
    {
        status_move,
        status_idle,
        status_eat
    }
    public bool FoodInRange = false;
    private MyStatus myStatus;
    public float singleStep;


    private Transform tr;
    private Rigidbody rb;
    public float moveSpeed = 1.5f;
    public float turnSpeed = 200.0f;
    private StageManagerIL stageManager;
    public Material goodMt, badMt, originMt;
    private Renderer floorRd;
    public string str;
    float rot = 0f;
    private float delayTime;
    private bool ignoreSprint;
    private int moveState;

    // Start is called before the first frame update



    public override void Initialize()
    {
        MaxStep = 10000000;
        tr = GetComponent<Transform>();
        //rb = GetComponent<Rigidbody>();
        //floorRd = tr.parent.Find("Floor").GetComponent<Renderer>();
        //originMt = floorRd.material;
        stageManager = tr.parent.GetComponent<StageManagerIL>();
        agent = GetComponent<NavMeshAgent>();

    }
    public override void OnEpisodeBegin()
    {
        //stageManager.InitStage();
        tr.localPosition = new Vector3(12.57f, 0f, 0.52f);
        tr.localRotation = Quaternion.identity;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (ignore) return;
        var action = actions.DiscreteActions;
        //Debug.Log($"[0]={action[0]},[1]={action[1]}");
        Vector3 dir = Vector3.zero;
        //Branch[0] ����/����/���� (0,1,2)

        switch (action[0])
        {
            case 0: dir = Vector3.zero;
                    isMoving = false;
                break;
            case 1: dir = Vector3.forward;
                    isMoving = true;

                break;
                //case 2: dir = -Vector3.forward; break;

           
        }
        switch (action[1])
        {
            case 1: rot += -10f;
                isMoving = true;
                break;
            case 2: rot += 10f;
                isMoving = true;
                break;

        }
        switch (action[2])
        {
            case 1:
                if (ignoreSprint) { sprint = false ; break; }
                sprint = true;
                break;
            case 2:
                sprint = false;
                break;

        }
        str = SAC.UpdateAnim(isMoving, sprint);
        SAC.SetAnimatorString(str);
        finput.SetControllerValue(sprint, dir, rot);
        SetReward(-1 / (float)MaxStep);


    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions.Clear();
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;

        }
        //if (Input.GetKey(KeyCode.S))
        //{
        //    actions[0] = 2;

        //}
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1;

        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2;

        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            actions[2] = 1;
        }
        else
            actions[2] = 2;
    }
    private void Start()
    {
        StartCoroutine(AnimalState());   
    }
    void UpdateGoal()
    {
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
    }
    IEnumerator status_move()
    {
        UpdateGoal();
        moveState = Random.Range(0, 2);
        switch (moveState)
        {
            case 0:
                ignoreSprint = true;
                break;
            case 1:
                ignoreSprint = false;
                break;

        }
        yield return new WaitUntil(check);
    }
    IEnumerator status_eat()
    {
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
        AddReward(-0.01f);
        //float stopDist = FoodInRange ? distStopFromFood : 0.1f;
        if (Vector3.Distance(goal, transform.position) < 0.1f)
        {
            SetReward(1.0f);
            EndEpisode();
            return true;
        }
        else
            return false;
        //return agent.remainingDistance < stopDist && agent.remainingDistance > 0f;
    }
    IEnumerator status_idle()
    {
        ignore = true;
        delayTime = 0f;
        str = SAC.UpdateAnim(false);
        SAC.SetAnimatorString(str);
        finput.SetControllerValue(false, new Vector3(0, 0, 0), 0f);
        yield return new WaitUntil(checkIdle);
        ignore = false;
    }
    bool checkIdle()
    {
        delayTime += Time.deltaTime;
        //str = SAC.UpdateAnim(false);
        //SAC.SetAnimatorString(str);
        //finput.SetControllerValue(false, new Vector3(0, 0, 0), 0f) ;
        if (delayTime > 4f)
            return true;
        else
            return false;
        //return agent.remainingDistance < stopDist && agent.remainingDistance > 0f;
    }
    IEnumerator AnimalState()
    {
        while (true)
        {
            while (true)
            {
                if (FoodInRange)
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
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == stageManager.hintColor.ToString())
    //    {
    //        SetReward(1.0f);
    //        EndEpisode();
    //        StartCoroutine(RevertMaterial(goodMt));
    //    }
    //    else
    //    {
    //        if (collision.collider.CompareTag("WALL") || collision.gameObject.name == "Hint")
    //        {
    //            SetReward(-0.01f);
    
    //        }
    //        else
    //        {
    //            SetReward(-1.0f);
    //            EndEpisode();
    //            StartCoroutine(RevertMaterial(badMt));

    //        }
    //    }
    //}
    IEnumerator RevertMaterial(Material changedMt)
    {
        floorRd.material = changedMt;
        yield return new WaitForSeconds(0.2f);
        floorRd.material = originMt;
    }
    
}
