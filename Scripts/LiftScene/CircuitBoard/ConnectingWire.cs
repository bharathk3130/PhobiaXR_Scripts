using KBCore.Refs;
using UnityEngine;

public class ConnectingWire : ValidatedMonoBehaviour
{
    [SerializeField, Child] WireTip _wireTip;
    [SerializeField, Self] LineRenderer _lineRenderer;

    void Update()
    {
        Vector3 wireTipPos = _wireTip.GetProjectedPosition();
        Vector3 wireTipLocalPos = transform.InverseTransformPoint(wireTipPos);
        _lineRenderer.SetPosition(1, wireTipLocalPos);
    }
}