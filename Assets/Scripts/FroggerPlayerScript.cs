using System.Collections;
using UnityEngine;

public class FroggerPlayerScript : MonoBehaviour
{
    Frogger_InputActions _actions;

    AudioManager audioManager;
    GameManager gameManager;

    [SerializeField] public bool isDead;
    [SerializeField] private bool onRiver;
    [SerializeField] private bool onPlatform;

    [SerializeField] private bool canMove;
    Animator anim;
    ScoringZoneScript _zone;

    private void Awake()
    {
        _actions = new Frogger_InputActions();

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _zone = GameObject.FindGameObjectWithTag("ScoreZone").GetComponent<ScoringZoneScript>();

        anim = GetComponentInChildren<Animator>();
        canMove = true;
        isDead = false;
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) PlayerUpdate();
    }

    void PlayerUpdate()
    {
        if (_actions.Player.Up.triggered && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            PlayerMove(Vector3.up);
            StartCoroutine(MoveTime());
        }
        if (_actions.Player.Down.triggered && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            PlayerMove(Vector3.down);
            StartCoroutine(MoveTime());
        }
        if (_actions.Player.Left.triggered && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            PlayerMove(Vector3.left);
            StartCoroutine(MoveTime());
        }
        if (_actions.Player.Right.triggered && canMove)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 270f);
            PlayerMove(Vector3.right);
            StartCoroutine(MoveTime());
        }
    }

    void PlayerMove(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;

        Collider2D _barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Barrier"));
        Collider2D _platform = Physics2D.OverlapBox(destination, Vector2.zero, 0, LayerMask.GetMask("Platform"));
        

        if (_barrier != null) return;
        
;       if (_platform != null)
        {
            transform.SetParent(_platform.transform);
            onPlatform = true;
        }
        else
        {
            transform.SetParent(null);
            onPlatform = false;
        }

            StartCoroutine(SmoothMove(destination));
    }

    IEnumerator SmoothMove(Vector3 destination)
    {
        Vector3 startPos = transform.position;
        anim.SetTrigger("Hop");
        audioManager._audi.PlayOneShot(audioManager.soundFX[1]);

        float elapsed = 0f;
        float duration = .125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, destination, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle" && !isDead) StartCoroutine(PlayerDeath(3));
        if (other.gameObject.tag == "River")
        {
            onRiver = true;
        }
        if (other.gameObject.tag == "KillVolume")
        {
            if (onRiver) StartCoroutine(PlayerDeath(2));
            else StartCoroutine(PlayerDeath(3));
        }

        if (other.gameObject.tag == "Pad")
        {
            LilypadScript _pad = other.gameObject.GetComponent<LilypadScript>();
            if (!_pad._occupied)
            {
                _pad._occupied = true;
                _pad._spr.enabled = true;
                gameManager.padCount--;

                //Lilypad check
                if (gameManager.padCount > 0)
                {
                    audioManager._audi.PlayOneShot(audioManager.soundFX[0]);
                    gameManager.NewFrog();
                }
                else
                    gameManager.WinScreen();

                StartCoroutine(MoveTime());
                Destroy(gameObject);
            }
            else StartCoroutine(PlayerDeath(2));
        }

        if (other.gameObject.tag == "ScoreZone")
        {
            ScoringZoneScript _scoreZ = other.gameObject.GetComponent<ScoringZoneScript>();
            if (!_scoreZ.zoneTrigger)
            {
                gameManager.gameScore += _zone.zoneScore;
                _zone.zoneTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "River")
        {
            onRiver = false;
        }
    }

    IEnumerator MoveTime()
    {
        canMove = false;
        yield return new WaitForSeconds(.175f);
        canMove = true;
        if (onRiver)
        {
            if (!onPlatform) StartCoroutine(PlayerDeath(2));
        }
    }

    public void TimeOut()
    {
        StartCoroutine(PlayerDeath(3));
    }

    IEnumerator PlayerDeath(int sfx)
    {
        isDead = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        anim.SetTrigger("Death");
        audioManager._audi.PlayOneShot(audioManager.soundFX[sfx]);
        yield return new WaitForSeconds(1.5f);
        gameManager.playerLives--;
        if(gameManager.playerLives < 0)
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.NewFrog();
        }
        Destroy(gameObject);
    }
}
