using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARFaceManager))]
public class ARFaceTracking : MonoBehaviour
{
    [SerializeField] private TMP_Text tMP_Text;
    private ARFaceManager aRFaceManager;

    private string logFilePath;

    private void Awake()
    {
        aRFaceManager = GetComponent<ARFaceManager>();
        logFilePath = Application.persistentDataPath + "/FaceMeshLog.txt";
    }

    private void Start()
    {
        // Clear previous log data file if it exists
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }

        aRFaceManager.facesChanged += OnFacesChanged;
    }

    private void Update()
    {
        foreach (ARFace face in aRFaceManager.trackables)
        {
            LogAndDisplayFaceMeshData(face);
        }
    }

    private void LogAndDisplayFaceMeshData(ARFace face)
    {
        string logText = "";

        if (face.vertices != null)
        {
            logText += $"Vertices Count: {face.vertices.Length}\n";
            foreach (var vertex in face.vertices)
            {
                logText += $"Vertex: {vertex}\n";
                Debug.Log($"Vertex: {vertex}");
            }
        }

        if (face.normals != null)
        {
            logText += $"Normals Count: {face.normals.Length}\n";
            foreach (var normal in face.normals)
            {
                logText += $"Normal: {normal}\n";
                Debug.Log($"Normal: {normal}");
            }
        }

        if (face.uvs != null)
        {
            logText += $"UV Count: {face.uvs.Length}\n";
            foreach (var uv in face.uvs)
            {
                logText += $"UV: {uv}\n";
                Debug.Log($"UV: {uv}");
            }
        }

        // Append data to a log file for retrieval
        File.AppendAllText(logFilePath, logText);

        // Display the latest update in the TMP UI text
        tMP_Text.text = logText;
    }

    private void OnFacesChanged(ARFacesChangedEventArgs changes)
    {
        foreach (var face in changes.added)
        {
            Debug.Log("New face added");
        }

        foreach (var face in changes.updated)
        {
            Debug.Log("Face updated");
        }

        foreach (var face in changes.removed)
        {
            Debug.Log("Face removed");
        }
    }

    private void OnDestroy()
    {
        aRFaceManager.facesChanged -= OnFacesChanged;
    }
}