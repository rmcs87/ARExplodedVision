using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private GameObject wagon;

    public static event Action<WagonComponentInfo> OnSelectedComponentChange = delegate { };
    public static event Action OnCloseWagon = delegate { };

    private Camera cam;
    private Animator wagonAnimator;    
    private TextAsset dataFile;
    private WagonComponentInfo[] wagonComponents;    
	private GameObject previewsFocusedTarget;
    private bool isExploded = false;

    void Start()
	{
        cam = Camera.main;
        wagonAnimator = wagon.GetComponent<Animator>();

        //Load Components info from Json;
        dataFile = Resources.Load<TextAsset>("Data");
        string[] wagonObjectJsons = dataFile.text.Split ('\n');
        wagonComponents = new WagonComponentInfo[wagonObjectJsons.Length];
        for (int i = 0; i < wagonObjectJsons.Length; i++)
		{
            wagonComponents[i] = JsonUtility.FromJson<WagonComponentInfo>(wagonObjectJsons[i]);
		}

    }
    
    private void Update()
    {        
        if (isExploded && Input.GetMouseButtonDown(0)) { 
            Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
            CheckRaySelection(cameraRay);
        }
    }

    void CheckRaySelection(Ray ray)
    {
        RaycastHit targetHit;
        if (Physics.Raycast(ray, out targetHit, 100))
        {
            var target = targetHit.collider.gameObject;
            if (target.tag != "WagonComponent")
                return;            

            if (target != previewsFocusedTarget)
            {
                ChangeFocusTarget(target);
            }
        }
    }

    private void ChangeFocusTarget(GameObject target)
    {
        OnSelectedComponentChange(wagonComponents[target.GetComponent<WagonComponentId>().Id]);
        target.GetComponent<Renderer>().material.color = Color.blue;
        if (previewsFocusedTarget != null)
        {
            previewsFocusedTarget.GetComponent<Renderer>().material.color = Color.white;
        }
        previewsFocusedTarget = target;
    }

    //Called by ButtonToggle
    public void ExplodeWagon()
    {
        if (isExploded)        
            CloseWagon();
        else
            wagonAnimator.SetTrigger("Open");
        
        isExploded = !isExploded;
    }

    //Called by ButtonToggle
    private void CloseWagon()
    {
        wagonAnimator.SetTrigger("Close");
        OnCloseWagon();
        if (previewsFocusedTarget != null)
        {
            previewsFocusedTarget.GetComponent<Renderer>().material.color = Color.white;
        }
        previewsFocusedTarget = null;
    } 

    public void Zoom(Scrollbar scrollbar)
    {
        //var sessionOrigin = GetComponent<ARSessionOrigin>();

        float scaleValue = 15 - scrollbar.value * 14;

        gameObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
}
