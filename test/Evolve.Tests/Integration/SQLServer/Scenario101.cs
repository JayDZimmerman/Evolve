﻿using System.Linq;
using EvolveDb.Configuration;
using EvolveDb.Tests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace EvolveDb.Tests.Integration.SQLServer
{
    [Collection("SQLServer collection")]
    public class Scenario101 : Scenario<SQLServerFixture>
    {
        public Scenario101(SQLServerFixture dbContainer, ITestOutputHelper output) : base(dbContainer, output) { }

        [Fact]
        [Category(Test.SQLServer, Test.Sceanario)]
        public void Scenario_sqlserver_tx_commit_all_only_when_every_scripts_succeed()
        {
            // Arrange
            Evolve.TransactionMode = TransactionKind.CommitAll;
            // Assert migration fails on last script, nothing applied
            Evolve.AssertMigrateThrows<EvolveException>(Cnn);
            Assert.True(Evolve.AppliedMigrations.Count == 0, $"There should be no migration applied when a migration fails in CommitAll mode.");
            Assert.False(MetadataTable.GetAllAppliedMigration().Any());
            Assert.False(MetadataTable.GetAllAppliedRepeatableMigration().Any());

            // Arrange
            Evolve.TargetVersion = new("2_0");
            // Assert migration succeeds, 2 scripts applied
            Evolve.AssertMigrateIsSuccessful(Cnn);
        }
    }
}
