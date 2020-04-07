using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTableTests
    {
        //public static List<object[]> TestData = new List<object[]>() {
        //    new object[] {
        //        typeof(FacetTable),
        //        3,
        //        new Dictionary<string, object>() {
        //            { "FacetTableId", 3 },
        //            { "FacetId", 3 },
        //            { "SequenceId", 1 },
        //            { "SchemaName", "" },
        //            { "TableName", "metainformation.tbl_denormalized_measured_values" },
        //            { "Alias", "" }
        //        }
        //    }
        //};

        //[Theory]
        //[MemberData(nameof(TestData))]
        //public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        //{
        //    // Arrange
        //    using (var context = ScaffoldUtility.DefaultFacetContext()) {
        //        // Act
        //        var entity = context.Find(type, new object[] { id });
        //        // Assert
        //        Assert.NotNull(entity);
        //        Asserter.EqualByDictionary(type, expected, entity);
        //    }
        //}
    }
}
