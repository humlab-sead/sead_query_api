// SessionCollection.cs
using Xunit;

[CollectionDefinition("UsePostgresDockerSession")]
public class SessionCollection : ICollectionFixture<PostgresSessionFixture> { }
