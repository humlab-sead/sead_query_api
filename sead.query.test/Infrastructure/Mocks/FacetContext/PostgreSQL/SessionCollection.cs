// SessionCollection.cs
using Xunit;

namespace SQT.Infrastructure;

[CollectionDefinition("UsePostgresDockerSession")]
public class UsePostgresFixtureCollection : ICollectionFixture<PostgresFixture> { }


[CollectionDefinition("UseSmartPostgresFixture")]
public class UseSmartPostgresFixtureCollection  : ICollectionFixture<SmartPostgresFixture> { }


[CollectionDefinition("UseImprovedPostgresFixture")]
public class SessionCollection : ICollectionFixture<ImprovedPostgresFixture> { }
