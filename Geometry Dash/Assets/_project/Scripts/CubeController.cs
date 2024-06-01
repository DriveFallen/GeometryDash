using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{
    public UnityEvent OnJumping;
    public UnityEvent OnLanding;
    public UnityEvent OnUpheaval;
    public UnityEvent OnFinish;
    public UnityEvent OnDanger;

    [SerializeField] private float cubeSpeed = 10f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    [SerializeField] private ParticleSystem deadExplosion;

    private Rigidbody2D _cubeRB;
    
    private bool _isGround = false;
    private bool _isMovement = true;

    private float targetAngle = 0f;

    private void Awake()
    {
        Time.timeScale = 1;
        _cubeRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isMovement)
        {
            _cubeRB.velocity = new Vector2(cubeSpeed, _cubeRB.velocity.y);
            if (_isGround)
            {
                if (Input.GetKey(KeyCode.Space)/* && !_isJumping*/)
                {
                    _isGround = false;
                    _cubeRB.velocity = new Vector2(_cubeRB.velocity.x, jumpPower);

                    if (_cubeRB.gravityScale > 0)
                    {
                        targetAngle -= 90f;
                    }
                    else
                    {
                        targetAngle += 90f;
                    }

                    OnJumping.Invoke();
                }
            }
        }

        float rotation = Mathf.LerpAngle(_cubeRB.rotation, targetAngle, rotateSpeed * Time.fixedDeltaTime);
        _cubeRB.MoveRotation(rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGround = true;

        if (collision.contacts[0].normal.x < -0.05)
        {
            StartCoroutine(Danger());
        }

        if (collision.gameObject.CompareTag("Danger"))
        {
            StartCoroutine(Danger());
        }

        OnLanding.Invoke();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Double Jump"))
        {
            _isGround = true;
        }

        if (other.CompareTag("BlackHole"))
        {
            AudioSource music = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
            music.Stop();

            _isMovement = false;
            OnFinish.Invoke();

        }
        
        if (other.CompareTag("Finish"))
        {
            _cubeRB.velocity = Vector2.zero;
            _cubeRB.gravityScale = 0;

            StartCoroutine(SceneController.LoadNextScene());
        }

        if (other.CompareTag("Upheaval"))
        {
            Upheaval();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Double Jump"))
        {
            _isGround = false;
        }
    }

    private void Upheaval()
    {
        _cubeRB.gravityScale *= -1;
        jumpPower *= -1;

        OnUpheaval.Invoke();
    }

    private IEnumerator Danger()
    {
        AudioSource music = GameObject.Find("SoundSystem").GetComponent<AudioSource>();
        music.Stop();

        _cubeRB.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        OnDanger.Invoke();
        deadExplosion.Play();
        _isMovement = false;
        _cubeRB.velocity = Vector2.zero;
        _cubeRB.gravityScale = 0;
        yield return new WaitForSeconds(0.5f);
        SceneController.Restart();
    }
}
