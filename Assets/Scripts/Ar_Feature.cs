using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class Ar_Feature : MonoBehaviour
{
    public TMPro.TMP_Text tMP_Text;

    private void Awake()
    {
        GetComponent<ARPlaneManager>().planesChanged += ARFeatures_planesChanged;
    }

    private void Start()
    {
        if (LoaderUtility.GetActiveLoader()?.GetLoadedSubsystem<XRPlaneSubsystem>() != null)
        {
            // XRPlaneSubsystem was loaded. The platform supports plane detection.
            tMP_Text.text = "Supported";
        }
        else
        {
            tMP_Text.text = "Not Supported";
        }
    }

    private void ARFeatures_planesChanged(ARPlanesChangedEventArgs changes)
    {
        // Clear text to avoid overlaps
        tMP_Text.text = "";

        // Handle added planes
        foreach (var plane in changes.added)
        {
            tMP_Text.text += $"Plane Added: {plane.trackableId}\n";
        }

        // Handle updated planes
        foreach (var plane in changes.updated)
        {
            tMP_Text.text += $"Plane Updated: {plane.trackableId}\n";
        }

        // Handle removed planes
        foreach (var plane in changes.removed)
        {
            tMP_Text.text += $"Plane Removed: {plane.trackableId}\n";
        }
    }

    private void OnDestroy()
    {
        GetComponent<ARPlaneManager>().planesChanged -= ARFeatures_planesChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
