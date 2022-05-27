using System;

namespace Annotation {

   [AttributeUsage(AttributeTargets.Parameter)]
    public class RequestParam : Attribute {
        public string name = "Part of me follow";
    }
}