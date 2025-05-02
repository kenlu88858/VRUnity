using UnityEngine;
public class case2_MovePeasantElder : MonoBehaviour
{
    public Transform peasantElder;  // Peasant Elder Halden 物件
    //public Vector3 startPosition = new Vector3(35.576f, 38.13972f, 46.89f);
    public Vector3 targetPosition = new Vector3(34.0328f, 38.13972f, 42.65032f);
    public float moveSpeed = 0.0001f;  // 移動速度
    public float followDistance = 1.5f;

    private Animator peasantElderAnimator;  // 用來控制動畫的 Animator

    bool starting = false;

    public Transform cameraTransform;

    public GameObject missionCanvas;

    public GameObject missionplane;

    public GameObject glowEffect1;

    public GameObject glowEffect2;

    public GameObject glowEffect3;

    public GameObject glowEffect4;

    //private bool missionComplete = false;

    

    void Start()
    {
        // 設定物件初始位置
        //peasantElder.position = startPosition;

        // 取得物件上的 Animator 組件
        peasantElderAnimator = peasantElder.GetComponent<Animator>();

        // 開始時設定 IsRunning 為 true 以播放 Run3 動畫
        if (peasantElderAnimator != null)
        {
            peasantElderAnimator.SetBool("IsRunning", true);  // 啟動跑步動畫
        }
        //cameraTransform.position = new Vector3(32.2f, 40.0f, 39.14f);
    }

    void Update()
    {
        Vector3 cameraPosition = cameraTransform.position;
        Debug.Log("Camera Position: " + cameraPosition);
        //Debug.Log("peasantElder Position: " + peasantElder.position);

        // 持續移動 Peasant Elder Halden 物件
        if (peasantElder.position != targetPosition && !starting)
        {
            peasantElder.position = Vector3.MoveTowards(peasantElder.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // 當物件達到目標位置，停止 "Run3" 動畫
            if (peasantElderAnimator != null && !starting)
            {
                peasantElderAnimator.SetBool("IsRunning", false);  // 停止動畫
                starting = true;
            }
        }

        if(starting){         
            Vector3 directionToCamera = (cameraPosition - peasantElder.position).normalized;
            Vector3 stopPosition = cameraPosition - directionToCamera * followDistance;
            float distanceToCamera = Vector3.Distance(peasantElder.position, stopPosition);

            if(distanceToCamera > 0.3f){
                peasantElderAnimator.SetBool("IsRunning", true);
                Vector3 moveDirection = (cameraPosition - peasantElder.position).normalized;
                if (moveDirection != Vector3.zero){
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                    peasantElder.rotation = Quaternion.Slerp(peasantElder.rotation, targetRotation, Time.deltaTime * 5f);
                }
                Vector3 followPosition = Vector3.MoveTowards(peasantElder.position, cameraPosition, moveSpeed * Time.deltaTime);
                peasantElder.position = new Vector3(followPosition.x, peasantElder.position.y, followPosition.z); // 固定 Y 軸
            }
            else{
                peasantElderAnimator.SetBool("IsRunning", false);
            }
        }

        if(Vector3.Distance(cameraTransform.position, new Vector3(30.45f, 39.43f, 46.02f)) < 0.6f){
            //missionComplete = true;
            Debug.Log("已至指定位置！");
            glowEffect1.SetActive(false);
            glowEffect2.SetActive(false);
            glowEffect3.SetActive(false);
            glowEffect4.SetActive(false);
            missionCanvas.SetActive(true);
            missionplane.SetActive(true);
        }
    }
}
