using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Vector3 _movedir;
    void Update()
    {
        _playerMovement.Move(_movedir);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movedir = context.ReadValue<Vector3>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            _playerMovement.Jump(); 
    }
}
