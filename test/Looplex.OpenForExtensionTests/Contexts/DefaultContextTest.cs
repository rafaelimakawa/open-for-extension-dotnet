using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Abstractions.Contexts;
using Looplex.OpenForExtension.Abstractions.Plugins;
using Looplex.OpenForExtension.Contexts;
using Looplex.OpenForExtension.Plugins;
using Looplex.OpenForExtensionTests.Commands;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Looplex.OpenForExtension.Abstractions.ExtensionMethods;
using Looplex.OpenForExtensionTests.Plugins;

namespace Looplex.OpenForExtensionTests.Contexts
{
    [TestClass]
    public class DefaultContextTest
    {
        private DefaultContext? _context;

        /// <summary>
        /// checks if rollback is applied correctly in reverse order of called plugins
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DefaultContext_RollbackReverseOrder()
        {
            //Prepare
            CancellationToken ct = CancellationToken.None;
            string executed1 = "Executed 1 ";
            string executed2 = "Executed 2 ";
            string executed3 = "Executed 3 ";
            string rollback1 = "RollBack 1 ";
            string rollback2 = "RollBack 2 ";
            string rollback3 = "RollBack 3 ";

            IPlugin firstPlugin = new TestPlugin1();
            IPlugin secondPlugin = new TestPlugin2();
            IPlugin thirdPlugin = new TestPlugin3();

            List<IPlugin> plugins = new List<IPlugin> { firstPlugin, secondPlugin, thirdPlugin };
            _context = (DefaultContext)DefaultContext.New(plugins);

            _context.State.testString = "";
            await _context.Plugins.ExecuteAsync<ITestCommand1>(_context, ct);
            await _context.Plugins.ExecuteAsync<ITestCommand2>(_context, ct);
            await _context.Plugins.ExecuteAsync<ITestCommand3>(_context, ct);

            await _context.DoRollBack();

            Assert.AreEqual(_context.State.testString, executed1 + executed2 + executed3 + rollback3 + rollback2 + rollback1);

        }
        /// <summary>
        /// Test simple log for plugin rollback and plugin rollback error
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DefaultContext_TestRollbackLogs()
        {
            //Prepare
            CancellationToken ct = CancellationToken.None;

            IPlugin firstPlugin = new TestPlugin1();
            IPlugin secondPlugin = new TestPlugin2();
            IPlugin thirdPlugin = new TestPlugin3RBFail();

            List<IPlugin> plugins = new List<IPlugin> { firstPlugin, secondPlugin, thirdPlugin };
            _context = (DefaultContext)DefaultContext.New(plugins);

            await _context.Plugins.ExecuteAsync<ITestCommand1>(_context, ct);
            await _context.Plugins.ExecuteAsync<ITestCommand2>(_context, ct);
            await _context.Plugins.ExecuteAsync<ITestCommand3>(_context, ct);

            string allRollbacksDone = "";
            Func<IContext, Task> logAction = (IContext ctx) =>
            {
                string rbName = ctx.State.LogMessage;
                allRollbacksDone += rbName;

                return Task.CompletedTask;
            };
            try
            {
                await _context.DoRollBack(logAction);
            }
            catch
            {
            }

            Assert.AreEqual(allRollbacksDone, $"Error when trying to rollback plugin {thirdPlugin.Name} Exeption: Rollback failed" + secondPlugin.Name + firstPlugin.Name);
        }

        /// <summary>
        /// plugin rollback error must raise exception
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DefaultContext_TestRollbackErrorMustRaiseException()
        {
            //Prepare
            CancellationToken ct = CancellationToken.None;

            IPlugin thirdPlugin = new TestPlugin3RBFail();

            List<IPlugin> plugins = new List<IPlugin> { thirdPlugin };
            _context = (DefaultContext)DefaultContext.New(plugins);

            await _context.Plugins.ExecuteAsync<ITestCommand3>(_context, ct);

            Assert.ThrowsAsync<AggregateException>(async () => await _context.DoRollBack());
        }
    }
}
