using Looplex.OpenForExtension.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Looplex.OpenForExtensionTests.Commands
{
    public class TestCommand1 : ITestCommand1
    {
        public Task ExecuteAsync(IContext context, CancellationToken cancellationToken)
        {
            if(!((IDictionary<string, object>)context.State).ContainsKey("testString")) context.State.testString = string.Empty;
            context.State.testString += "Executed 1 ";
            return Task.CompletedTask;
        }

        public  Task ExecuteRollBackAsync(IContext context)
        {
            context.State.testString += "RollBack 1 ";
            return Task.CompletedTask;
        }
    }
}
