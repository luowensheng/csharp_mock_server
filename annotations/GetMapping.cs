using System;


namespace Annotation {

    [AttributeUsage(AttributeTargets.Method)]
    public class GetMappingAttribute : Attribute
    {
        public string value { get; set; }
    }

}