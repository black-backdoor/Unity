using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    //public static CameraSwitcher instance;
    public bool allow_change_cam;
    
    //if disabled camera won't change wenn specific key is pressed
    public bool allow_SpecificCameraKeys;

    [Header("List of available cameras")]
    public GameObject[] Cameras;
    public int _cameraID = 0;
    public int editor_cam_list_id;

    [Header("Keyboard Controls")]
    [Tooltip("Controls via the keyboard")]

    public KeyCode ChangeCameraKey;

    public KeyCode[] SelectSpecificCameraKeys;

    // Update is called once per frame
    void Update()
    {
        if(allow_change_cam){
            if(Input.GetKeyDown(ChangeCameraKey)){
                if ((_cameraID + 1) >= Cameras.Length){
                    _cameraID = 0;
                }
                else {
                    _cameraID = _cameraID + 1;
                }
                ChooseCamera(_cameraID);
            }
            if(allow_SpecificCameraKeys){
                if(Cameras.Length != 0){
                    for (int i = 0; i < Cameras.Length; i++)
                    {
                        if (Input.GetKeyDown(SelectSpecificCameraKeys[i]))
                        {
                            ChooseCamera(i);
                        }
                    }

                }
            }
        }
    }
    void ChooseCamera(int _idCam)
    {
        if (_idCam >= 0 && _idCam < Cameras.Length)
        {
            foreach (GameObject Cam in Cameras)
            {
                Cam.SetActive(false);
            }
            Cameras[_idCam].SetActive(true);
        }
    }
}
