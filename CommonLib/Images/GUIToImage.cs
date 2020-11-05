namespace CommonLib.Images
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Input;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;

  /// <summary>
  /// Used to send a GUI object to an image file.
  /// </summary>
  public static class GUIToImage
  {
    /// <summary>
    /// Save the window as a bitmap.
    /// </summary>
    /// <param name="window">window to print</param>
    /// <param name="dpi">dot per inch</param>
    /// <param name="filename">filename to save to</param>
    public static void SaveWindow(Window window,
                                  int    dpi,
                                  string filename)
    {
      RenderTargetBitmap bitmap = new RenderTargetBitmap((int)window.Width,
                                                         (int)window.Height,
                                                         dpi,
                                                         dpi,
                                                         PixelFormats.Pbgra32);

      bitmap.Render(window);

      SaveRTBAsPNG(bitmap, filename);
    }

    /// <summary>
    /// Save the grid object as a bitmap.
    /// </summary>
    /// <param name="window">window which contains the grid</param>
    /// <param name="grid">grid to save as an image</param>
    /// <param name="dpi">dots per inch</param>
    /// <param name="filename">filename to save to</param>
    public static void SaveGrid(Window window,
                                Grid   grid,
                                int    dpi, 
                                string filename)
    {
      System.Windows.Size size = new System.Windows.Size(window.Width, window.Height);
      grid.Measure(size);

      var bitmap = new RenderTargetBitmap((int)grid.Width *200/96,
                                          (int)grid.Height *200/96,
                                          200,
                                          200,
                                          PixelFormats.Pbgra32);

      bitmap.Render(grid);

      SaveRTBAsPNG(bitmap, filename);
    }

    /// <summary>
    /// Save as PNG file.
    /// </summary>
    /// <param name="bitmap">bitmap to save</param>
    /// <param name="filename">filename of the image</param>
    private static void SaveRTBAsPNG(RenderTargetBitmap bitmap,
                                     string             filename)
    {
      //JpegBitmapEncoder encoder = new JpegBitmapEncoder();
      PngBitmapEncoder encoder = new PngBitmapEncoder();

      encoder.Frames.Add(BitmapFrame.Create(bitmap));

      using (FileStream saveFile = File.Create(filename))
      {
        encoder.Save(saveFile);
      }
    }
  }
}