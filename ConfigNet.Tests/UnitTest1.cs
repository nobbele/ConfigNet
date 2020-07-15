using ConfigNet.Stores;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xunit;

namespace ConfigNet.Tests
{
    public class ComplexType
    {
        public int Value { get; set; }
        public string StringValue { get; set; }
    }
    public interface Config1
    {
        int Value { get; set; }
        string StringValue { get; set; }
        ComplexType ComplexValue { get; set; }
        [DefaultValue(5)] int DefaultValue { get; }
    }
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            var config = new ConfigurationBuilder<Config1>()
                .AddStore(new MemoryStore())
                .AddStore(new JsonStore("testconfig.json"))
                .Build();

            config.Value = 7;
            config.StringValue = "seven";
            config.ComplexValue = new ComplexType()
            {
                Value = 6,
                StringValue = "six",
            };
            Assert.Equal(7, config.Value);
            Assert.Equal("seven", config.StringValue);
            Assert.Equal(6, config.ComplexValue.Value);
            Assert.Equal("six", config.ComplexValue.StringValue);
            Assert.Equal(5, config.DefaultValue);
        }
    }
}
