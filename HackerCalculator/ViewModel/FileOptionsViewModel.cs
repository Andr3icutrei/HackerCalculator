using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HackerCalculator.Model;
using HackerCalculator.Services;

namespace HackerCalculator.ViewModel
{
    public class FileOptionsViewModel
    {
        public ClipboardService ClipboardService;

        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand CutCommand { get; }

        public FileOptionsViewModel()
        {
            ClipboardService = new ClipboardService();

            CopyCommand = new RelayCommandGeneric<Calculation>(Copy, CanEditWrapper);
            CutCommand = new RelayCommandGeneric<Calculation>(Cut, CanEditWrapper);
            PasteCommand = new RelayCommandGeneric<Calculation>(Paste,CanPasteWrapper);
        }

        private void Copy(Calculation c)
        {
            ClipboardService.Copy(c);
        }

        private void Cut(Calculation c)
        {
            ClipboardService.Cut(c);
        }

        private void Paste(Calculation c)
        {
            ClipboardService.Paste(c);
        }
        private bool CanEditWrapper(Calculation c)
        {
            return CanEdit(c);
        }

        private bool CanPasteWrapper(Calculation c)
        {
            return CanPaste(c);
        }

        private bool CanEdit(Calculation c)
        {
            return c?.CompleteCalculation != null;
        }

        private bool CanPaste(Calculation c)
        {
            return !string.IsNullOrEmpty(ClipboardService.Clipboard) &&
                   c?.CompleteCalculation != null;
        }   
    }
}
