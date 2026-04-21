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
    internal class TestPlugin2 : AbstractPlugin
    {
        public override string Name => "second test Plugin ";

        public override string Description => "plugin for test 2";

        public override IEnumerable<ICommand> Commands => [
            new TestCommand2()
        ];

        public override IEnumerable<string> GetSubscriptions()
        {
            return [];
        }
    }
}
