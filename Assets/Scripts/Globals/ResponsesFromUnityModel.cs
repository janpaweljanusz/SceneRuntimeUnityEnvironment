using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Globals
{
    [Serializable]
    public class EngineKeyPressed // consider changing to struct
    {
        public string key;
    }
    public class EngineResponse
    {
        public string method;
        public EngineKeyPressed data;
        public EngineResponse(string method)
        {
            this.method = method;
            this.data = new EngineKeyPressed()
            {
                key = "space"
            };
        }
    }
}