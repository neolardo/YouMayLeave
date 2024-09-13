using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    [SerializeField] Rigidbody2D rb;

    private InputActionMap actionMap;
    private InputAction moveAction;
    private InputAction attackAction;
    private Vector2 moveVector;

    void Awake()
    {
        actionMap = actions.FindActionMap("Gameplay");
        moveAction = actionMap.FindAction("Move");
        attackAction = actionMap.FindAction("Attack");
        attackAction.performed += OnAttack;
    }

    void Start()
    {
    }

    void Update()
    {
        moveVector = moveAction.ReadValue<Vector2>();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attacked!");
    }

    void FixedUpdate()
    {
        if(moveVector.x < 0)
        {   
            rb.AddForce(Vector2.left, ForceMode2D.Force);
        }
        else if(moveVector.x > 0)
        {
            rb.AddForce(Vector2.right, ForceMode2D.Force);
        }
        if (moveVector.y < 0)
        {
            rb.AddForce(Vector2.down, ForceMode2D.Force);
        }
        else if (moveVector.y > 0)
        {
            rb.AddForce(Vector2.up, ForceMode2D.Force);
        }
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
