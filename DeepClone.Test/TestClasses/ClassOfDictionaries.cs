using System.Collections.Generic;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfDictionaries
    {
        public Dictionary<int, int> ValueValue { get; set; }
        public Dictionary<int, ClassOfReferences> ValueReference { get; set; }
    }
}
