
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class JSAsyncToTask
    {
        /// <summary>
        /// A method to call js async method, return a JS promise and convert it to C# task. Stored as a method instead of extension to keep dynamics. 
        /// ManualResetEventSlim is used to keep JS threat alive. 
        /// </summary>
        /// <param name="asyncJSFunction">js async functon</param>
        /// <param name="dtParameter">js function parameter (optional)</param>
        /// <returns>C# task</returns>
        public Task<dynamic> ToTask(dynamic asyncJSFunction, dynamic? dtParameter = null)
        {
            var done = new ManualResetEventSlim();
            dynamic promise = asyncJSFunction(done, dtParameter);
            var tcs = new TaskCompletionSource<dynamic>();
            Action<object> onResolved = value => tcs.SetResult(value);
            Action<dynamic> onRejected = reason => tcs.SetException(new Exception(reason.message));
            promise.then(onResolved, onRejected);
            done.Wait();
            return tcs.Task;
        }
    }
}