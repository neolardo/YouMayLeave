using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    [HideInInspector] public InputAction anythingAction;
    private InputActionMap actionMap;

    void Awake()
    {
        actionMap = actions.FindActionMap(Constants.UIActionMap);
        anythingAction = actionMap.FindAction(Constants.AnythingInputAction);
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
