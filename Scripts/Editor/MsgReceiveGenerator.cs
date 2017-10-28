using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;



public static class MsgReceiveGenerator
{



    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        Generate();
    }

    struct MethId { public MethodInfo info; public ushort Msgid; }
    public static IMessageSwitcher Switcher;
    static void Generate()
    {
        //var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var assemblies = new Assembly[] { typeof(MsgId).Assembly };

        MethodInfo[] withMsg = null;
        List<Type> theirClasses = new List<Type>();
        List<MethId> methIds = new List<MethId>();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var types = assemblies[i].GetTypes();
            for (int j = 0; j < types.Length; j++)
            {
                var meth = types[j].GetMethods();
                for (int k = 0; k < meth.Length; k++)
                {
                    if (meth[k].GetCustomAttributes(typeof(MsgId), true).Length > 0)
                        theirClasses.Add(types[j]);
                }
            }
            withMsg = assemblies[i].GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(MsgId), true).Length > 0)
                      .ToArray();

            var msgs = withMsg.SelectMany(t => t.GetCustomAttributes(typeof(MsgId), true)).ToArray();
            for (int j = 0; j < msgs.Length; j++)
            {
                methIds.Add(new MethId() { info = withMsg[j], Msgid = ((MsgId)msgs[j]).id });
                //Utils.EditorLog(methIds[methIds.Count - 1].info.GetBaseDefinition() + ", " + methIds[methIds.Count - 1].Msgid);
            }


        }

        FindRetypeMethods();

        string code = @"public class IdSwitcher : IMessageSwitcher {


	public void SwitchMessage(int connectionId, byte[] msg){
        int ind = 0;
		var msgId = FromByteArrayExtensions.Ushort(msg, ref ind);
        Utils.EditorLog(msgId);
		switch(msgId){
			default:
				break;
                        ";

        for (int i = 0; i < methIds.Count; i++)
        {
            code += "case " + methIds[i].Msgid + ": {";
            var parameters = methIds[i].info.GetParameters();
            for (int j = 1; j < parameters.Length; j++)//First parameter must be INT CONNECTIONID
            {
                //Debug.Log(parameters[j].ParameterType);//TODO connect to byte array extension, hard af, maybe switch..it's at compile time anyway
                code += "var var" + j + " = " + GetRetypeMethod(parameters[j].ParameterType) + "; " + Environment.NewLine;
            }
            code += theirClasses[i] + "." + methIds[i].info.Name + "( "; //ještě jméno classy ty pablbe
            code += "connectionId" + (parameters.Length > 1 ? ", " : "");
            for (int j = 1; j < parameters.Length; j++)
            {
                code += "var" + j;
                if (j < parameters.Length - 1)
                    code += ", ";
            }
            code += " );" + Environment.NewLine;
            //code += " Utils.EditorLog(msgId.ToString());";
            code += "break; }" + Environment.NewLine;
        }

        code += @"		}
	}
}
                   ";
        //Utils.EditorLog(code);
        var assembly = StringCompiler.Compile(code);


        //test
        
        //Type fooType = assembly.GetType("IdSwitcher");
        //object foo = assembly.CreateInstance("IdSwitcher");
        //Debug.Log(fooType.AssemblyQualifiedName);
        //Switcher = (IMessageSwitcher)foo;

        //MethodInfo printMethod = fooType.GetMethod("SwitchMessage");
        //printMethod.Invoke(foo, BindingFlags.InvokeMethod, null, new object[] {10,(ushort)15,null }, System.Globalization.CultureInfo.CurrentCulture);

    }

    static Dictionary<Type, string> retypeMethods = new Dictionary<Type, string>();
    static string GetRetypeMethod(Type parType)
    {
        string mName;
        if (retypeMethods.TryGetValue(parType, out mName))
            return "FromByteArrayExtensions." + mName + "(msg, ref ind)";
        Utils.EditorLog(parType + " is not a sendable type. You must implement it in FromByteArrayExtensions.");
        return null;
    }

    static void FindRetypeMethods() {
        var info = typeof(FromByteArrayExtensions);
        var methods = info.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly);
        for (int i = 0; i < methods.Length; i++)
        {
            retypeMethods.Add(methods[i].ReturnType, methods[i].Name);
        }
    }
}

public static class StringCompiler
{


    public static Assembly Compile(string code)
    {
        CodeDomProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
        var cp = new CompilerParameters();
        cp.GenerateExecutable = false;
        cp.ReferencedAssemblies.Add("System.dll");
        cp.ReferencedAssemblies.Add("System.Core.dll");
        cp.ReferencedAssemblies.Add(typeof(MsgId).Assembly.Location);
        cp.OutputAssembly = "Assets/StreamingAssets/NetCodeGen.dll";
        var res = provider.CompileAssemblyFromSource(cp, code);
        for (int i = 0; i < res.Errors.Count; i++)
        {
            Debug.Log(res.Errors[i]);
        }
        return res.CompiledAssembly;
    }

}
