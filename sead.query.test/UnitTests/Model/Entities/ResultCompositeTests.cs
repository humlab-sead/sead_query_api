using Moq;
using SeadQueryCore;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.Model
{
    public class ResultCompositeTests
    {
        private ResultComposite CreateResultAggregate()
        {
            return new ResultComposite() {
                CompositeId = 3,
                CompositeKey = "sample_group_level",
                Fields = new List<ResultCompositeField>() {
                    new ResultCompositeField() {
                        CompositeFieldId = 1,
                        FieldTypeId = "count_item",
                        SequenceId = 1,
                        ResultField = new ResultField() {
                            ResultFieldKey = "xxx",
                            FieldTypeId = "single_item"
                        },
                        FieldType = new ResultFieldType() {
                            IsResultValue = true
                        }
                    },

                    new ResultCompositeField() {
                        CompositeFieldId = 2,
                        FieldTypeId = "count_item",
                        SequenceId = 2,
                        ResultField = new ResultField() {
                            ResultFieldKey = "yyy",
                            FieldTypeId = "single_item"
                        },
                        FieldType = new ResultFieldType() {
                            IsResultValue = false
                        }
                    }
                }
            };
        }

        //private ResultAggregate CreateMockedResultAggregate()
        //{
        //    var field1 = new Mock<ResultAggregateField>();
        //    field1.Setup(o => o.FieldType.IsResultValue).Returns(false);
        //    field1.Setup(o => o.AggregateFieldId).Returns(1);

        //    var field2 = new Mock<ResultAggregateField>();
        //    field2.Setup(o => o.FieldType.IsResultValue).Returns(true);
        //    field1.Setup(o => o.AggregateFieldId).Returns(2);

        //    var fields = (new List<ResultAggregateField>() { field1.Object, field2.Object });
        //    var mockResultAggregate = new Mock<ResultAggregate>();
        //    mockResultAggregate.Setup(x => x.Fields).Returns<ICollection<ResultAggregateField>>(_ => fields);

        //    return mockResultAggregate.Object;
        //}

        [Fact]
        public void GetFields_WithTwoFieldsField_ReturnsFields()
        {
            // Arrange
            var item = CreateResultAggregate();

            // Act
            var result = item.GetSortedFields();

            // Assert
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public void GetResultFields_WithTwoFields_ReturnsTwoResultFields()
        {
            // Arrange
            var item = CreateResultAggregate();

            // Act
            var result = item.GetResultFields();

            // Assert
            Assert.Single(result);
        }
    }
}
