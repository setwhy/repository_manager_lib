using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryManagerLib;
using Xunit;

namespace RepositoryManagerTests
{
    public class RepositoryManagerTests
    {
        private RepositoryManager CreateInitializedRepo()
        {
            var repo = new RepositoryManager();
            repo.Initialize();
            return repo;
        }

        [Fact]
        public void Register_ValidJson_ShouldSucceed()
        {
            var repo = CreateInitializedRepo();

            repo.Register("jsonItem", "{ \"id\": 1 }", (int)ItemType.Json);

            Assert.Equal("{ \"id\": 1 }", repo.Retrieve("jsonItem"));
            Assert.Equal((int)ItemType.Json, repo.GetType("jsonItem"));
        }

        [Fact]
        public void Register_ValidXml_ShouldSucceed()
        {
            var repo = CreateInitializedRepo();

            repo.Register("xmlItem", "<root><id>1</id></root>", (int)ItemType.Xml);

            Assert.Equal("<root><id>1</id></root>", repo.Retrieve("xmlItem"));
            Assert.Equal((int)ItemType.Xml, repo.GetType("xmlItem"));
        }

        [Fact]
        public void Register_InvalidJson_ShouldThrow()
        {
            var repo = CreateInitializedRepo();

            Assert.Throws<ArgumentException>(() =>
                repo.Register("badJson", "{ invalid json }", (int)ItemType.Json));
        }

        [Fact]
        public void Register_InvalidXml_ShouldThrow()
        {
            var repo = CreateInitializedRepo();

            Assert.Throws<ArgumentException>(() =>
                repo.Register("badXml", "<root><unclosed>", (int)ItemType.Xml));
        }

        [Fact]
        public void Register_Duplicate_ShouldThrow()
        {
            var repo = CreateInitializedRepo();
            repo.Register("dupItem", "{ \"id\": 1 }", (int)ItemType.Json);

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("dupItem", "{ \"id\": 2 }", (int)ItemType.Json));
        }

        [Fact]
        public void Retrieve_NonExisting_ShouldThrow()
        {
            var repo = CreateInitializedRepo();

            Assert.Throws<KeyNotFoundException>(() =>
                repo.Retrieve("missingItem"));
        }

        [Fact]
        public void GetType_NonExisting_ShouldThrow()
        {
            var repo = CreateInitializedRepo();

            Assert.Throws<KeyNotFoundException>(() =>
                repo.GetType("missingItem"));
        }

        [Fact]
        public void Deregister_Existing_ShouldSucceed()
        {
            var repo = CreateInitializedRepo();
            repo.Register("toRemove", "{ \"id\": 1 }", (int)ItemType.Json);

            repo.Deregister("toRemove");

            Assert.Throws<KeyNotFoundException>(() => repo.Retrieve("toRemove"));
        }

        [Fact]
        public void Deregister_NonExisting_ShouldThrow()
        {
            var repo = CreateInitializedRepo();

            Assert.Throws<KeyNotFoundException>(() =>
                repo.Deregister("notThere"));
        }

        [Fact]
        public void MustInitializeBeforeUse_ShouldThrow()
        {
            var repo = new RepositoryManager();

            Assert.Throws<InvalidOperationException>(() =>
                repo.Register("item", "{ }", (int)ItemType.Json));
        }

        [Fact]
        public void InitializeTwice_ShouldThrow()
        {
            var repo = new RepositoryManager();
            repo.Initialize();

            Assert.Throws<InvalidOperationException>(() => repo.Initialize());
        }

        [Fact]
        public void ThreadSafety_RegisterMultipleItems_ShouldWork()
        {
            var repo = CreateInitializedRepo();

            Parallel.For(0, 1000, i =>
            {
                var key = "item" + i;
                repo.Register(key, "{ \"id\": " + i + " }", (int)ItemType.Json);
            });

            Assert.Equal("{ \"id\": 500 }", repo.Retrieve("item500"));
        }
    }
}
