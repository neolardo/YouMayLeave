using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    [SerializeField][Range(1, 30)] float jumpForce = 15;
    [SerializeField][Range(5, 8)] float horizontalSpeed = 6;
    private Player player;
    private BoxCollider2D col;
    private CancellationToken playerCT;
    private InputActionMap actionMap;
    private InputAction moveAction;
    private InputAction attackAction;
    private Vector2 moveVector;
    private Vector2 frameVelocity;
    private bool canJumpAgain = true;
    private bool isGrounded;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    private LayerMask collisionMask;
    private RaycastOrigins raycastOrigins;
    private CollisionInfo collisionInfo;

    private const float jumpDelay = 0.3f;
    private const float jumpGravity = 30;
    private const float fallGravity = 50;
    private const int horizontalRayCount = 3;
    private const int verticalRayCount = 3;
    private const float skinWidth = 0.1f;

    void Awake()
    {
        player = GetComponent<Player>();
        col = GetComponent<BoxCollider2D>();
        actionMap = actions.FindActionMap("Gameplay");
        moveAction = actionMap.FindAction("Move");
        attackAction = actionMap.FindAction("Attack");
        attackAction.performed += OnAttack;
        playerCT = this.GetCancellationTokenOnDestroy();
        collisionMask = Physics2D.GetLayerCollisionMask(Globals.PlayerLayer);
        CalculateRaySpacing();
    }

    void Update()
    {
        GatherInputs();
    }

    private void GatherInputs()
    {
        moveVector = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        DetectCollisions();
        Move();
    }

    private void UpdateVelocity()
    {
        isGrounded = collisionInfo.below;
        if (collisionInfo.above || collisionInfo.below)
        {
            frameVelocity = new Vector2(frameVelocity.x, 0);
        }
        HandleSidewaysMovement();
        ApplyGravity();
        HandleJump();
    }


    private void DetectCollisions()
    {
        collisionInfo.Reset();
        UpdateRaycastOrigins();
        if (frameVelocity.x != 0)
        {
            HorizontalCollisions();
        }
        if (frameVelocity.y != 0)
        {
            VerticalCollisions();
        }
    }

    private void Move()
    {
        transform.Translate(frameVelocity);
    }

    private void HandleSidewaysMovement()
    {
        if (Mathf.Abs(moveVector.x) > Globals.MoveInputDelta)
        {
            frameVelocity = new Vector2(moveVector.x* horizontalSpeed * Time.fixedDeltaTime, frameVelocity.y);
        }
        else
        {
            frameVelocity = new Vector2(0, frameVelocity.y);
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            if (frameVelocity.y > 0) // jumping
            {
                frameVelocity = new Vector2(frameVelocity.x, frameVelocity.y - (jumpGravity * Time.fixedDeltaTime * Time.fixedDeltaTime));
            }
            else // falling
            {
                frameVelocity = new Vector2(frameVelocity.x, frameVelocity.y - (fallGravity * Time.fixedDeltaTime * Time.fixedDeltaTime));
            }
        }
    }

    private void HandleJump()
    {
        if (moveVector.y > Globals.MoveInputDelta && isGrounded && canJumpAgain)
        {
            frameVelocity = new Vector2(frameVelocity.x, jumpForce *Time.fixedDeltaTime);
            isGrounded = false;
            RestrictJumpingAgainAsync(playerCT);
        }
    }

    void HorizontalCollisions()
    {
        float directionX = Mathf.Sign(frameVelocity.x);
        float rayLength = Mathf.Abs(frameVelocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                frameVelocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions()
    {
        float directionY = Mathf.Sign(frameVelocity.y);
        float rayLength = Mathf.Abs(frameVelocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + frameVelocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                frameVelocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        player.Attack();
    }

    private async UniTaskVoid RestrictJumpingAgainAsync(CancellationToken token)
    {
        canJumpAgain = false;
        await UniTask.WaitForSeconds(jumpDelay, cancellationToken: token);
        canJumpAgain = true;
    }

    void OnEnable()
    {
        actionMap.Enable();
    }

    void OnDisable()
    {
        actionMap.Disable();
    }
}
