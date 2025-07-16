using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] Sprite Stand;
    [SerializeField] Sprite Walk1;
    [SerializeField] Sprite Walk2;
    [SerializeField] Sprite Step;
    [SerializeField] Sprite Jump;
    [SerializeField] Sprite Up;
    [SerializeField] Sprite Air;
    [SerializeField] Sprite Down;
    [SerializeField] Sprite Land;
    [SerializeField] Sprite ChargeStand1;
    [SerializeField] Sprite ChargeWalk1;
    [SerializeField] Sprite ChargeJump1;
    [SerializeField] Sprite ChargeStand2;
    [SerializeField] Sprite ChargeWalk2;
    [SerializeField] Sprite ChargeJump2;
    [SerializeField] Sprite ChargeStand3;
    [SerializeField] Sprite ChargeWalk3;
    [SerializeField] Sprite ChargeJump3;
    [SerializeField] Sprite ShootStand;
    [SerializeField] Sprite ShootWalk;
    [SerializeField] GameObject Arrow;
    [SerializeField] AudioSource BowAudioSource;
    [SerializeField] AudioClip drawSound;
    [SerializeField] AudioClip shootSound;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isMoving = false;
    private bool isCharging = false;
    public bool isShooting = false;
    private bool lookRight = true;
    private bool lookLeft = false;
    private bool isGround = true;
    private bool up = false;
    private bool down = false;
    public bool atode;
    public float speed = 3f;
    [SerializeField] float walkSpan = 0.8f;
    [SerializeField] float chargeSpan = 0.6f;
    public float jumpPower;
    private float moveTimer = 0f;
    private float downTimer = 0f;
    private float upTimer = 0f;
    public float chargeTimer = 0f;
    public float shootTimer = 0f;
    public float span = 0.15f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        BowAudioSource = GetComponent<AudioSource>();
        BowAudioSource.clip = drawSound;
        BowAudioSource.loop = false;
        BowAudioSource.volume = 0.3f;
    }
    void Update()
    {
        //向きの判定
        if (!isCharging && !isShooting)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                lookRight = true;
                lookLeft = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lookRight = false;
                lookLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    lookRight = false;
                    lookLeft = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    lookRight = true;
                    lookLeft = false;
                }
            }
        }
        if (lookRight) transform.localScale = new Vector3(1f, 1f, 1f);
        if (lookLeft) transform.localScale = new Vector3(-1f, 1f, 1f);
        //向きの判定


        //弓
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BowAudioSource.pitch = Random.Range(1.8f, 2.2f);
            BowAudioSource.Play();
            Debug.Log("Play called");

        }
        if (Input.GetKey(KeyCode.Z) && !isShooting)
        {
            isCharging = true;
            chargeTimer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isCharging = false;
            BowAudioSource.Stop();
            Debug.Log("Stop called");
            if (chargeTimer >= chargeSpan)
            {
                isShooting = true;
                BowAudioSource.pitch = 1f;
                BowAudioSource.PlayOneShot(shootSound);
                if (lookRight)
                {
                    Instantiate(Arrow, transform.position + Vector3.right * transform.localScale.x * 0.35f, Quaternion.identity);
                }
                else
                {
                    Instantiate(Arrow, transform.position + Vector3.left * transform.localScale.x * -0.35f, Quaternion.Euler(0f, 0f, -180f));
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    lookRight = true;
                    lookLeft = false;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    lookRight = false;
                    lookLeft = true;
                }
            }
            chargeTimer = 0f;
        }
        if (isShooting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= 0.2f)
            {
                isShooting = false;
                shootTimer = 0f;
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    lookRight = true;
                    lookLeft = false;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    lookRight = false;
                    lookLeft = true;
                }
            }
        }
        //弓


        //移動
        rb.linearVelocityX = 0f;
        if (Input.GetKey(KeyCode.RightArrow) && (lookRight || isCharging || isShooting))
        {
            isMoving = true;
            rb.linearVelocityX = speed;
            if (!isCharging && !isShooting) rb.linearVelocityX *= 3f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && (lookLeft || isCharging || isShooting))
        {
            isMoving = true;
            rb.linearVelocityX = -speed;
            if (!isCharging && !isShooting) rb.linearVelocityX *= 3f;
        }
        if (Input.GetKey(KeyCode.LeftShift) && !isCharging && !isShooting)
        {
            moveTimer += Time.deltaTime * 0.5f;
            rb.linearVelocityX *= 1.5f;
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            isMoving = false;
        }
        if (moveTimer >= walkSpan) moveTimer = 0f;
        if (isMoving) moveTimer += Time.deltaTime;
        else moveTimer = 0f;
        //移動


        //地面判定
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.right * transform.localScale.x * 0.3f + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + Vector3.left * transform.localScale.x * 0.3f + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit1.collider != null || hit2.collider != null || hit3.collider != null && rb.linearVelocity.y == 0f)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        //地面判定


        //上昇、下降判定
        if (downTimer >= span) downTimer = 0f;
        if (rb.linearVelocity.y < -3f)
        {
            downTimer += Time.deltaTime;
            down = true;
        }
        else
        {
            downTimer = 0f;
            down = false;
        }
        if (rb.linearVelocity.y > 1f)
        {
            upTimer += Time.deltaTime;
            up = true;
        }
        else
        {
            upTimer = 0f;
            up = false;
        }
        //上昇、下降判定


        //ジャンプ
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGround = false;
        }
        if (!Input.GetKey(KeyCode.UpArrow) && up) rb.linearVelocityY -= jumpPower * Time.deltaTime;
        //ジャンプ


        //スプライト関連
        sr.sprite = Stand;
        if (isCharging)
        {
            if (chargeTimer <= chargeSpan * 0.5f)
            {
                sr.sprite = ChargeStand1;
            }
            else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
            {
                sr.sprite = ChargeStand2;
            }
            else
            {
                sr.sprite = ChargeStand3;
            }
        }
        if (isShooting)
        {
            sr.sprite = ShootStand;
        }
        if (isMoving && isGround)
        {
            if ((walkSpan * 0.25f < moveTimer && moveTimer <= walkSpan * 0.5f) ||
                (walkSpan * 0.75f < moveTimer && moveTimer <= walkSpan))
            {
                sr.sprite = Stand;
                if (isCharging)
                {
                    if (chargeTimer <= chargeSpan * 0.5f)
                    {
                        sr.sprite = ChargeStand1;
                    }
                    else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                    {
                        sr.sprite = ChargeStand2;
                    }
                    else
                    {
                        sr.sprite = ChargeStand3;
                    }
                }
                if (isShooting)
                {
                    sr.sprite = ShootStand;
                }
            }
                else if (0f < moveTimer && moveTimer <= walkSpan * 0.25f)
                {
                    sr.sprite = Walk1;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = ChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = ChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = ChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = ShootWalk;
                    }
                }
                else
                {
                    sr.sprite = Walk2;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = ChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = ChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = ChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = ShootWalk;
                    }
                }
            }

        if (!isGround)
        {
            if (!isShooting)
            {
                if (up)
                {
                    if (upTimer <= 0.1f)
                    {
                        sr.sprite = Jump;
                    }
                    else
                    {
                        sr.sprite = Up;
                    }
                }
                else if (down)
                {
                    sr.sprite = Down;
                }
                else
                {
                    sr.sprite = Air;
                }
            }
            if (isCharging)
                {
                    if (chargeTimer <= chargeSpan * 0.5f)
                    {
                        sr.sprite = ChargeJump1;
                    }
                    else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                    {
                        sr.sprite = ChargeJump2;
                    }
                    else
                    {
                        sr.sprite = ChargeJump3;
                    }
                }
        }
    }
}
