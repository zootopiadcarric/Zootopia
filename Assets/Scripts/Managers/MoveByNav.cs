using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveByNav : MonoBehaviour
{
    enum MyStatus
    {
        status_move,
        status_idle,
        status_eat
    }
    //public Transform goal;
    public float distStopFromFood = 0.25f;
    public float speed_walk = 0.5f;
    public float speed_run = 1f;
    
    protected Animator anim;
    
    int maskLayer;
    Material mat;
    bool FoodInRange = false;
    NavMeshAgent agent;
    MyStatus myStatus;

    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponentInChildren<Animator>();
        myStatus = MyStatus.status_idle;
        maskLayer = LayerMask.GetMask("Obstacle");
        //mat = goal.GetComponent<Renderer>().material;
        agent = GetComponent<NavMeshAgent>();
        
        StartCoroutine("GoAction");
        
    }

    bool check()
    {
        float stopDist = FoodInRange ? distStopFromFood : 0.1f;
        return agent.remainingDistance < stopDist && agent.remainingDistance > 0f;
    }

    IEnumerator status_move()
    {
        anim.SetBool("Walk", true);
        agent.speed = speed_walk;
        UpdateGoal();
        
        yield return new WaitUntil(check);
    }
    IEnumerator status_idle()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(0,3));
    }
    IEnumerator status_eat()
    {
        agent.isStopped = true;
        while (FoodInRange)
        {
            anim.SetBool("Eat", true);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            anim.SetBool("Eat", false);
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            yield return StartCoroutine(specialAct());
        }
        
    }
    protected virtual IEnumerator specialAct()
    {
        yield return null;
    }
    void AteItAll()
    {
        FoodInRange = false;
    }
    IEnumerator GoAction()
    {
        while (true)
        {
            //yield return new WaitUntil(move);
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
            anim.SetBool("Eat", false);
            
            agent.isStopped = false;

            if (FoodInRange)
            {
                myStatus = MyStatus.status_eat;
            }
            else
            {
                myStatus = Random.value < 0.5f ? MyStatus.status_idle : MyStatus.status_move;
            }

            yield return StartCoroutine(myStatus.ToString());
            
        }
    }
    bool CheckObstacle(Vector3 loca)
    {
        Collider[] colls = Physics.OverlapSphere(loca, 0.1f, maskLayer);
        if (colls.Length > 0)
            //mat.color = Color.red;
            return true;
        else
            //mat.color = Color.white;
            return false;
    }
    void UpdateGoal()
    {
        float x, y, r;
        Vector3 goal;
        do
        {
            x = Random.Range(-1.0f, 1.0f);
            y = Random.Range(-1.0f, 1.0f);
            r = Random.Range(1.0f, 3.0f);
            
            goal = new Vector3(x, 0, y);
            goal = goal.normalized * r;

        } while (CheckObstacle(goal));
        //this.goal.position = goal;
        agent.SetDestination(goal);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodInRange = true;
            //goal.position = other.transform.position;
            agent.speed = speed_run;
            anim.SetBool("Run", true);
            agent.SetDestination(other.transform.position);

            other.GetComponent<FoodManager>().AteAll += AteItAll;
        }
    }

}
