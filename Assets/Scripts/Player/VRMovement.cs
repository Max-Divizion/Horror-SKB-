using UnityEngine;
using UnityEngine.XR;

public class VRMovement : MonoBehaviour
{
    public bool Sprint;

    void Update()
    {
        // ѕолучаем состо€ние левого контроллера (можно изменить на правый, если нужно)
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // ѕровер€ем, нажата ли кнопка на левом контроллере (например, PrimaryButton)
        if (leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed))
        {
            Sprint = isPressed;
        }
        else
        {
            Sprint = false;
        }
    }
}
