using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassWithConstructor
    {
        public ClassOfReferences Nested { get; set; }
        public ClassWithConstructor(int someval)
        {
            Nested = new ClassOfReferences {FirstObjectProp = new ClassOfValues() {IntProp = someval}};
        }
    }

    public class ClassWIthConstructorHolder
    {
        public ClassWithConstructor Constructor { get; set; }
    }
}
