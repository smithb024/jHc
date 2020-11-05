namespace jHCVMUI.Views.Image
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

  public static class ImageConverter
  {
    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteObject(IntPtr value);

    /// <summary>
    /// Converts an image to a bitmap
    /// </summary>
    /// <param name="origImage">image to convert</param>
    /// <returns>new bitmap</returns>
    public static BitmapSource GetImageStream(System.Drawing.Image origImage)
    {
      Bitmap barcodeBitmap = new Bitmap(origImage);
      IntPtr bitmapPointer = barcodeBitmap.GetHbitmap();
      BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmapPointer,
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