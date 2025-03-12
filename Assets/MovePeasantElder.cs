using UnityEngine;

public class MovePeasantElder : MonoBehaviour
{
    public Transform peasantElder;  // Peasant Elder Halden 物件
    //public Vector3 startPosition = new Vector3(35.576f, 38.13972f, 46.89f);
    public Vector3 targetPosition = new Vector3(34.0328f, 38.13972f, 42.65032f);
    public float moveSpeed = 0.0001f;  // 移動速度

    private Animator peasantElderAnimator;  // 用來控制動畫的 Animator

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
    }

    void Update()
    {
        // 持續移動 Peasant Elder Halden 物件
        if (peasantElder.position != targetPosition)
        {
            peasantElder.position = Vector3.MoveTowards(peasantElder.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // 當物件達到目標位置，停止 "Run3" 動畫
            if (peasantElderAnimator != null)
            {
                peasantElderAnimator.SetBool("IsRunning", false);  // 停止動畫
            }
        }
    }
}
