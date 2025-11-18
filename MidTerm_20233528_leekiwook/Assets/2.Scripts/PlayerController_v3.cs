using UnityEngine;

public class PlayerController_v3 : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float jumpForce = 10.0f;
    
    // Animator 컴포넌트 참조 (private - Inspector에 안 보임)
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private int score = 0;

    //리스폰용 시작 위치
    private Vector3 startPosition;
    
    void Start()
    {
    
        // 게임 시작 시 한 번만 - Animator 컴포넌트 찾아서 저장
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        // 게임 시작 시 위치를 저장 - 새로 추가!
        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);

        
        // 디버그: 제대로 찾았는지 확인
        if (animator != null)
        {
            Debug.Log("Animator 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Animator 컴포넌트가 없습니다!");
        }

        if (spriteRenderer != null)
        {
            Debug.Log("SpriteRenderer 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("SpriteRenderer 컴포넌트가 없습니다!");
        }

        if (rb != null)
        {
            Debug.Log("Rigidbody2D 컴포넌트를 찾았습니다!");
        }
        else
        {
            Debug.LogError("Rigidbody2D 컴포넌트가 없습니다!");
        }
    }

    void Update()
    {
        // 이동 벡터 계산
        float moveX = 0f;

        // A키 (왼쪽) - 스프라이트 뒤집기
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            spriteRenderer.flipX = true; // X축 뒤집기

        }

        // D키 (오른쪽) - 원래 방향
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
            spriteRenderer.flipX = false; // 원래 크기

        }

        // 달리기 속도 계산
        float currentMoveSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = moveSpeed * 2f;
            Debug.Log("달리기 모드 활성화!");
        }

        //물리 기반 이동
        rb.linearVelocity = new Vector2(moveX * currentMoveSpeed, rb.linearVelocity.y);


        // 속도 계산: 이동 중이면 moveSpeed, 아니면 0
        float currentSpeed = Mathf.Abs(rb.linearVelocity.x);

        // Animator에 속도 전달
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            Debug.Log("Current Speed: " + currentSpeed);
        }

        // 점프 입력 (한 번만 실행되어야 하므로 GetKeyDown!)
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            if (animator != null)
            {
                animator.SetTrigger("Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                Debug.Log("점프!");
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Ground" Tag를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 착지!");
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에서 떨어짐");
            isGrounded = false;
        }
    }

    //아이템 수집 감지(Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;  // 점수 증가
            Debug.Log("코인 획득! 현재 점수: " + score);
            Destroy(other.gameObject);  // 코인 제거
        }

        // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉🎉🎉 게임 클리어! 🎉🎉🎉");
            Debug.Log("최종 점수: " + score + "점");
            
            // 캐릭터 조작 비활성화
            enabled = false;
        }

    }

}