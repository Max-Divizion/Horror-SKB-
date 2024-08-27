using UnityEngine;
using UnityEngine.XR;

public class VRMovement : MonoBehaviour
{
    public bool Sprint;

    void Update()
    {
        // �������� ��������� ������ ����������� (����� �������� �� ������, ���� �����)
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // ���������, ������ �� ������ �� ����� ����������� (��������, PrimaryButton)
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
