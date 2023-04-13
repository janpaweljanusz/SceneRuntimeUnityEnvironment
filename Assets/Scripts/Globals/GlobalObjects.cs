using Microsoft.ClearScript.V8;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Globals
{
    public class GlobalObjects
    {
        public static int EntityId;
        public static List<EngineRequest> MessagesFromJS = new List<EngineRequest>();
        public static List<EngineResponse> MessagesFromUnity = new List<EngineResponse>();
        public class EngineModule
        {
            
            public async Task<string[]> sendMessage(params object[] messages)
            {
                dynamic array = messages[0];
                var arrayRet = (IList)array;
                foreach (var message in arrayRet)
                {
                    try
                    {
                        EngineRequest requestBase = JsonUtility.FromJson<EngineRequest>(message.ToString());
                        requestBase.AddMessageToDictionry(message.ToString(),MessagesFromJS);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("parsing error");
                    }
                }

                return MessagesFromUnity.Select(message =>
                { 
                    var str = string.Empty;
                    try
                    {
                        str = JsonUtility.ToJson(message);
                    }
                    catch (Exception e) 
                    {
                        Debug.LogError(e.Message);
                    }
                    return str;
                    }).ToArray();
                //return messagesFromUnity.ToArray();
            }
            
        }
        /// <summary>
        /// Brings console.log functionality to JS contect
        /// </summary>
        public class Console
        {
            public void log(string text)
            {
                Debug.Log(text);
                //System.Console.WriteLine(text);
            }
            public void log(object text)
            {
                Debug.Log(text);
                //System.Console.WriteLine(text);
            }
        }

        private EngineModule _engineModuleInstance;
        /// <summary>
        /// Constructor handles adding global objects and methods to a JS engine instance
        /// </summary>
        /// <param name="engine"></param>
        public GlobalObjects(V8ScriptEngine engine)
        {
            _engineModuleInstance = new EngineModule();
            engine.Script.console = new GlobalObjects.Console();
            engine.Script.require = (Func<string, EngineModule>)require;
        }
        /// <summary>
        /// This method is used to bring EngineModule global instance to JS scene context.
        /// </summary>
        /// <param name="moduleName">is a key for gettings certain module</param>
        /// <returns>EngineModule global object instance </returns>
        /// <exception cref="Exception"></exception>
        private EngineModule require(string moduleName)
        {
            if (moduleName != "~engine")
            {
                throw new Exception("Unknown module");
            }

            return _engineModuleInstance;
        }

    }
}
