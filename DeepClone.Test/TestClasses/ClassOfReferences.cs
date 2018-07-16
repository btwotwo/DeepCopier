using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfReferences
    {
        private ClassOfValues _privateObject;
        public ClassOfValues FirstObjectField;
        public ClassOfValues SecondObjectField;
        
        public ClassOfValues FirstObjectProp { get; set; }
        public ClassOfValues SecondObjectProp { get; set; }

        public ClassOfValues PrivateFieldProp
        {
            get => _privateObject;
            set
            {
                value.PrivateFieldProp = 5;
                _privateObject = value;
            }
        }
    }
}
