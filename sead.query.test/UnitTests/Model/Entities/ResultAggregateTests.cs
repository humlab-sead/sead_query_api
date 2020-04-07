using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultAggregateTests
    {
        private ResultAggregate CreateResultAggregate()
        {
            return new ResultAggregate() {
                AggregateId = 3,
                AggregateKey = "sample_group_level",
                Fields = new List<ResultAggregateField>() {
                    new ResultAggregateField() {
                        AggregateFieldId = 1,
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

                    new ResultAggregateField() {
                        AggregateFieldId = 2,
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
            var result = item.GetFields();

            // Assert
            Assert.Equal(2, result.Count);
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
