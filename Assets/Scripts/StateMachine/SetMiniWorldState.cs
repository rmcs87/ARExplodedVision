using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SetMiniWorldState : GameManagerState
{
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private GameObject miniWorld;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public SetMiniWorldState(GameManager gm) : base(gm)
    {
        this.raycastManager = gm.RaycastManager;
        this.planeManager = gm.PlaneManager;
        this.miniWorld = gm.MiniWorld;
    }

    public override void OnStateExit()
    {
        gm.ScanAnimation.StopTapAnimation();
    }

    public override void Tick()
    {
        if (TryToPlaceMiniWorld())
        {
            gm.SetState(new PlayingState(gm));
        }
    }

    private bool TryToPlaceMiniWorld()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return false;

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            var hitPose = s_Hits[0].pose;

            PlaceMiniWorld(hitPose.position, hitPose.rotation);

            gm.StartCoroutine(disableCloudAndPlanes());

            return true;
        }
        return false;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private IEnumerator disableCloudAndPlanes()
    {
        var PointCloud = gm.GetComponent<ARPointCloudManager>();
        PointCloud.enabled = false;
        PointCloud.SetTrackablesActive(false);

        planeManager.enabled = false;
        planeManager.SetTrackablesActive(false);       

        yield return null;
    }

    private void PlaceMiniWorld(Vector3 position, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(Vector3.up * 180);

        gm.GetComponent<ARSessionOrigin>().MakeContentAppearAt(
            miniWorld.transform,
            new Vector3(position.x, position.y, position.z),
            rotation);        
    }

}