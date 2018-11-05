﻿using DynamicData.Tests.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace DynamicData.Tests.Cache
{
    
    public class TransformManySimpleFixture: IDisposable
    {
        private readonly ISourceCache<PersonWithChildren, string> _source;
        private readonly ChangeSetAggregator<Person, string> _results;

        public  TransformManySimpleFixture()
        {
            _source = new SourceCache<PersonWithChildren, string>(p => p.Key);

            _results = _source.Connect().TransformMany(p => p.Relations, p => p.Name)
                .AsAggregator();
        }

        public void Dispose()
        {
            _source.Dispose();
            _results.Dispose();
        }

        [Fact]
        public void Adds()
        {
            var parent = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1),
                new Person("Child2", 2),
                new Person("Child3", 3)
            });
            _source.AddOrUpdate(parent);
            _results.Data.Count.Should().Be(3, "Should be 4 in the cache");

            _results.Data.Lookup("Child1").HasValue.Should().BeTrue();
            _results.Data.Lookup("Child2").HasValue.Should().BeTrue();
            _results.Data.Lookup("Child3").HasValue.Should().BeTrue();
        }


        [Fact]
        public void Remove()
        {
            var parent = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child2", 2), new Person("Child3", 3)
            });
            _source.AddOrUpdate(parent);
            _source.Remove(parent);
            _results.Data.Count.Should().Be(0, "Should be 4 in the cache");
        }

        [Fact]
        public void RemovewithIncompleteChildren()
        {
            var parent1 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child2", 2), new Person("Child3", 3)
            });
            _source.AddOrUpdate(parent1);

            var parent2 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child3", 3)
            });
            _source.Remove(parent2);
            _results.Data.Count.Should().Be(0, "Should be 0 in the cache");
        }

        [Fact]
        public void UpdateWithLessChildren()
        {
            var parent1 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child2", 2), new Person("Child3", 3)
            });
            _source.AddOrUpdate(parent1);

            var parent2 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child3", 3),
            });
            _source.AddOrUpdate(parent2);
            _results.Data.Count.Should().Be(2, "Should be 2 in the cache");
            _results.Data.Lookup("Child1").HasValue.Should().BeTrue();
            _results.Data.Lookup("Child3").HasValue.Should().BeTrue();
        }


        [Fact]
        public void UpdateWithMultipleChanges()
        {
            var parent1 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child2", 2), new Person("Child3", 3)
            });
            _source.AddOrUpdate(parent1);

            var parent2 = new PersonWithChildren("parent", 50, new Person[]
            {
                new Person("Child1", 1), new Person("Child3", 3), new Person("Child5", 3),
            });
            _source.AddOrUpdate(parent2);
            _results.Data.Count.Should().Be(3, "Should be 2 in the cache");
            _results.Data.Lookup("Child1").HasValue.Should().BeTrue();
            _results.Data.Lookup("Child3").HasValue.Should().BeTrue();
            _results.Data.Lookup("Child5").HasValue.Should().BeTrue();
        }

    }
}