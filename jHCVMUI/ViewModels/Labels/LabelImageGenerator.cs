namespace jHCVMUI.ViewModels.Labels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CommonHandicapLib.Types;

    using HandicapModel;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using jHCVMUI.Views.Labels;

    /// <summary>
    /// Static class which is used to create image files containing labels and their crib sheets.
    /// </summary>
    public static class LabelImageGenerator
    {
        public const int numberOfCribSheetColumns = 2;
        public const int numberOfCribSheetRows = 12;

        public const string raceLabelsName = "RaceLabels";
        public const string raceLabelsCribName = "RaceLabelsCribSheet";
        public const string spareLabelsName = "SpareLabels";
        public const string spareLabelsCribName = "SpareLabelsCribSheet";

        /// <summary>
        /// Create a set of image files which display the labels provided.
        /// </summary>
        /// <param name="athletelabels">all athlete labels to print images</param>
        /// <param name="savePath">path to save the images to</param>
        /// <param name="numberOfColumns">number of columns of labels</param>
        /// <param name="numberOfRow">number of rows of labels</param>
        public static void CreateRaceLabels(List<AthleteLabel> athletelabels,
                                            string savePath,
                                            int numberOfColumns,
                                            int numberOfRows)
        {
            ObservableCollection<AthleteLabel> labels = new ObservableCollection<AthleteLabel>();
            int labelsOnSheet = 0;
            int sheetNumber = 1;



            // loop through all athletedetails.
            // Every (columns * rows) call create sheet.

            foreach (AthleteLabel athlete in athletelabels)
            {
                ++labelsOnSheet;



                labels.Add(athlete);

                if (labelsOnSheet == numberOfColumns * numberOfRows)
                {
                    CreateSingleSheet(new LabelsSheetViewModel(labels),
                                      savePath + Path.DirectorySeparatorChar + raceLabelsName + sheetNumber.ToString() + ".png");

                    ++sheetNumber;
                    labelsOnSheet = 0;
                    labels = new ObservableCollection<AthleteLabel>();
                }
            }

            // Save any remaining labels.
            if (labelsOnSheet > 0)
            {
                CreateSingleSheet(new LabelsSheetViewModel(labels),
                                  savePath + Path.DirectorySeparatorChar + raceLabelsName + sheetNumber.ToString() + ".png");
            }
        }

        /// <summary>
        /// Creates a set of images containing the crib sheets for the labels.
        /// </summary>
        /// <param name="athletelabels">all athlete labels to print images</param>
        /// <param name="savePath">path to save the images to</param>
        public static void CreateRaceLabelsCribSheet(List<AthleteLabel> athletelabels,
                                                     string savePath)
        {
            ObservableCollection<AthleteLabel> labels = new ObservableCollection<AthleteLabel>();
            int labelsOnSheet = 0;
            int sheetNumber = 1;
            int numberOfCribPerSheet = numberOfCribSheetColumns * numberOfCribSheetRows;

            foreach (AthleteLabel athlete in athletelabels)
            {
                ++labelsOnSheet;
                labels.Add(athlete);

                if (labelsOnSheet == numberOfCribPerSheet)
                {
                    CreateSingleCribSheet(new LabelsSheetViewModel(labels),
                                          savePath + Path.DirectorySeparatorChar + raceLabelsCribName + sheetNumber.ToString() + ".png");

                    ++sheetNumber;
                    labelsOnSheet = 0;
                    labels = new ObservableCollection<AthleteLabel>();
                }
            }

            // Save any remaining labels.
            if (labelsOnSheet > 0)
            {
                CreateSingleCribSheet(new LabelsSheetViewModel(labels),
                                      savePath + Path.DirectorySeparatorChar + raceLabelsCribName + sheetNumber.ToString() + ".png");

                // **************************Error**************************
                // For some reason when the print race labels command is selected this saves properly, but when the the print all
                // (race and spare) command is selected, the last sheet isn't saved. That's this one. I don't know why, however
                // my plan is to put all this in a pdf at some point. Therefore, rather than spend too much time thinking about it
                // I will just save it twice.
                CreateSingleCribSheet(new LabelsSheetViewModel(labels),
                                      savePath + Path.DirectorySeparatorChar + raceLabelsCribName + sheetNumber.ToString() + ".png");
                // **************************Error**************************
            }
        }

        /// <summary>
        /// Creates a set of image files, each containing a spare barcodes.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="savePath">path to save the images to</param>
        /// <param name="numberOfSpareSheets">number of spare sheets to create</param>
        /// <param name="numberOfColumns">number of columns of labels</param>
        /// <param name="numberOfRow">number of rows of labels</param>
        /// <param name="eventDetails">details of the event, name and date</param>
        public static void CreateSpareLabels(
            IModel model,
            string savePath,
            int numberOfSpareSheets,
            int numberOfColumns,
            int numberOfRow,
            string eventDetails)
        {
            int raceNumber = model.Athletes.NextAvailableRaceNumber;
            SeriesConfigType config = SeriesConfigMngr.ReadSeriesConfiguration();

            for (int sheetNumber = 0; sheetNumber < numberOfSpareSheets; ++sheetNumber)
            {
                ObservableCollection<AthleteLabel> sheet = new ObservableCollection<AthleteLabel>();

                for (int labelIndex = 0; labelIndex < numberOfColumns * numberOfRow; ++labelIndex)
                {
                    AthleteLabel newLabel =
                      new AthleteLabel(
                        string.Empty,
                        string.Empty,
                        GetRaceNumber(config?.NumberPrefix, raceNumber),
                        null,
                        false);
                    newLabel.EventDetails = eventDetails;
                    newLabel.AthleteLabelWidth = A4Details.GetLabelWidth96DPI(numberOfColumns);
                    newLabel.AthleteLabelHeight = A4Details.GetLabelHeight96DPI(numberOfRow);

                    sheet.Add(newLabel);

                    // TODO UpdateString
                    ++raceNumber;
                }

                CreateSingleSheet(new LabelsSheetViewModel(sheet),
                                  savePath + Path.DirectorySeparatorChar + spareLabelsName + sheetNumber.ToString() + ".png");
            }
        }

        /// <summary>
        /// Creates a set of images containing spare barcodes on a crib sheet.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="savePath">path to save the images to</param>
        /// <param name="numberOfSpareSheets">number of spare sheets to create</param>
        /// <param name="numberOfLabelColumns">number of columns of labels</param>
        /// <param name="numberOfLabelRow">number of rows of labels</param>
        public static void CreateSpareLabelsCribSheet(
            IModel model,
            string savePath,
            int numberOfSpareSheets,
            int numberOfLabelColumns,
            int numberOfLabelRow)
        {
            SeriesConfigType config = SeriesConfigMngr.ReadSeriesConfiguration();

            int summarySheetNumber = 1;
            int raceNumber = model.Athletes.NextAvailableRaceNumber;
            int numberOfSpareLabels = numberOfLabelColumns * numberOfLabelRow * numberOfSpareSheets;
            int numberOfCribPerSheet = numberOfCribSheetColumns * numberOfCribSheetRows;
            ObservableCollection<AthleteLabel> labels = new ObservableCollection<AthleteLabel>();

            for (int labelIndex = 0; labelIndex < numberOfSpareLabels; ++labelIndex)
            {
                labels.Add(new AthleteLabel("________________________________________________________________________",
                           string.Empty,
                           GetRaceNumber(config?.NumberPrefix, raceNumber),
                           null,
                           false));

                if (labels.Count == numberOfCribPerSheet)
                {
                    CreateSingleCribSheet(
                      new LabelsSheetViewModel(labels),
                      savePath + Path.DirectorySeparatorChar + spareLabelsCribName + summarySheetNumber.ToString() + ".png");

                    ++summarySheetNumber;
                    labels = new ObservableCollection<AthleteLabel>();
                }

                // TODO UpdateString
                ++raceNumber;
            }

            if (labels.Count > 0)
            {
                CreateSingleCribSheet(new LabelsSheetViewModel(labels),
                                      savePath + Path.DirectorySeparatorChar + spareLabelsCribName + summarySheetNumber.ToString() + ".png");
            }
        }

        /// <summary>
        /// Creates a single sheet of labels.
        /// </summary>
        /// <param name="labels">labels to save to an image</param>
        /// <param name="imageName">file name including full path details</param>
        private static void CreateSingleSheet(LabelsSheetViewModel labels, string imageName)
        {
            LabelSheetDialog labelDialog = new LabelSheetDialog();

            labelDialog.DataContext = labels;

            labelDialog.Show();

            labelDialog.SaveMyWindow(96, imageName);

            labelDialog.Close();
        }

        /// <summary>
        /// Creates a crib sheet of bar codes.
        /// </summary>
        /// <param name="labels">labels to save to an image</param>
        /// <param name="imageName">file name including full path details</param>
        private static void CreateSingleCribSheet(
          LabelsSheetViewModel labels,
          string imageName)
        {
            SummarySheetDialog summaryDialog = new SummarySheetDialog();

            summaryDialog.DataContext = labels;

            summaryDialog.Show();

            summaryDialog.SaveMyWindow(96, imageName);

            summaryDialog.Close();
        }

        private static string GetRaceNumber(
          string numberPrefix,
          int raceNumber)
        {
            return numberPrefix + raceNumber.ToString("000000");
        }

    }
}