﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGun : MonoBehaviour
{
    public new Camera camera;

    public VoxelHandler voxelHandler;

    public float explosionSize = 5f;


    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Mine();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Build();
        }
    }

    protected void Mine()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 4f))
        {
            return;
        }

        Vector3 somewhereInBlock = hit.point + ray.direction.normalized * 0.01f;

        int x = Mathf.FloorToInt(somewhereInBlock.x);
        int y = Mathf.FloorToInt(somewhereInBlock.y);
        int z = Mathf.FloorToInt(somewhereInBlock.z);

        VoxelHandler.instance.SetBlock(x, y, z, Blocks.Air);
    }
    protected void Build()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 4f))
        {
            return;
        }

        Vector3 somewhereInBlock = hit.point - ray.direction.normalized * 0.01f;

        int x = Mathf.FloorToInt(somewhereInBlock.x);
        int y = Mathf.FloorToInt(somewhereInBlock.y);
        int z = Mathf.FloorToInt(somewhereInBlock.z);

        VoxelHandler.instance.SetBlock(x, y, z, Blocks.Wood);
    }
}
