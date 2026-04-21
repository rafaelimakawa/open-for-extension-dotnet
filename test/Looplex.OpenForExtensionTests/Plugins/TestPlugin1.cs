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
    public class TestPlugin1 : AbstractPlugin
    {
        public override string Name => "First test Plugin ";

        public override string Description => "plugin for test 1";

        public override IEnumerable<ICommand> Commands => [
            new TestCommand1()
        ];

        public override IEnumerable<string> GetSubscriptions()
        {
            return [];
        }
    }
}
