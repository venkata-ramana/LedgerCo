using FluentAssertions;
using Ledger.Factories;
using Ledger.Processors;
using System;
using Xunit;

namespace Ledger.Test.Factories
{
    public class ProcessFactoryTest
    {
        [Fact]
        public void ShouldCreate_FileProcessor_As_DefaultProcessor()
        {
            var processor = ProcessorFactory.CreateProcessor(0, "input.txt");

            processor.Should().NotBeNull();
            processor.GetType().Should().Be(typeof(FileProcessor));
        }

        [Fact]
        public void ShouldCreate_FileProcessor_When_FileProcessor_TypeProvided()
        {
            var processor = ProcessorFactory.CreateProcessor(Constants.ProcessorType.FileProcessor, "input.txt");

            processor.Should().NotBeNull();
            processor.GetType().Should().Be(typeof(FileProcessor));
        }

        [Fact]
        public void Should_Throw_ArgumentException_WhenInvalidInputFileProvided()
        {
            Action action = () => ProcessorFactory.CreateProcessor(Constants.ProcessorType.FileProcessor, string.Empty);

            action.Should().Throw<ArgumentException>().WithMessage(Constants.ErrorMessages.FilePathError);
        }
    }
}
