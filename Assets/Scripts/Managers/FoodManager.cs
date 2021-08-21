using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FoodManager : MonoBehaviour
{
    public event Action AteAll;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Kill", 10f);
        
    }
    void Kill()
    {
        AteAll?.Invoke();
        Destroy(gameObject);
    }
}
