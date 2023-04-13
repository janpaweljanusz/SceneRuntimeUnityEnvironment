using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Globals
{
    [Serializable]
    public class Transform
    {
        public float[] position = new float[3];
        public float[] rotation = new float[4];
        public float[] scale = new float[3];
    }
    public abstract class EngineEntity 
    {
        public abstract void ExecuteEntityOperation(Entity cubePrefab, EntityManager entityManager);
    }
    [Serializable]
    public class EngineEntityAdd : EngineEntity
    {
        public int id;

        public override void ExecuteEntityOperation(Entity cubePrefab, EntityManager entityManager)
        {
            if (SpawnCubeSystem._cubes.Count > 0)
            {
                return;
            }
            var cube = entityManager.Instantiate(cubePrefab);
            entityManager.AddComponentData(cube, new CubeId() { Value = this.id });
            SpawnCubeSystem._cubes.Add(this.id, cube);
        }
    }
    [Serializable]
    public class EngineEntityTransformSet : EngineEntity
    {
        public int entityId;
        public Transform transform;

        public override void ExecuteEntityOperation(Entity cubePrefab, EntityManager entityManager)
        {

            if (SpawnCubeSystem._cubes.TryGetValue(entityId, out var cube))
            {
                entityManager.SetComponentData(cube, new LocalToWorld()
                {
                    Value = Matrix4x4Utility.Matrix4X4FromJson(this.transform)
                });
            }
        }
    }
    public class EngineRequest
    {
        public string method;
        public void AddMessageToDictionry(string fullMessage, List<EngineRequest> messagesFromJS)
        {
            if (method == "entity_add")
            {
                EngineRequest<EngineEntityAdd> messageFromJS = JsonUtility.FromJson<EngineRequest<EngineEntityAdd>>(fullMessage);
                // I could call it right here or store in messagesfromJS. Storing would give more possibilities, ie interpolation, extrapolation
                // I assume that JS update might be less frequent
                messagesFromJS.Add(messageFromJS);
            }
            else if (method == "entity_transform_update")
            {
                // I could call it right here or store in messagesfromJS. Storing would give more possibilities, ie interpolation, extrapolation
                // I assume that JS update might be less frequent
                EngineRequest<EngineEntityTransformSet> messageFromJS =
                    JsonUtility.FromJson<EngineRequest<EngineEntityTransformSet>>(fullMessage);
                messagesFromJS.Add(messageFromJS);
            }
            else
            {
                Debug.Log("unknown message type");
            }
        }
        public virtual void ExecuteEntityOperation(Entity cubePrefab, EntityManager entityManager)
        {
        }
    }
    public class EngineRequest<T> : EngineRequest where T : EngineEntity
    {
        public T data;
        public override void ExecuteEntityOperation(Entity cubePrefab, EntityManager entityManager)
        {
            data.ExecuteEntityOperation(cubePrefab, entityManager);
        }
    }
}