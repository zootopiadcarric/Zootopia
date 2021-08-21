using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerIL : MonoBehaviour
{

    public enum HINT_COLOR
    {
        BLACK, RED, GREEN, BLUE
    }
    public HINT_COLOR hintColor = HINT_COLOR.BLACK;
    public Material[] hintMt;
    public string[] hintTag;
    private new Renderer renderer;
    private int prevColorIdx = -1;
    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.Find("Hint").GetComponent<MeshRenderer>();

    }
    public void InitStage()
    {
        int idx = 0;
        do
        {
            idx = Random.Range(0, hintMt.Length);
        } while (idx == prevColorIdx);
        prevColorIdx = idx;
        renderer.material = hintMt[idx];
        renderer.gameObject.tag = hintTag[idx];
        hintColor = (HINT_COLOR)idx;
    }
 

    // Update is called once per frame
    void Update()
    {
        
    }
}
