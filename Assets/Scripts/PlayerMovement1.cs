using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] Sprite rightStand;
    [SerializeField] Sprite rightWalk1;
    [SerializeField] Sprite rightWalk2;
    [SerializeField] Sprite rightJump;
    [SerializeField] Sprite rightUp;
    [SerializeField] Sprite rightDown1;
    [SerializeField] Sprite rightDown2;
    [SerializeField] Sprite rightDown3;
    [SerializeField] Sprite rightChargeStand1;
    [SerializeField] Sprite rightChargeWalk1;
    [SerializeField] Sprite rightChargeJump1;
    [SerializeField] Sprite rightChargeStand2;
    [SerializeField] Sprite rightChargeWalk2;
    [SerializeField] Sprite rightChargeJump2;
    [SerializeField] Sprite rightChargeStand3;
    [SerializeField] Sprite rightChargeWalk3;
    [SerializeField] Sprite rightChargeJump3;
    [SerializeField] Sprite rightShootStand;
    [SerializeField] Sprite rightShootWalk;
    [SerializeField] GameObject rightArrow;
    [SerializeField] Sprite leftStand;
    [SerializeField] Sprite leftWalk1;
    [SerializeField] Sprite leftWalk2;
    [SerializeField] Sprite leftJump;
    [SerializeField] Sprite leftUp;
    [SerializeField] Sprite leftDown1;
    [SerializeField] Sprite leftDown2;
    [SerializeField] Sprite leftDown3;
    [SerializeField] Sprite leftChargeStand1;
    [SerializeField] Sprite leftChargeWalk1;
    [SerializeField] Sprite leftChargeJump1;
    [SerializeField] Sprite leftChargeStand2;
    [SerializeField] Sprite leftChargeWalk2;
    [SerializeField] Sprite leftChargeJump2;
    [SerializeField] Sprite leftChargeStand3;
    [SerializeField] Sprite leftChargeWalk3;
    [SerializeField] Sprite leftChargeJump3;
    [SerializeField] Sprite leftShootStand;
    [SerializeField] Sprite leftShootWalk;
    [SerializeField] GameObject leftArrow;
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
        //向きの判定


        //弓
        if (Input.GetKey(KeyCode.Z) && !isShooting)
        {
            isCharging = true;
            chargeTimer += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isCharging = false;
            if (chargeTimer >= chargeSpan)
            {
                isShooting = true;
                if (lookRight)
                {
                    Instantiate(rightArrow, transform.position + Vector3.right * transform.localScale.x * 0.35f, Quaternion.identity);
                }
                else
                {
                    Instantiate(leftArrow, transform.position + Vector3.left * transform.localScale.x * 0.35f, Quaternion.identity);
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
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("GroundLayer"));
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + Vector3.right * transform.localScale.x * 0.3f + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("GroundLayer"));
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + Vector3.left * transform.localScale.x * 0.3f + Vector3.down * transform.localScale.y * 0.6f, Vector2.down, 0.1f, LayerMask.GetMask("GroundLayer"));
        if (hit1.collider != null || hit2.collider != null || hit3.collider != null && rb.linearVelocity.y == 0f)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        if (downTimer >= span) downTimer = 0f;
        if (rb.linearVelocity.y < -0.2f)
        {
            downTimer += Time.deltaTime;
            down = true;
        }
        else
        {
            downTimer = 0f;
            down = false;
        }

        //if (upTimer >= span) upTimer = 0f;
        if (rb.linearVelocity.y > 0.2f)
        {
            upTimer += Time.deltaTime;
            up = true;
        }
        else
        {
            upTimer = 0f;
            up = false;
        }
        //地面判定


        //ジャンプ
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGround = false;
        }
        if (!Input.GetKey(KeyCode.UpArrow) && up) rb.linearVelocityY -= jumpPower * Time.deltaTime;
        //ジャンプ


        //スプライト関連
        if (lookRight) sr.sprite = rightStand;
        else sr.sprite = leftStand;
        if (isCharging)
        {
            if (lookRight)
            {
                if (chargeTimer <= chargeSpan * 0.5f)
                {
                    sr.sprite = rightChargeStand1;
                }
                else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                {
                    sr.sprite = rightChargeStand2;
                }
                else
                {
                    sr.sprite = rightChargeStand3;
                }
            }
            if (lookLeft)
            {
                if (chargeTimer <= chargeSpan * 0.5f)
                {
                    sr.sprite = leftChargeStand1;
                }
                else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                {
                    sr.sprite = leftChargeStand2;
                }
                else
                {
                    sr.sprite = leftChargeStand3;
                }
            }
        }
        if (isShooting)
        {
            if (lookRight)
            {
                sr.sprite = rightShootStand;
            }
            if (lookLeft)
            {
                sr.sprite = leftShootStand;
            }
        }
        if (isMoving && isGround)
        {
            if (lookRight)
            {
                if ((walkSpan * 0.25f < moveTimer && moveTimer <= walkSpan * 0.5f) ||
                    (walkSpan * 0.75f < moveTimer && moveTimer <= walkSpan))
                {
                    sr.sprite = rightStand;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = rightChargeStand1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = rightChargeStand2;
                        }
                        else
                        {
                            sr.sprite = rightChargeStand3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = rightShootStand;
                    }
                }
                else if (0f < moveTimer && moveTimer <= walkSpan * 0.25f)
                {
                    sr.sprite = rightWalk1;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = rightChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = rightChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = rightChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = rightShootWalk;
                    }
                }
                else
                {
                    sr.sprite = rightWalk2;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = rightChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = rightChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = rightChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = rightShootWalk;
                    }
                }
            }
            if (lookLeft)
            {
                if ((walkSpan * 0.25f < moveTimer && moveTimer <= walkSpan * 0.5f) ||
                    (walkSpan * 0.75f < moveTimer && moveTimer <= walkSpan))
                {
                    sr.sprite = leftStand;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = leftChargeStand1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = leftChargeStand2;
                        }
                        else
                        {
                            sr.sprite = leftChargeStand3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = leftShootStand;
                    }
                }
                else if (0f < moveTimer && moveTimer <= walkSpan * 0.25f)
                {
                    sr.sprite = leftWalk1;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = leftChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = leftChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = leftChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = leftShootWalk;
                    }
                }
                else
                {
                    sr.sprite = leftWalk2;
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = leftChargeWalk1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = leftChargeWalk2;
                        }
                        else
                        {
                            sr.sprite = leftChargeWalk3;
                        }
                    }
                    if (isShooting)
                    {
                        sr.sprite = leftShootWalk;
                    }
                }
            }
        }
        if (!isGround)
        {
            if (lookRight)
            {
                if (up)
                {
                    if (upTimer <= 0.1f)
                    {
                        sr.sprite = rightJump;
                    }
                    else
                    {
                        sr.sprite = rightUp;
                    }
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = rightChargeJump1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = rightChargeJump2;
                        }
                        else
                        {
                            sr.sprite = rightChargeJump3;
                        }
                    }
                }
                if (down && !isCharging && !isShooting)
                {
                    if (0f < downTimer && downTimer <= span * 0.25f || span * 0.5f < downTimer && downTimer <= span * 0.75f)
                    {
                        sr.sprite = rightDown1;
                    }
                    else if (span * 0.25f < downTimer && downTimer <= span * 0.5f)
                    {
                        sr.sprite = rightDown2;
                    }
                    else
                    {
                        sr.sprite = rightDown3;
                    }
                }
            }
            if (lookLeft)
            {
                if (up)
                {
                    if (upTimer <= 0.1f)
                    {
                        sr.sprite = leftJump;
                    }
                    else
                    {
                        sr.sprite = leftUp;
                    }
                    if (isCharging)
                    {
                        if (chargeTimer <= chargeSpan * 0.5f)
                        {
                            sr.sprite = leftChargeJump1;
                        }
                        else if (chargeSpan * 0.5f < chargeTimer && chargeTimer <= chargeSpan)
                        {
                            sr.sprite = leftChargeJump2;
                        }
                        else
                        {
                            sr.sprite = leftChargeJump3;
                        }
                    }
                }
                if (down && !isCharging && !isShooting)
                {
                    if (0f < downTimer && downTimer <= span * 0.25f || span * 0.5f < downTimer && downTimer <= span * 0.75f)
                    {
                        sr.sprite = leftDown1;
                    }
                    else if (span * 0.25f < downTimer && downTimer <= span * 0.5f)
                    {
                        sr.sprite = leftDown2;
                    }
                    else
                    {
                        sr.sprite = leftDown3;
                    }
                }
            }
        }
    }
}
