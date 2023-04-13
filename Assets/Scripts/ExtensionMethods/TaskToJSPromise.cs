using Microsoft.ClearScript;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class TaskToJSPromise
    {
        private delegate void Executor(dynamic resolve, dynamic reject);
        public static object ToPromise<T>(this Task<T> task)
        {
            var nPromise = (ScriptObject)ScriptEngine.Current.Script.Promise;
            return nPromise.Invoke(true, new Executor((resolve, reject) =>
            {
                task.ContinueWith(t =>
                {
                    if (t.IsCompleted)
                    {
                        resolve(t.Result);
                    }
                    else
                    {
                        reject(t.Exception);
                    }
                });
            }));
        }
        public static object ToPromise(this Task task)
        {
            var nPromise = (ScriptObject)ScriptEngine.Current.Script.Promise;
            return nPromise.Invoke(true, new Executor((resolve, reject) =>
            {
                task.ContinueWith(t =>
                {
                    if (t.IsCompleted)
                    {
                        resolve();
                    }
                    else
                    {
                        reject(t.Exception);
                    }
                });
            }));
        }
    }
}
