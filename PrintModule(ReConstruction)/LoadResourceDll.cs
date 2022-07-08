using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PrintModule_ReConstruction_
{
    internal static class LoadResourceDll
    {
        private static Dictionary<string, Assembly> Dlls = new Dictionary<string, Assembly>();
        private static Dictionary<string, object> Assemblies = new Dictionary<string, object>();

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly ass;
            string assName = new AssemblyName(args.Name).FullName;
            if (Dlls.TryGetValue(assName, out ass) && ass != null)
            {
                Dlls[assName] = null;
                return ass;
            }
            else
            {
                throw new DllNotFoundException(assName);
            }
        }

        /// <summary> 注册资源中的dll
        /// </summary>
        public static void RegistDLL()
        {
            Assembly ass = new StackTrace(0).GetFrame(1).GetMethod().Module.Assembly;
            if (Assemblies.ContainsKey(ass.FullName))
            {
                return;
            }
            Assemblies.Add(ass.FullName, null);
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            string[] res = ass.GetManifestResourceNames();
            foreach (string r in res)
            {
                if (r.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Stream s = ass.GetManifestResourceStream(r);
                        byte[] bts = new byte[s.Length];
                        s.Read(bts, 0, (int)s.Length);
                        Assembly da = Assembly.Load(bts);
                        if (Dlls.ContainsKey(da.FullName))
                        {
                            continue;
                        }
                        Dlls[da.FullName] = da;
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}