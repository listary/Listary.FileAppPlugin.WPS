using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Microsoft.Extensions.Logging;

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
            string processName = Path.GetFileName(Win32Utils.GetProcessPathFromHwnd(hWnd)).ToLowerInvariant();
            if (processName is "wps.exe" or "et.exe" or "wpp.exe" or "wpspdf.exe")
            {
                // It is WPS's file dialog?
                try
                {
                    var automation = new UIA3Automation();
                    AutomationElement kcfdFileDialog = automation.FromHandle(hWnd);
                    if (kcfdFileDialog.ClassName == "KcfdFileDialog")
                    {
                        return new FileDialogWindow(_host, hWnd, automation, kcfdFileDialog);
                    }
                }
                catch (TimeoutException e)
                {
                    _host.Logger.LogWarning($"UIA timeout: {e}");
                }
                catch (Exception e)
                {
                    _host.Logger.LogError($"Failed to bind window: {e}");
                }
            }
            return null;
        }
    }
}
