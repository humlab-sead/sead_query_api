// SessionCollection.cs
using Xunit;

[CollectionDefinition("Postgres Session")]
public class SessionCollection : ICollectionFixture<PostgresSessionFixture> { }
