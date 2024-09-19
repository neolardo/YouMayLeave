using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    [HideInInspector] public InputAction attackAction;
    [HideInInspector] public Vector2 moveVector;
    private InputActionMap actionMap;
    private InputAction moveAction;

    void Awake()
    {
        actionMap = actions.FindActionMap(Constants.GamePlayActionMap);
        moveAction = actionMap.FindAction(Constants.MoveInputAction);
        attackAction = actionMap.FindAction(Constants.AttackInputAction);
    }

    void Update()
    {
        GatherInputs();
    }

    private void GatherInputs()
    {
        moveVector = moveAction.ReadValue<Vector2>();
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
