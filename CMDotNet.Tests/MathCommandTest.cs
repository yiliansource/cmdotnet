using NUnit.Framework;

namespace YSource.Commands.UnitTests
{
    [TestFixture]
    public class MathCommandTest
    {
        private readonly ConsoleCommandHost _host;

        public MathCommandTest()
        {
            _host = new ConsoleCommandHost();
            _host.Initialize();
        }

        [Test]
        public void CommandParseValidity()
        {
            Assert.DoesNotThrow(() => _host.HandleCommand("math 1 5"));
        }
    }
}