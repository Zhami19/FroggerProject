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

    public void TurtleDive()
    {
        anim.SetTrigger("Dive");
    }

    public void TurtleRise()
    {
        anim.SetTrigger("Rise");
    }

    public void ColliderDisable()
    {
        turtleDiveScript.killCollider.enabled = true;
    }

    public void ColliderEnable()
    {
        turtleDiveScript.killCollider.enabled = false;
    }
}
