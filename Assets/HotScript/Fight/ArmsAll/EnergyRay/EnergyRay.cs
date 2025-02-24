


using UnityEngine;

public class EnergyRay : ArmChildBase
{
    LineRenderer _lineRenderer;
    public LineRenderer lineRender
    {
        get
        {
            if (_lineRenderer == null)
            {
                _lineRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
                return _lineRenderer;
            }else {
                return _lineRenderer;
            }
        }
    }
    public override void ChangeScale(float scaleFactor)
    {
        base.ChangeScale(scaleFactor);
        lineRender.startWidth *= scaleFactor;
        lineRender.endWidth *= scaleFactor;
    }
}



