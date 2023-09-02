using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace Listary.FileAppPlugin.WPS.FileDialog
{
    public class FileDialogWindow : IFileWindow, IDisposable
    {
        private readonly IFileAppPluginHost _host;
        private readonly UIA3Automation _automation;
        private readonly AutomationElement _kcfdFileDialog;

        public IntPtr Handle { get; }

        public FileDialogWindow(IFileAppPluginHost host, IntPtr hWnd, UIA3Automation automation, AutomationElement kcfdFileDialog)
        {
            _host = host;
            Handle = hWnd;
            _automation = automation;
            _kcfdFileDialog = kcfdFileDialog;
        }

        public void Dispose()
        {
            _automation.Dispose();
        }

        public async Task<IFileTab> GetCurrentTab()
        {
            return new FileDialogTab(_host, _kcfdFileDialog);
        }
    }
}
