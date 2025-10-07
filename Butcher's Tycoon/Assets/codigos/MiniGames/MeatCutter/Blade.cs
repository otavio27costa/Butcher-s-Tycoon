using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private float minCuttingVelocity;

    private bool isCutting;
    private Vector2 previousPos;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCut();
        }
    }

    void StartCutting()
    {
        isCutting = true;
        trail.Clear();
        trail.emitting = true;
        previousPos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void StopCutting()
    {
        isCutting = false;
        trail.emitting = false;
    }

    void UpdateCut()
    {
        Vector2 newPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPos;
        previousPos = newPos;
    }
}
