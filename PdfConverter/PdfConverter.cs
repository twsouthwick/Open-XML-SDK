using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PdfConverter
{
    internal class PdfConverter : IDisposable
    {
        private readonly Application _application;
        private readonly DisablePdfWarning _registry;

        public PdfConverter()
        {
            _application = new Application
            {

            };

            _registry = new DisablePdfWarning(_application.Version);

#if DEBUG
            _application.Visible = true;
#endif
        }

        public void Convert(string path, string newPath = null)
        {
            var document = _application.Documents.Open(path, ConfirmConversions: false, Visible: true);

            if (string.IsNullOrEmpty(newPath))
            {
                newPath = Path.ChangeExtension(path, ".docx");
            }

            document.SaveAs2(newPath);
            document.Close();
        }

        ~PdfConverter()
        {
            _application.Quit();
            _registry.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _application.Quit();
            _registry.Dispose();
        }
    }
    internal sealed class DisablePdfWarning : IDisposable
    {
        private const string PdfWarningNameKeyName = "DisableConvertPdfWarning";

        private readonly RegistryKey _key;
        private readonly object _value;

        public DisablePdfWarning(string version)
        {
            _key = Registry.CurrentUser.OpenSubKey(Path.Combine("SOFTWARE", "Microsoft", "Office", version, "Word", "options"), true);
            _value = _key?.GetValue(PdfWarningNameKeyName);
            _key.SetValue(PdfWarningNameKeyName, 1);
        }

        public void Dispose()
        {
            if (_value is null)
            {
                _key.DeleteValue(PdfWarningNameKeyName);
            }
            else
            {
                _key.SetValue(PdfWarningNameKeyName, _value);
            }

            _key.Dispose();
        }
    }
}