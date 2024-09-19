using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public void SetCameraTarget(GameObject target)
    {
        _virtualCamera.LookAt = target.transform;
        _virtualCamera.Follow = target.transform;
    }
}
