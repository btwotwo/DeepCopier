using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfValues
    {
        public int IntField;
        public bool BoolField;
        public long LongField;

        public int IntProp { get; set; }
        public bool BoolProp { get; set; }
        public long LongProp { get; set; }

        public string StringProp { get; set; }

        public int GetIntField()
        {
            return IntField;
        }
    }
}
