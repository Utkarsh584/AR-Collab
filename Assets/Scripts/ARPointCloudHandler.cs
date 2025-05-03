using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPointCloudManager))]
public class ARPointCloudHandler : MonoBehaviour
{
    public ARPointCloudManager aRPointCloud;
    List<Vector3> pointsCloud = new List<Vector3>();
    public TMPro.TMP_Text tMP_Text;

    private string filePath;

    private void Awake()
    {
        aRPointCloud = GetComponent<ARPointCloudManager>();
        filePath = Path.Combine(Application.persistentDataPath, "PointCloudData.txt");
    }

    void OnEnable()
    {
        if (aRPointCloud != null)
        {
            aRPointCloud.pointCloudsChanged += OnPointCloudsChanged;
        }
    }

    void OnDisable()
    {
        if (aRPointCloud != null)
        {
            aRPointCloud.pointCloudsChanged -= OnPointCloudsChanged;
        }
    }

    void Update()
    {
        tMP_Text.text = $"Point Cloud Count: {pointsCloud.Count}";
    }

    void OnPointCloudsChanged(ARPointCloudChangedEventArgs eventArgs)
    {
        Debug.Log("Point Cloud Changed Event Triggered");

        foreach (var pointCloud in eventArgs.updated)
        {
            if (pointCloud.positions.HasValue)
            {
                foreach (Vector3 point in pointCloud.positions.Value)
                {
                    if (!pointsCloud.Contains(point)) // Avoid duplicate entries
                    {
                        pointsCloud.Add(point);
                    }
                }
            }
        }

        Debug.Log($"Total collected points: {pointsCloud.Count}");
        SavePointsToFile();
    }

    void SavePointsToFile()
    {
        Debug.Log($"Appending {pointsCloud.Count} points to file...");

        if (pointsCloud.Count == 0)
        {
            Debug.LogWarning("No points to save.");
            return;
        }

        using (StreamWriter writer = new StreamWriter(filePath, true)) // Append data
        {
            foreach (Vector3 point in pointsCloud)
            {
                writer.WriteLine($"{point.x},{point.y},{point.z}");
            }
        }
        Debug.Log($"Point cloud data appended to: {filePath}");
    }

    void TestFileWriting()
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine("Test line 1");
            writer.WriteLine("Test line 2");
        }
        Debug.Log("Test data written to file.");
    }
}