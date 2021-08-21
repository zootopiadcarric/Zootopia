using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SceneAnimatorController : MonoBehaviour
{
    Animator animator;
    public float m_fSpeed=3f;
    public string str;
    public enum AnimState
    {
        isWalking,
        isRunning,
        isBarking,
        isSitting,
        Idle
    }
    public AnimState animState; 
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public string UpdateAnim(bool isMoving, bool isSprinting = false)
    {
        animState = AnimState.Idle;

        if (isMoving)
        {
            animState = AnimState.isWalking;
            if (isSprinting) animState = AnimState.isRunning;
        }
        str = animState.ToString();
        return str;
        //SetAnimatorString(animState.ToString());
    }
    public void SetAnimatorString(string value)
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].name != value)
            {
                animator.SetBool(animator.parameters[i].name, false);
                print(value);
            }
        }

        if(value != "Idle")
            animator.SetBool(value, true);

    }
}
