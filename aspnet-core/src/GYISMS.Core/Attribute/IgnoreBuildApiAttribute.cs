using System;

namespace GYISMS.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class IgnoreBuildApiAttribute : System.Attribute
    {
        public IgnoreBuildApiAttribute() { }
    }
}
