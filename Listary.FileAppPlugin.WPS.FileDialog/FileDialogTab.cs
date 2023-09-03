using FlaUI.Core.AutomationElements;
using FlaUI.Core.WindowsAPI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core.Input;

namespace Listary.FileAppPlugin.WPS.FileDialog
{
    public class FileDialogTab : IFileTab, IOpenFolder, IOpenFile
    {
        private readonly IFileAppPluginHost _host;
        private readonly TextBox _fileNameEdit;

        public FileDialogTab(IFileAppPluginHost host, AutomationElement kcfdFileDialog)
        {
            _host = host;

            var kcfdFilterWidget = kcfdFileDialog.FindFirstDescendant(cf => cf.ByClassName("KcfdFilterWidget"));
            if (kcfdFilterWidget == null)
            {
                _host.Logger.LogError("Failed to find KcfdFilterWidget");
                return;
            }

            _fileNameEdit = kcfdFilterWidget
                .FindAllChildren(cf => cf.ByClassName("KcfdComboBox"))
                .Select(kcfdComboBox => kcfdComboBox.FindFirstChild(cf => cf.ByClassName("QLineEdit")))
                .FirstOrDefault(qLineEdit => qLineEdit != null)
                ?.AsTextBox();
            if (_fileNameEdit == null)
            {
                _host.Logger.LogError("Failed to find file name edit");
                return;
            }
        }

        private async Task<bool> SetFileNameAndEnter(string path)
        {
            if (_fileNameEdit != null)
            {
                try
                {
                    _fileNameEdit.Text = path;

                    _fileNameEdit.Focus();
                    Keyboard.Type(VirtualKeyShort.RETURN);

                    // Waits a little to allow inputs to be processed
                    await Task.Delay(100);

                    return true;
                }
                catch (Exception e)
                {
                    _host.Logger.LogError($"Failed to set the text of the file name edit: {e}");
                }
            }
            return false;
        }

        public Task<bool> OpenFolder(string path)
        {
            return SetFileNameAndEnter(path);
        }

        public Task<bool> OpenFile(string path)
        {
            return SetFileNameAndEnter(path);
        }
    }
}
