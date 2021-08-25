using T4TextTransformationProxy;

namespace MyT4Assembly
{
    public static class CodeGeneratorHelper
    {
        public static void GenerateCode(object context)
        {
            var tt = TextTransformationProxy.GetProxy(context);
            
            tt.WriteLine("class TestClass");
            tt.WriteLine("{");
            tt.PushIndent("    ");
            tt.WriteLine("///<summary>Some sommary</summary>");
            tt.WriteLine("public string Str1 {get; set; }");
            tt.PopIndent();
            tt.WriteLine("}");
        }
    }
}