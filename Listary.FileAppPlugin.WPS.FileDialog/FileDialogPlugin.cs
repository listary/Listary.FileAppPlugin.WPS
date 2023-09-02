using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace Listary.FileAppPlugin.WPS.FileDialog
{
    public class FileDialogPlugin : IFileAppPlugin
    {
        private IFileAppPluginHost _host;

        public bool IsOpenedFolderProvider => false;
        
        public bool IsQuickSwitchTarget => true;
        
        public bool IsSharedAcrossApplications => false;

        public SearchBarType SearchBarType => SearchBarType.Fixed;
        
        public async Task<bool> Initialize(IFileAppPluginHost host)
        {
            _host = host;
            return true;
        }

        public IFileWindow BindFileWindow(IntPtr hWnd)
        {
            // It is from WPS?
            if (Win32Utils.GetProcessPathFromHwnd(hWnd).EndsWith("\\wps.exe", StringComparison.OrdinalIgnoreCase))
            {
                // It is WPS's file dialog?
                var automation = new UIA3Automation();
                AutomationElement kcfdFileDialog = automation.FromHandle(hWnd);
                if (kcfdFileDialog.ClassName == "KcfdFileDialog")
                {
                    return new FileDialogWindow(_host, hWnd, automation, kcfdFileDialog);
                }
            }
            return null;
        }
    }
}
