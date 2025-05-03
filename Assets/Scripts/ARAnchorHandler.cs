using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager), typeof(ARAnchorManager))]
public class ARAnchorHandler : MonoBehaviour
{
    private ARRaycastManager aRRaycastManager;
    private ARAnchorManager aRAnchorManager;

    [SerializeField] private GameObject arAnchorPrefab;
    private GameObject arAnchor;

    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    [SerializeField] private TMPro.TMP_Text tMP_Text;

    private NewControls newControls; // Reference to the New Input System

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRAnchorManager = GetComponent<ARAnchorManager>();

        newControls = new NewControls(); // Initialize Input System
    }

    private void OnEnable()
    {
        newControls.Input.Touch.performed += ctx => PlaceAnchor(); // Bind Touch Action
        newControls.Enable();
    }

    private void OnDisable()
    {
        newControls.Input.Touch.performed -= ctx => PlaceAnchor();
        newControls.Disable();
    }

    private void PlaceAnchor()
    {
        Vector2 screenPoint = new Vector2(Screen.width / 2, Screen.height / 2); // Center of screen

        if (aRRaycastManager.Raycast(screenPoint, m_Hits, TrackableType.AllTypes))
        {
            tMP_Text.text = "Raycast :: ";
            if (m_Hits.Count > 0)
            {
                tMP_Text.text += "Count :: ";
                if (arAnchor == null)
                {
                    tMP_Text.text += "Inst :: ";
                    arAnchor = Instantiate(arAnchorPrefab, m_Hits[0].pose.position, m_Hits[0].pose.rotation);
                    arAnchor.AddComponent<ARAnchor>();
                }
                else
                {
                    tMP_Text.text += $"Pos : {m_Hits[0].pose.position} :: ";
                    arAnchor.transform.position = m_Hits[0].pose.position;
                }
            }
        }
    }
}