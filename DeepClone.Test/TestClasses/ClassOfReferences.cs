using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfReferences
    {
        public ClassOfValues FirstObjectField;
        public ClassOfValues SecondObjectField;
        
        public ClassOfValues FirstObjectProp { get; set; }
        public ClassOfValues SecondObjectProp { get; set; }
    }
}
