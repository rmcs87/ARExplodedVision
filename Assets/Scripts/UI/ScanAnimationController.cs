using UnityEngine;

public class ScanAnimationController : MonoBehaviour
{
    [SerializeField] Animator moveDeviceAnimation;
    [SerializeField] Animator tapToPlaceAnimation;

    const string k_FadeOffAnim = "FadeOff";
    const string k_FadeOnAnim = "FadeOn";     

    public void SwitchFromMoveToTapAnimation()
    {
        moveDeviceAnimation.SetTrigger(k_FadeOffAnim);
        tapToPlaceAnimation.SetTrigger(k_FadeOnAnim);
    }

    public void StartMoveAnimation()
    {
        moveDeviceAnimation.SetTrigger(k_FadeOnAnim);
    }

    public void StopAllAnimations()
    {
        moveDeviceAnimation.SetTrigger(k_FadeOffAnim);
        tapToPlaceAnimation.SetTrigger(k_FadeOffAnim);
    }

    public void StopTapAnimation()
    {        
        tapToPlaceAnimation.SetTrigger(k_FadeOffAnim);
    }

}