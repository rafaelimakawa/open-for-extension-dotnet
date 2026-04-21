using Looplex.OpenForExtension.Abstractions.Commands;
using Looplex.OpenForExtension.Plugins;
using Looplex.OpenForExtensionTests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Looplex.OpenForExtensionTests.Plugins
{
    public class TestPlugin3RBFail : AbstractPlugin
    {
        public override string Name => "Plugin whith rollbackfail";

        public override string Description => "plugin for fail 3";

        public override IEnumerable<ICommand> Commands => [
            new TestCommandFailRollback()
        ];

        public override IEnumerable<string> GetSubscriptions()
        {
            return [];
        }
    }
}
