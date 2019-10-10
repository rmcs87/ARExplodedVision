using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI componentTitleText;
    [SerializeField] private TextMeshProUGUI componentContentText;

    private Animator animator;
    private bool isShowing = false;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        InteractionController.OnSelectedComponentChange += InteractionControl_OnSelectedComponentChange;
        InteractionController.OnCloseWagon += InteractionControl_OnCloseWagon;
    }

    private void OnDestroy()
    {
        InteractionController.OnSelectedComponentChange -= InteractionControl_OnSelectedComponentChange;
        InteractionController.OnCloseWagon -= InteractionControl_OnCloseWagon;
    }

    private void InteractionControl_OnCloseWagon()
    {
        Hide();
    }

    private void InteractionControl_OnSelectedComponentChange(WagonComponentInfo componentInfo)
    {
        if (isShowing)
        {
            HideAndShow(componentInfo);
        }
        else
        {
            Show(componentInfo);
        }
    }

    private void HideAndShow(WagonComponentInfo componentInfo)
    {
        Hide();
        StartCoroutine(ShowAfterDelay(componentInfo));
    }

    private void Show(WagonComponentInfo componentInfo)
    {
        isShowing = true;
        componentTitleText.text = componentInfo.title;
        componentContentText.text = componentInfo.content;
        animator.SetBool("isOn", true);        
    }    

    private void Hide()
    {
        isShowing = false;
        animator.SetBool("isOn", false);
    }

    private IEnumerator ShowAfterDelay(WagonComponentInfo componentInfo)
    {
        yield return new WaitForSeconds(1.5f);
        Show(componentInfo);
    }    
}
