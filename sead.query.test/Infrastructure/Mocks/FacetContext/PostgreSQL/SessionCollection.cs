// SessionCollection.cs
using Xunit;

namespace SQT.Infrastructure;

[CollectionDefinition("UsePostgresFixture")]
public class UsePostgresFixtureCollection : ICollectionFixture<SmartPostgresFixture> { }


// [CollectionDefinition("UseSmartPostgresFixture")]
// public class UseSmartPostgresFixtureCollection : ICollectionFixture<SmartPostgresFixture> { }


// [CollectionDefinition("UseImprovedPostgresFixture")]
// public class SessionCollection : ICollectionFixture<ImprovedPostgresFixture> { }
