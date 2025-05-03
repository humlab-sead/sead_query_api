// SessionCollection.cs
using Xunit;

[CollectionDefinition("Postgres Docker Session")]
public class SessionCollection : ICollectionFixture<PostgresSessionFixture> { }
