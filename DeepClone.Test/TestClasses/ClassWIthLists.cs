using System;
using System.Collections.Generic;
using System.Text;

namespace DeepClone.Test.TestClasses
{
    public class ClassWIthLists
    {
        public List<int> ListOfValues { get; set; }
        public List<ClassOfReferences> ListOfReferenceses { get; set; }

        public int[] ArrayOfValues { get; set; }
        public ClassOfReferences[] ArrayOfReferenceses { get; set; }
    }
}
