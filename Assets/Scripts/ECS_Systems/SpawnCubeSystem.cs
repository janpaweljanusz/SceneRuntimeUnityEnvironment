using ExtensionMethods;
using Globals;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;
using Utils;

public partial class SpawnCubeSystem : SystemBase
{
    private Entity _cubePrefab;
    private Entity _cubeSpawner;
    // to protect from instancing more then one object with the same id
    public static Dictionary<int, Entity> _cubes = new Dictionary<int, Entity>();
    JSAsyncToTask jsAsyncToTask;
    V8ScriptEngine engine;
    protected override void OnCreate()
    {
        engine = new V8ScriptEngine();

        engine.AddHostType("Console", typeof(Console));
        engine.AddHostType(typeof(TaskToJSPromise));
        engine.AddHostType(typeof(Task));
        // instantiating global objects (c#/js bridge)
        var globalObjects = new Globals.GlobalObjects(engine);
        var moduleInstance = new Globals.Module(engine);


        engine.Execute(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "js/sandboxedRuntime.js")));
        engine.Execute(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "js/sandboxedRuntimeWrapper.js")));

        jsAsyncToTask = new JSAsyncToTask();
        jsAsyncToTask.ToTask(engine.Script.onStartWrapper);
    }
    protected override void OnStartRunning()
    {
        Debug.Log("Started");
        Application.targetFrameRate = 30;
        _cubePrefab = GetSingleton<CubePrefab>().Value;

    }
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalObjects.MessagesFromUnity.Add(new EngineResponse("key_down"));
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GlobalObjects.MessagesFromUnity.Add(new EngineResponse("key_up"));
        }

        jsAsyncToTask.ToTask(engine.Script.onUpdateWrapper, deltaTime); // just update method - it can be called everyframe or can wait
        GlobalObjects.MessagesFromUnity.Clear();

        foreach (EngineRequest message in GlobalObjects.MessagesFromJS)
        {
            message.ExecuteEntityOperation(_cubePrefab, EntityManager);
        }
        GlobalObjects.MessagesFromJS.Clear();
    }

}
