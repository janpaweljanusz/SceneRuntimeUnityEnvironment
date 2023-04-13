using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class CubeSpawnerSystem : ComponentSystem
{



    protected override void OnUpdate()
    {
        //Entities.ForEach(ref Rotation rotation, ref Scale scale, 
        //// Rotate the cube entity
        //EntityManager.AddComponentData(cubeEntity, new Rotation
        //{
        //    Value = quaternion.RotateY(Time.DeltaTime)
        //});
    }
}