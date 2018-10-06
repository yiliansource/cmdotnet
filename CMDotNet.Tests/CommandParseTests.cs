using System;
using System.Reflection;

using NUnit.Framework;

namespace CMDotNet.UnitTests
{
    [TestFixture]
    public class CommandParseTests
    {
        private readonly CommandService _service;
        private const string COMMAND_PREFIX = "";

        public CommandParseTests()
        {
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetExecutingAssembly());
        }

        private void HandleCommand(CommandMessage msg)
        {
            int argPos = 0;
            if (!msg.HasStringPrefix(COMMAND_PREFIX, ref argPos))
                return;

            IExecutionResult result = _service.Execute(msg, argPos);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"{ result.Error }: { result.ErrorReason }");
                Assert.Fail();
            }
        }

        private void TestCommand(string command)
        {
            CommandMessage msg = new CommandMessage(command);
            HandleCommand(msg);
        }
       
        [Test]
        public void NoParameters()
            => TestCommand("ping");

        [Test]
        public void DynamicInteger()
            => TestCommand("square 5");

        [Test]
        public void RemainderString()
            => TestCommand("write This is a long string!");

        [Test]
        public void CustomType()
            => TestCommand("mynum 42");
    }
}