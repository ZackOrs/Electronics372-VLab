using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float xBoundMax = 5.0f;
    [SerializeField] float xBoundMin = -5.0f;
    [SerializeField] float zBoundMax = 8.0f;
    [SerializeField] float zBoundMin = -2.0f;

    [SerializeField] float gravity = -13.0f;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if(lockCursor){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Globals.cameraAttachedToPlayer)
        {
            UpdateMouseLook();
            UpdateMovement();
            CheckPosition();
        }
    }
    void UpdateMouseLook()
    {

        if(!(Globals.gamePaused || Globals.menuOpened) )
        {
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

            cameraPitch -= currentMouseDelta.y * mouseSensitivity;
            cameraPitch = Mathf.Clamp(cameraPitch,-90.0f, 90.0f);
            playerCamera.localEulerAngles = Vector2.right * cameraPitch;

            transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity) ;
        }
    }

    void UpdateMovement()
    {

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if(controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
        
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
    
        controller.Move(velocity * Time.deltaTime);

    }
    void CheckPosition()
    {
        Vector3 position = GameObject.Find("Player").transform.position;
        if(position.x > xBoundMax){
            position.x = xBoundMax;
            GameObject.Find("Player").transform.position = position;
        }
        else if(position.x < xBoundMin){
            position.x = xBoundMin;
            GameObject.Find("Player").transform.position = position;
        }
        if(position.z > zBoundMax){
            position.z = zBoundMax;
            GameObject.Find("Player").transform.position = position;
        }
                if(position.z < zBoundMin){
            position.z = zBoundMin;
            GameObject.Find("Player").transform.position = position;
        }
    }

}
