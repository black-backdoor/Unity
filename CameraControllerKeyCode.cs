using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerKeyCode : MonoBehaviour{
    [Header("Camera")]
    public Camera cam;

    [Header("Target")]
    public Transform followTransform;
    public bool followTarget;
    public bool allowFollowExit;
    public bool allowFollowStart;

    [Header("Keyboard Controls")]
    [Tooltip("Controls via the keyboard")]
    public KeyCode key_forward;
    public KeyCode key_backward;
    public KeyCode key_left;
    public KeyCode key_right;
    public KeyCode key_zoomIn;
    public KeyCode key_zoomOut;
    public KeyCode key_fast;
    public KeyCode key_escape_followTransform;

    [Header("Settings")]
    public bool allowDrag;
    public bool allowRotation;
    public bool allowZoom;
    public bool allowMove;


    public float normalSpeed;
    public float fastSpeed;

    private float _movementSpeed;
    public float movementTime;

    private Vector3 _newPosition;

    public float rotationAmount;
    private Quaternion _newRotation;

    public Vector3 zoomAmount;
    private Vector3 _newZoom;

    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;

    private Vector3 _rotateStartPosition;
    private Vector3 _rotateCurrentPosition;

    // Start is called before the first frame update
    void Start()
    {
        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = cam.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
       update_Camera();
    }

    void update_Camera(){
        if(!followTarget){
            if(allowDrag){
                HandleDrag();
            }
            if(allowRotation){
                HandleRotation();
            }
            if(allowMove){
                HandleMovementInput();
            }
            if(allowZoom){
                HandleZoom();
            }
            transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime);
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, _newZoom, Time.deltaTime * movementTime);
        }
        else if(followTarget){
            if(followTransform != null){
                transform.position = followTransform.position;
            }
        }
        if(allowFollowExit){
            if(followTransform != null){
                if(Input.GetKeyDown(key_escape_followTransform)){
                    followTransform = null;
                }
            }
        }
    }

    void HandleDrag(){
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);

                _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }

    }

    void HandleRotation(){
        if (Input.GetKey(KeyCode.Q)){
            _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E)){
            _newRotation *= Quaternion.Euler(Vector3.down * rotationAmount);
        }

        if (Input.GetMouseButtonDown(2)){
            _rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)){
            _rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;

            _rotateStartPosition = _rotateCurrentPosition;

            _newRotation *= Quaternion.Euler(Vector3.down * difference.x / 5f);
        }
    }

    void HandleMovementInput(){
        _movementSpeed = (Input.GetKey(key_fast)) ? fastSpeed : normalSpeed;

        if (Input.GetKey(key_forward) || Input.GetKey(KeyCode.UpArrow)){
            _newPosition += (transform.forward * _movementSpeed);
        }

        if (Input.GetKey(key_backward) || Input.GetKey(KeyCode.DownArrow)){
            _newPosition -= (transform.forward * _movementSpeed);
        }

        if (Input.GetKey(key_left) || Input.GetKey(KeyCode.RightArrow)){
            _newPosition += (transform.right * _movementSpeed);
        }

        if (Input.GetKey(key_right) || Input.GetKey(KeyCode.LeftArrow)){
            _newPosition -= (transform.right * _movementSpeed);
        }
    }

    void HandleZoom(){
        //mouse zoom
        if (Input.mouseScrollDelta.y != 0){
            _newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        //key zoom
        if (Input.GetKey(key_zoomIn)){
            _newZoom += zoomAmount;
        }

        if (Input.GetKey(key_zoomOut)){
            _newZoom -= zoomAmount;
        }
    }
}
