using UnityEngine;

public class LilypadScript : MonoBehaviour
{
    public SpriteRenderer _spr;
    public bool _occupied;

    private void Start()
    {
        _spr = GetComponent<SpriteRenderer>();
        _spr.enabled = false;
        _occupied = false;
    }
}
