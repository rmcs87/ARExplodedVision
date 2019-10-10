using UnityEngine.XR.ARFoundation;

public class ScanState : GameManagerState
{
    private ScanAnimationController scanAnimation;
    private ARPlaneManager planeManager;

    public ScanState(GameManager gm) : base(gm)
    {
        this.scanAnimation = gm.ScanAnimation;
        this.planeManager = gm.PlaneManager;
    }

    public override void OnStateEnter()
    {
        if (scanAnimation.gameObject.activeSelf != true)
            scanAnimation.gameObject.SetActive(true);
        else
            scanAnimation.StartMoveAnimation();
    }

    public override void OnStateExit()
    {
        scanAnimation.SwitchFromMoveToTapAnimation();
    }

    public override void Tick()
    {
        if (PlanesFound())
        {
            gm.SetState(new SetMiniWorldState(gm));
        }
    }    

    private bool PlanesFound()
    {
        return planeManager.trackables.count > 0;
    }
}
