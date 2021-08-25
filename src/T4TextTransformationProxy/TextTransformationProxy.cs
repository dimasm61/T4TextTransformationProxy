using System;
using System.Linq;
using System.Reflection;

namespace T4TextTransformationProxy
{
    public class TextTransformationProxy
    {
        private readonly object _ttContext;
        private readonly MethodInfo _miWrite;
        private readonly MethodInfo _miWriteLine;
        private readonly MethodInfo _miPushIndent;
        private readonly MethodInfo _miPopIndent;

        public TextTransformationProxy(object ttContext)
        {
            _ttContext = ttContext;
            var contextType = ttContext.GetType();

            if (!contextType.ToString().EndsWith(".GeneratedTextTransformation"))
                throw new Exception($"Possible the wrong context type - '{contextType}'. It should be '...GeneratedTextTransformation'");

            var methods = contextType.GetMethods()
                .Where(c => !c.ToString().Contains("System.Object[]"));

            _miWrite = methods.FirstOrDefault(c => c.Name == "Write");

            _miWriteLine = methods.FirstOrDefault(c => c.Name == "WriteLine");

            _miPopIndent = contextType.GetMethod("PopIndent");

            _miPushIndent = contextType.GetMethod("PushIndent");
            
            if(_miWrite == null) throw new Exception($"In type '{contextType}' can't find 'Write(string str)' method");
            if(_miWriteLine == null) throw new Exception($"In type '{contextType}' can't find 'WriteLine(string str)' method");
            if(_miPopIndent == null) throw new Exception($"In type '{contextType}' can't find 'PopIndent()' method");
            if(_miPushIndent == null) throw new Exception($"In type '{contextType}' can't find 'PushIndent(string str)' method");
        }

        public static TextTransformationProxy GetProxy(object ttContext)
        {
            if (ttContext is TextTransformationProxy proxy)
                return proxy;

            return new TextTransformationProxy(ttContext);
        }

        public void Write(string str) => _miWrite.Invoke(_ttContext, new object[] { str });
        
        public void WriteLine(string str) => _miWriteLine.Invoke(_ttContext, new object[] { str });
        
        public void PushIndent(string indent) => _miPushIndent.Invoke(_ttContext, new object[] { indent });
        
        public void PopIndent() => _miPopIndent.Invoke(_ttContext, null);
    }
}