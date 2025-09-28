using System.Collections;
using UnityEngine;

public class TurtleDiveScript : MonoBehaviour
{
    public TurtleAnimationScript[] _turtle;
    public Collider2D killCollider;
    [SerializeField] private bool isDiving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        killCollider.enabled = false;
        isDiving = false;
        InvokeRepeating("TurtleDive", 3, 10);
    }

    void TurtleDive()
    {
        StartCoroutine(DiveTime());
    }

    IEnumerator DiveTime()
    {
        if (!isDiving)
        {
            foreach(TurtleAnimationScript t in _turtle)
            {
                t.TurtleDive();
            }
            yield return new WaitForSeconds(.5f);

            foreach (TurtleAnimationScript t in _turtle)
            {
                t.TurtleRise();
            }
        }
    }
}
