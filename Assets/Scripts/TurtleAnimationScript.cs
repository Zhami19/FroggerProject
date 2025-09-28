using UnityEngine;

public class TurtleAnimationScript : MonoBehaviour
{
    Animator anim;
    TurtleDiveScript turtleDiveScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        turtleDiveScript = GetComponentInParent<TurtleDiveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
