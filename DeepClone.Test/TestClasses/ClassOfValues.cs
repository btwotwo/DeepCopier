using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfValues
    {
        private int _privateField;

        public int IntField;
        public bool BoolField;
        public long LongField;

        public int IntProp { get; set; }
        public bool BoolProp { get; set; }
        public long LongProp { get; set; }

        public int PrivateFieldProp
        {
            get => _privateField;
            set => _privateField = value * 2;
        }

        public string StringProp { get; set; }


    }
}
