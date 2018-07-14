using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassWithDictionaries
    {
        public Dictionary<int, int> ValueValue { get; set; }
        public Dictionary<int, ClassOfReferences> ValueReference { get; set; }
    }
}
