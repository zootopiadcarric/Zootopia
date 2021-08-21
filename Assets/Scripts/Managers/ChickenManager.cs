using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MoveByNav
{
    protected override IEnumerator specialAct()
    {
        anim.SetBool("Turn Head", true);
        yield return new WaitForSeconds(3.5f);
        anim.SetBool("Turn Head", false);
    }
}
