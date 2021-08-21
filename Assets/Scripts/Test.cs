using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    NavMeshAgent nav;
    public Transform ball;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        StartCoroutine("move1");
    }

    IEnumerator move1()
    {
        print("코루틴1 호출");
        yield return new WaitForSeconds(1);
        print("코루틴1 1초 지남");
        yield return StartCoroutine("myAction");
        nav.SetDestination(ball.position);

    }
    IEnumerator myAction()
    {
        print("코루틴2 호출");
        yield return new WaitForSeconds(10);
        print("코루틴2 10초 지남");

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                nav.isStopped = false;
                ball.position = hit.point;
                nav.SetDestination(hit.point);
            }

        }
        //print(nav.remainingDistance);
        //if (nav.remainingDistance < 4f && nav.remainingDistance > 0)
        //{
        //    nav.isStopped = true;
        //}
    }
}
