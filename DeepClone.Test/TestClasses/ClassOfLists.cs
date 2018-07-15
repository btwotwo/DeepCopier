using System.Collections.Generic;

namespace DeepClone.Test.TestClasses
{
    public class ClassOfLists
    {
        public List<int> ListOfValues { get; set; }
        public List<ClassOfReferences> ListOfReferenceses { get; set; }

        public int[] ArrayOfValues { get; set; }
        public ClassOfReferences[] ArrayOfReferenceses { get; set; }
    }
}

