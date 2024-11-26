using UnityEngine;

public class PlayerFreeLook : MonoBehaviour
{
    [SerializeField] private InputLayer inputLayer;
    public bool IsFreeLooking { get; private set; } = false;

    private void OnEnable() => inputLayer.freeLookEvent += OnFreeLookAround;

    private void OnDisable() => inputLayer.freeLookEvent -= OnFreeLookAround;

    private void FixedUpdate()
    {
        // Rotate towards the camera direction if not free looking
        if (!IsFreeLooking)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 5);
        }
    }

    private void OnFreeLookAround(bool input) => IsFreeLooking = input;
}
