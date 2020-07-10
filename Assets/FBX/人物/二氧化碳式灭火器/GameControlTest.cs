using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlTest : MonoBehaviour
{
    public GameObject M_Camera;
    public GameObject P_Head;
    public GameObject p_head;
    public GameObject Chara;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = Chara.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        M_Camera.transform.position = P_Head.transform.position;
        //M_Camera.transform.rotation = P_Head.transform.rotation;
        if (Input.GetKeyDown(KeyCode.Q))
            animator.SetInteger("state", 0);
        if (Input.GetKeyDown(KeyCode.W))
            animator.SetInteger("state", 1);
        if (Input.GetKeyDown(KeyCode.E))
            animator.SetInteger("state", 2);
    }
    public void AnimationTest()
    {
            Chara.GetComponent<Animation>().Play("1");
    }
}
