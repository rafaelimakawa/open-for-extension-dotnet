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
    public class TestPlugin3 : AbstractPlugin
    {
        public override string Name => "Third test Plugin ";

        public override string Description => "plugin for test 3";

        public override IEnumerable<ICommand> Commands => [
            new TestCommand3()
        ];

        public override IEnumerable<string> GetSubscriptions()
        {
            return [];
        }
    }
}
