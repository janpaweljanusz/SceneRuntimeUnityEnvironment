using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Globals
{
    public class Module
    {
        public dynamic exports = new ExpandoObject();
        public dynamic Require(string name)
        {
            var byName = (IDictionary<string, object>)exports;
            return byName[name];
        }
        public Module(V8ScriptEngine engine)
        {
            engine.Script.originalRequire = (Func<string, dynamic>)Require;
            engine.Script.module = this;
        }
    }
}
