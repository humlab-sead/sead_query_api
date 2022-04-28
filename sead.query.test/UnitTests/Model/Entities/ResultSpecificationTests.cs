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
    public class ResultSpecificationTests
    {
        private ResultSpecification CreateResultSpecification()
        {
            return new ResultSpecification()
            {
                SpecificationId = 3,
                SpecificationKey = "sample_group_level",
                Fields = new List<ResultSpecificationField>() {
                    new ResultSpecificationField() {
                        SpecificationFieldId = 1,
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

                    new ResultSpecificationField() {
                        SpecificationFieldId = 2,
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
        //}

        [Fact]
        public void GetFields_WithTwoFieldsField_ReturnsFields()
        {
            // Arrange
            var item = CreateResultSpecification();

            // Act
            var result = item.GetSortedFields();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetResultFields_WithTwoFields_ReturnsTwoResultFields()
        {
            // Arrange
            var item = CreateResultSpecification();

            // Act
            var result = item.GetResultFields();

            // Assert
            Assert.Single(result);
        }
    }
}
