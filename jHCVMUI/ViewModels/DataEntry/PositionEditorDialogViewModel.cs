using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jHCVMUI.ViewModels.DataEntry
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;

    using CommonLib.Helpers;
    using CommonHandicapLib.Types;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.AthletesModel;

    using NynaeveLib.Commands;
    using HandicapModel.Interfaces;

    public class PositionEditorDialogViewModel : DialogViewModelBase
    {
        /// <summary>
        /// Junior handicap model.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Indicates the index of the currently selected barcode.
        /// </summary>
        private int barcodesIndex;

        /// <summary>
        /// Initialises a new instance of the <see cref="PositionEditorDialogViewModel"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        public PositionEditorDialogViewModel(
            IModel model)
        {
            this.model = model;
            this.Barcodes = new ObservableCollection<PositionEditorRawItem>();
            this.MissingPositionTokens = new List<string>();
            this.barcodesIndex = -1;

            this.OpenCommand =
              new SimpleCommand(
                this.OpenFile,
                this.CanOpenFile);
            this.SaveAsCommand =
              new SimpleCommand(
                this.SaveAsFile,
                this.CanSaveAsFile);

            this.UpCommand =
              new SimpleCommand(
                this.MoveUp,
                this.CanMoveUp);
            this.DeleteCommand =
              new SimpleCommand(
                this.DeleteBarcode,
                this.IsIndexValid);
            this.DownCommand =
              new SimpleCommand(
                this.MoveDown,
                this.CanMoveDown);
        }

        /// <summary>
        /// Command used move an item in the <see cref="Barcodes"/> collection.
        /// </summary>
        public ICommand UpCommand { get; protected set; }

        /// <summary>
        /// Command used move an item in the <see cref="Barcodes"/> collection.
        /// </summary>
        public ICommand DownCommand { get; protected set; }

        /// <summary>
        /// Command used delete an item from the <see cref="Barcodes"/> collection.
        /// </summary>
        public ICommand DeleteCommand { get; protected set; }

        /// <summary>
        /// Gets the barcode data which is to be manipulated by this editor view model.
        /// </summary>
        public ObservableCollection<PositionEditorRawItem> Barcodes { get; }

        /// <summary>
        /// Gets or sets the index of the selected barcode.
        /// </summary>
        public int BarcodesIndex
        {
            get
            {
                return barcodesIndex;
            }

            set
            {
                barcodesIndex = value;
            }
        }

        /// <summary>
        /// Gets the number of duplicate lines.
        /// </summary>
        public int NumberOfDoubleReads { get; set; }

        /// <summary>
        /// Gets the number of duplicate numbers or positions.
        /// </summary>
        public int NumberOfDuplicates { get; set; }

        /// <summary>
        /// Gets the number of missing numbers or barcodes, this is ensuring that everything is in a 
        /// number/position pair.
        /// </summary>
        public int NumberOfMissingBarcodes { get; set; }

        /// <summary>
        /// Gets the number of numbers which aren't registered.
        /// </summary>
        public int NumberOfUnrecognisedNumbers { get; set; }

        /// <summary>
        /// List any position tokens which are expected, but aren't present. This isn't neccessarily a
        /// problem as these can be added individually at a later time.
        /// </summary>
        public List<string> MissingPositionTokens { get; set; }

        /// <summary>
        /// Gets the missing position tokens as a list.
        /// </summary>
        public string MissingPositions =>
          StringCollectionOutput.ListToString(this.MissingPositionTokens);

        /// <summary>
        /// 
        /// </summary>
        private void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            List<string> importedData;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                importedData = CommonIO.ReadFile(dialog.FileName);
            }
            else
            {
                importedData = new List<string>();
            }


            foreach (string input in importedData)
            {
                PositionEditorRawItem raw =
                  new PositionEditorRawItem(
                    input);
                this.Barcodes.Add(raw);
            }

            this.CalculateFaults();
            this.UpdateProperties();
        }

        /// <summary>
        /// Save the contents of the dialog to a file.
        /// </summary>
        private void SaveAsFile()
        {
            this.SaveAsTxtFile(this.SaveFile);
        }

        /// <summary>
        /// Save the current file.
        /// </summary>
        /// <param name="filename">name of the file to save to</param>
        private void SaveFile(string filename)
        {
            using (StreamWriter streamWriter = new StreamWriter(filename))
            {
                foreach (PositionEditorRawItem barcode in this.Barcodes)
                {
                    streamWriter.WriteLine(barcode.Complete);
                }
            }
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        private void UpdateProperties()
        {
            this.CalculateFaults();
            this.RaisePropertyChangedEvent(nameof(this.Barcodes));
            this.RaisePropertyChangedEvent(nameof(this.BarcodesIndex));
            this.RaisePropertyChangedEvent(nameof(this.NumberOfDoubleReads));
            this.RaisePropertyChangedEvent(nameof(this.NumberOfDuplicates));
            this.RaisePropertyChangedEvent(nameof(this.NumberOfMissingBarcodes));
            this.RaisePropertyChangedEvent(nameof(this.NumberOfUnrecognisedNumbers));
            this.RaisePropertyChangedEvent(nameof(this.MissingPositionTokens));
            this.RaisePropertyChangedEvent(nameof(this.MissingPositions));
        }

        /// <summary>
        /// Search all the <see cref="PositionEditorRawItem"/> and determine whether there are any
        /// faults.
        /// </summary>
        private void CalculateFaults()
        {
            this.ClearExistingFaults();
            this.CheckUnrecognisedEventNumbers();
            this.CheckDuplicates();
            this.CheckPairs();
            this.CheckDoubleReads();

            this.DetermineMissingPositionTokens();

            this.CountFaults();
        }

        /// <summary>
        /// Reset the faults for each <see cref="PositionEditorRawItem"/> to 
        /// <see cref="PositionEditorFaults.NoFault"/>.
        /// </summary>
        private void ClearExistingFaults()
        {
            foreach (PositionEditorRawItem barcode in this.Barcodes)
            {
                barcode.Fault = PositionEditorFaults.NoFault;
            }

            this.MissingPositionTokens = new List<string>();
        }

        /// <summary>
        /// Check to see if a line has been read twice, if any found, mark both with the error.
        /// </summary>
        private void CheckDoubleReads()
        {
            // Don't need to work on the last item.
            for (int index = 0; index < this.Barcodes.Count - 1; ++index)
            {
                if (string.Compare(this.Barcodes[index].Barcode, this.Barcodes[index + 1].Barcode) == 0)
                {
                    this.Barcodes[index].Fault = PositionEditorFaults.DoubleRead;
                    this.Barcodes[index + 1].Fault = PositionEditorFaults.DoubleRead;
                }
            }
        }

        /// <summary>
        /// Check to see if a barcode has been read twice.
        /// </summary>
        private void CheckDuplicates()
        {
            for (int index = 0; index < this.Barcodes.Count; ++index)
            {
                if (this.Barcodes.Count(b => b.Barcode == this.Barcodes[index].Barcode) > 1)
                {
                    this.Barcodes[index].Fault = PositionEditorFaults.Duplicate;
                }
            }
        }

        /// <summary>
        /// Check to make sure that the barcodes are read in number, position pairs. It doesn't matter
        /// what the order is.
        /// </summary>
        private void CheckPairs()
        {
            for (int index = 0; index < this.Barcodes.Count; ++index)
            {
                if (index % 2 == 0 &&
                  index + 1 < this.Barcodes.Count &&
                  this.Barcodes[index].Type == this.Barcodes[index + 1].Type)
                {
                    this.Barcodes[index].Fault = PositionEditorFaults.Missing;
                }
            }
        }

        /// <summary>
        /// Search through all athletes to ensure that all race numbers are registered.
        /// Raise a fault for each one which isn't
        /// </summary>
        private void CheckUnrecognisedEventNumbers()
        {
            for (int index = 0; index < this.Barcodes.Count; ++index)
            {
                // Only consider race numbers.
                if (this.Barcodes[index].Type == PositionEditorType.Position)
                {
                    continue;
                }

                // Search through each athlete and break if found.
                bool found = false;
                foreach (AthleteDetails athlete in this.model.Athletes.AthleteDetails)
                {
                    if (athlete.MatchesAthlete(this.Barcodes[index].Barcode))
                    {
                        found = true;
                        break;
                    }
                }

                // Raise fault if not found.
                if (!found)
                {
                    this.Barcodes[index].Fault = PositionEditorFaults.NumberNotRecognised;
                }
            }
        }

        private void DetermineMissingPositionTokens()
        {
            if (this.Barcodes.Count < 1)
            {
                return;
            }

            int minBarcodes = 0;
            int maxBarcodes = 0;

            foreach (PositionEditorRawItem barcode in this.Barcodes)
            {
                if (barcode.Type == PositionEditorType.RaceNumber ||
                  barcode.Position < 0)
                {
                    continue;
                }

                if (minBarcodes == 0)
                {
                    minBarcodes = barcode.Position;
                    maxBarcodes = barcode.Position;
                }
                else
                {
                    minBarcodes = this.ReplacePosition(true, minBarcodes, barcode.Position);
                    maxBarcodes = this.ReplacePosition(false, maxBarcodes, barcode.Position);
                }
            }

            this.CalculateMissingPositions(
              minBarcodes,
              maxBarcodes);
        }

        /// <summary>
        /// Return the <paramref name="barcodePosition"/> if the <paramref name="barcodePosition"/> is
        /// less than or greater the <paramref name="currentRecord"/> as dictated by 
        /// <paramref name="lessThan"/>. 
        /// </summary>
        /// <param name="lessThan">less than or greater than</param>
        /// <param name="currentRecord">current position record</param>
        /// <param name="barcodePosition">position of the barcode being tested.</param>
        /// <returns>
        /// larger or smaller value, as dictated.
        /// </returns>
        private int ReplacePosition(
          bool lessThan,
          int currentRecord,
          int barcodePosition)
        {
            if (lessThan && barcodePosition < currentRecord)
            {
                return barcodePosition;
            }
            else if (!lessThan && barcodePosition > currentRecord)
            {
                return barcodePosition;
            }

            return currentRecord;
        }

        private void CalculateMissingPositions(
          int minKnownPosition,
          int maxKnownPosition)
        {
            this.MissingPositionTokens = new List<string>();

            for (int position = minKnownPosition; position < maxKnownPosition; ++position)
            {
                if (!this.Barcodes.Any(b => b.Position == position))
                {
                    this.MissingPositionTokens.Add(position.ToString());
                }
            }
        }

        /// <summary>
        /// Count the number of faults.
        /// </summary>
        private void CountFaults()
        {
            this.NumberOfDoubleReads = 0;
            this.NumberOfDuplicates = 0;
            this.NumberOfMissingBarcodes = 0;
            this.NumberOfUnrecognisedNumbers = 0;

            foreach (PositionEditorRawItem barcode in this.Barcodes)
            {
                switch (barcode.Fault)
                {
                    case PositionEditorFaults.DoubleRead:
                        ++this.NumberOfDoubleReads;
                        break;
                    case PositionEditorFaults.Duplicate:
                        ++this.NumberOfDuplicates;
                        break;
                    case PositionEditorFaults.Missing:
                        ++this.NumberOfMissingBarcodes;
                        break;
                    case PositionEditorFaults.NumberNotRecognised:
                        ++this.NumberOfUnrecognisedNumbers;
                        break;
                }
            }
        }

        /// <summary>
        /// Indicates when the <see cref="BarcodesIndex"/> represents a valid index
        /// </summary>
        /// <returns>valid index</returns>
        private bool IsIndexValid()
        {
            if (this.Barcodes == null)
            {
                return false;
            }

            return this.BarcodesIndex >= 0 && this.BarcodesIndex < this.Barcodes.Count;
        }

        /// <summary>
        /// Move selected item up one in the <see cref="Barcodes"/> collection
        /// </summary>
        private void MoveUp()
        {
            this.Barcodes.Move(
              this.BarcodesIndex,
              this.BarcodesIndex - 1);
            this.UpdateProperties();
        }

        /// <summary>
        /// Move selected item down one in the <see cref="Barcodes"/> collection
        /// </summary>
        private void MoveDown()
        {
            this.Barcodes.Move(
              this.BarcodesIndex,
              this.BarcodesIndex + 1);
            this.UpdateProperties();
        }

        private void DeleteBarcode()
        {
            int localIndex = this.BarcodesIndex;
            this.Barcodes.RemoveAt(this.BarcodesIndex);
            this.BarcodesIndex = localIndex;
            this.UpdateProperties();
        }

        /// <summary>
        /// Determines when the <see cref="UpCommand"/> can be selected;
        /// </summary>
        /// <returns>
        /// It is possible to move an item up one position in <see cref="Barcodes"/>
        /// </returns>
        private bool CanMoveUp()
        {
            return this.IsIndexValid() && this.BarcodesIndex != 0;
        }

        /// <summary>
        /// Determines when the <see cref="DownCommand"/> can be selected;
        /// </summary>
        /// <returns>
        /// It is possible to move an item down one position in <see cref="Barcodes"/>
        /// </returns>
        private bool CanMoveDown()
        {
            return this.IsIndexValid() && this.BarcodesIndex != this.Barcodes.Count - 1;
        }
    }
}