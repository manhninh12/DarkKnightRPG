using UnityEngine;
#if UNITY_6000_0_OR_NEWER || true // Đảm bảo dùng đúng namespace cho Unity 6 (Cinemachine 3)
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraAutoTarget : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        TryFindTarget();
    }

    void Update()
    {
        // Liên tục kiểm tra, nếu camera bị mất mục tiêu (bị null do chuyển scene) thì tìm lại
        if (vcam != null && vcam.Target.TrackingTarget == null)
        {
            TryFindTarget();
        }
    }

    void TryFindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && vcam != null)
        {
            // Trong Cinemachine 3 của Unity 6, mục tiêu là TrackingTarget
            vcam.Target.TrackingTarget = player.transform;
        }
    }
}
#endif
