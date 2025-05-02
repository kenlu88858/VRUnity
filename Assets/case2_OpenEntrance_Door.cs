using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class case2_OpenEntrance_Door : MonoBehaviour
{
    [Header("左右門的 Transform")]
    public Transform leftDoor;  // Entrance_Door_002
    public Transform rightDoor; // Entrance_Door_003

    [Header("開門後的位置與旋轉")]
    public Vector3 leftDoorOpenPosition = new Vector3(0.0754f, 0.103f, 0f);
    public Vector3 rightDoorOpenPosition = new Vector3(-0.0731f, 0.076f, 0f);
    public Vector3 leftDoorOpenRotation = new Vector3(0f, 0f, -90f);
    public Vector3 rightDoorOpenRotation = new Vector3(0f, 0f, 90f);

    [Header("開門速度")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 2f;

    private Vector3 leftDoorClosedPosition;
    private Vector3 rightDoorClosedPosition;
    private Quaternion leftDoorClosedRotation;
    private Quaternion rightDoorClosedRotation;

    //public bool canopen = false;

    private bool isOpen = false;
    //private UnityEngine.XR.InputDevice rightHandController;

    public InputActionProperty toggleAction; // 用於開關門的 Input Action

    //private bool previousButtonState = false;  // 用來紀錄前一次的按鈕狀態

    public Transform cameraTransform;// 用來設定相機物件的 Transform

    void Start()
    {
        // 初始化VR控制器 (右手)
        //InitializeRightHandController();

        // 記錄關門時的初始位置與旋轉
        leftDoorClosedPosition = leftDoor.localPosition;
        rightDoorClosedPosition = rightDoor.localPosition;
        leftDoorClosedRotation = leftDoor.localRotation;
        rightDoorClosedRotation = rightDoor.localRotation;
    }

    void Update()
    {
        //if (!rightHandController.isValid)
        //{
            //InitializeRightHandController();
        //}

        // 檢測右手A按鍵 (主按鈕)
        //bool primaryButtonPressed;
        if (Vector3.Distance(cameraTransform.position, new Vector3(29.6f, 39.31f, 32.71f)) < 0.5f)
        {
            Debug.Log("dooropen!");
            ToggleDoor();
        }

        //Vector3 cameraPosition = cameraTransform.position;
        //Debug.Log("Camera Position: " + cameraPosition);


        if (isOpen && Vector3.Distance(cameraTransform.position, new Vector3(29.6f, 39.31f, 32.71f)) < 0.3f)
        {
            // 載入新的場景
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // 等待 1 秒鐘
        yield return new WaitForSeconds(0.5f);

        // 載入新的場景
        SceneManager.LoadScene("case2-5_Garden");
    }



    /*     void InitializeRightHandController()
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

            if (devices.Count > 0)
            {
                rightHandController = devices[0];
            }
        } */

    public void ToggleDoor()
    {
        StopAllCoroutines();
        if(!isOpen)
        {
            StartCoroutine(MoveAndRotateDoor(leftDoor, leftDoorOpenPosition, Quaternion.Euler(leftDoorOpenRotation)));
            StartCoroutine(MoveAndRotateDoor(rightDoor, rightDoorOpenPosition, Quaternion.Euler(rightDoorOpenRotation)));
            isOpen = true;
        }
    }

    private System.Collections.IEnumerator MoveAndRotateDoor(Transform door, Vector3 targetPosition, Quaternion targetRotation)
    {
        float time = 0f;
        Vector3 startPosition = door.localPosition;
        Quaternion startRotation = door.localRotation;

        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            door.localPosition = Vector3.Lerp(startPosition, targetPosition, time);
            door.localRotation = Quaternion.Slerp(startRotation, targetRotation, time);
            yield return null;
        }

        door.localPosition = targetPosition; 
        door.localRotation = targetRotation; 
    }
}
