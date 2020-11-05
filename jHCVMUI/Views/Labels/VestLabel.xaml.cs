namespace jHCVMUI.Views.Labels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using BarcodeLib;

    /// <summary>
    /// Interaction logic for VestLabel.xaml
    /// </summary>
    public partial class VestLabel : UserControl
    {
        BarcodeLib.Barcode raceBarcode = new BarcodeLib.Barcode();

        /// <summary>
        /// Initialises a new instance of <see cref="VestLabel"/> class.
        /// </summary>
        public VestLabel()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Control has loaded, generate a barcode image
        /// </summary>
        /// <param name="sender">event argument</param>
        /// <param name="e">control object</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TYPE barcodeType = TYPE.CODE128;

            try
            {
                raceBarcode.IncludeLabel = true;
                raceBarcode.Alignment = AlignmentPositions.CENTER;
                raceBarcode.LabelPosition = LabelPositions.BOTTOMCENTER;
                raceBarcode.LabelFont = new Font("Times New Roman", 28);
                raceBarcode.Width = 400;

                // Encode image.
                // ***************************
                // For some reason this causes an "Object Instance not set to an instance of an object". Don't know why.
                // The error wasn't raised here, but in LabelGenerationDialog.xaml.
                // ***************************
                ImageSource myImage = GetImageStream(this.raceBarcode.Encode(barcodeType, this.numberLabel.Content.ToString()));
                this.barcodeImage.Source = myImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);

        /// <summary>
        /// Converts an image to a bitmap
        /// </summary>
        /// <param name="origImage">image to convert</param>
        /// <returns>new bitmap</returns>
        public BitmapSource GetImageStream(System.Drawing.Image origImage)
        {
            Bitmap barcodeBitmap = new Bitmap(origImage);
            IntPtr bitmapPointer = barcodeBitmap.GetHbitmap();
            BitmapSource bitmapSource =
              System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmapPointer,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            //freeze bitmapSource and clear memory to avoid memory leaks
            bitmapSource.Freeze();
            DeleteObject(bitmapPointer);

            return bitmapSource;
        }
    }
}