using System.Collections.Generic;
using DeepClone.Test.TestClasses;
using Xunit;

namespace DeepClone.Test
{
    public class Test
    {
        [Fact]
        public void CopyFrom_ValueFields_Clones()
        {
            var valueClass = new ClassOfValues {BoolField = true, IntField = 123, LongField = 123456812312331231};

            var newValueClass = new ClassOfValues().CopyFrom(valueClass);

            Assert.True(newValueClass.BoolField);
            Assert.Equal(123, newValueClass.IntField);
            Assert.Equal(123456812312331231, newValueClass.LongField);
        }

        [Fact]
        public void CopyFrom_ValueProps_Clones()
        {
            var valueClass = new ClassOfValues() {BoolProp = true, IntProp = 123, LongProp = 99999999999999999};
            var newValueClass = new ClassOfValues().CopyFrom(valueClass);

            Assert.True(newValueClass.BoolProp);
            Assert.Equal(123, newValueClass.IntProp);
            Assert.Equal(99999999999999999, newValueClass.LongProp);
        }

        [Fact]
        public void CopyFrom_ReferencesFields_Clones()
        {
            var first = new ClassOfValues {BoolField = true, IntField = 123, LongField = 123456812312331231};
            var second = new ClassOfValues() {BoolField = false, IntField = 1, LongField = 333};
            var nestedClass = new ClassOfReferences() {FirstObjectField = first, SecondObjectField = second};

            var newNestedClass = new ClassOfReferences().CopyFrom(nestedClass);

            Assert.True(newNestedClass.FirstObjectField.BoolField);
            Assert.False(newNestedClass.SecondObjectField.BoolField);

            Assert.Equal(123, newNestedClass.FirstObjectField.IntField);
            Assert.Equal(1, newNestedClass.SecondObjectField.IntField);

            Assert.Equal(333, newNestedClass.SecondObjectField.LongField);
            Assert.Equal(123456812312331231, newNestedClass.FirstObjectField.LongField);

        }

        [Fact]
        public void CopyFrom_ReferencesProps_Clones()
        {
            var first = new ClassOfValues { BoolField = true, IntField = 123, LongField = 123456812312331231 };
            var second = new ClassOfValues() { BoolProp = false, IntProp= 1, LongProp = 333 };
            var nestedClass = new ClassOfReferences() { FirstObjectProp = first, SecondObjectProp = second };

            var newNestedClass = new ClassOfReferences().CopyFrom(nestedClass);

            Assert.True(newNestedClass.FirstObjectProp.BoolField);
            Assert.False(newNestedClass.SecondObjectProp.BoolField);

            Assert.Equal(123, newNestedClass.FirstObjectProp.IntField);
            Assert.Equal(1, newNestedClass.SecondObjectProp.IntProp);

            Assert.Equal(333, newNestedClass.SecondObjectProp.LongProp);
            Assert.Equal(123456812312331231, newNestedClass.FirstObjectProp.LongField);

        }

        [Fact]
        public void CopyFrom_ReferenceValues_CanNotModifyOriginal()
        {
            var values = new ClassOfValues() {BoolProp = true, BoolField = false};
            var nested = new ClassOfReferences() {FirstObjectProp = values};
            var clone = new ClassOfReferences().CopyFrom(nested);

            nested.FirstObjectProp.BoolProp = false;
            nested.FirstObjectProp.BoolField = true;

            Assert.True(clone.FirstObjectProp.BoolProp);
            Assert.False(clone.FirstObjectProp.BoolField);
        }

        [Fact]
        public void CopyFrom_Constructor_HandlesCorrectly()
        {
            var constructorHolder = new ClassWIthConstructorHolder()
            {
                Constructor = new ClassWithConstructor(12)
            };
            var copy = constructorHolder.CopyFrom(constructorHolder);
            Assert.Equal(12, copy.Constructor.Nested.FirstObjectProp.IntProp);
        }

        [Fact]
        public void CopyFrom_Dictionaries_Clones()
        {
            var classOfDictionaries = new ClassWithDictionaries()
            {
                ValueValue = new Dictionary<int, int>() {{10, 1}, {11, 2}},
                ValueReference = new Dictionary<int, ClassOfReferences>()
                {
                    {1, new ClassOfReferences() {FirstObjectProp = new ClassOfValues() {BoolProp = true}}}
                }
            };
            var clone = new ClassWithDictionaries().CopyFrom(classOfDictionaries);
            
            Assert.Equal(1, clone.ValueValue[10]);
            Assert.Equal(2, clone.ValueValue[11]);
            
            Assert.True(clone.ValueReference[1].FirstObjectProp.BoolProp);
        }

        [Fact]
        public void CopyFrom_Dictionaries_CanNotModifyOriginalValue()
        {
            var classOfDictionaries = new ClassWithDictionaries()
            {
                ValueReference = new Dictionary<int, ClassOfReferences>()
                {
                    {1, new ClassOfReferences() {FirstObjectField = new ClassOfValues() {IntProp = 123}}}
                }
            };

            var clone = new ClassWithDictionaries().CopyFrom(classOfDictionaries);

            classOfDictionaries.ValueReference[1].FirstObjectField.IntProp = 123456;

            Assert.Equal(123, clone.ValueReference[1].FirstObjectField.IntProp);
        }
    }
}
