using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Annotation;

namespace Server
{
    using EndpointsMapping = Dictionary<string, ReturnNone>;
    using ControllerMapping = Dictionary<string, Dictionary<string, ReturnNone> >;
    delegate object ReturnNone();
    class MockServer
    {
        public static void Run(int port)
        {
            Console.WriteLine("Initializing server");
            string url = $"http://localhost:{port}";
            var map = getAllMapping();
            Console.WriteLine($"Running server at {url}");
            foreach(var (name, obj) in map)
                foreach(var (path, func) in obj){
                    System.Console.WriteLine($"[name:{name}, path:{url}{path}, result:{func.Invoke()}]");
                }
        }

        private static ControllerMapping getAllMapping(){

            var map = new ControllerMapping();
            
            var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetCustomAttributes<ControllerAttribute>().Count() > 0
                        select t;
                        
            foreach (var t in types){

                 map.Add(t.Name, new EndpointsMapping());

                 foreach (var m in t.GetMethods())
                    if (m.GetCustomAttributes<GetMappingAttribute>().Count() > 0)
                    {
                        System.Console.WriteLine(t.Name);
                        var attr = m.GetCustomAttribute<GetMappingAttribute>();
                        // System.Console.WriteLine($"{m.Name}, {m.ReflectedType}, {m.ReturnType}, {m.GetParameters().Length}]"); //, m.GetParameters());
                        // foreach(var pr in m.GetParameters()){
                        //     System.Console.WriteLine($"{pr} {pr.Name}, {pr.ParameterType} {pr.Position}");
                        // }
                        var obj = Activator.CreateInstance(m.DeclaringType); // Instantiate the class
                        var parameters = m.GetParameters();
                        var n = parameters.Count();
                        var ps = new object[n];
                        for(int i=0; i<n; i++){
                            var cat = parameters[i].GetCustomAttribute<RequestParam>();
                            ps[i] = (cat!=null)? cat.name: "text";
                        }

                        map[t.Name].Add(attr.value, ()=>m.Invoke(obj, ps));
                    }
            }
            return map;
        
    }    
    }
}
