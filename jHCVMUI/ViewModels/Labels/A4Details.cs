namespace jHCVMUI.ViewModels.Labels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  /// <summary>
  /// Details regarding A4 paper.
  /// </summary>
  public static class A4Details
  {
    /// <summary>
    /// Gets the width of an A4 piece of paper assuming 96 DPI.
    /// </summary>
    public static int A4Width96DPI
    {
      get { return 792; }
    }

    /// <summary>
    /// Gets the height of an A4 piece of paper assuming 96 DPI.
    /// </summary>
    public static int A4Height96DPI
    {
      get { return 1122; }
    }

    /// <summary>
    /// Gets the margin used for an A4 piece of paper assuming 96 DPI.
    /// </summary>
    public static int LabelStandardTopMargin96DPI
    {
      get { return 57; }
    }

    /// <summary>
    /// Gets the margin used for an A4 piece of paper assuming 96 DPI.
    /// </summary>
    public static int LabelStandardSideMargin96DPI
    {
      get { return 27; }
    }

    /// <summary>
    /// Gets the width of a label assuming 96 DPI and the number of given columns.
    /// </summary>
    /// <param name="columns">number of columns used</param>
    /// <returns>width of label</returns>
    public static int GetLabelWidth96DPI(int columns)
    {
      int width = (A4Width96DPI - 2 * LabelStandardSideMargin96DPI) / columns;

      // Ensure they fit.
      if (width * columns > A4Width96DPI - 2 * LabelStandardSideMargin96DPI)
      {
        --width;
      }

      return width;
    }

    /// <summary>
    /// Gets the width of a label assuming 96 DPI and the number of given columns.
    /// </summary>
    /// <param name="rows">number of columns used</param>
    /// <returns>width of label</returns>
    public static int GetLabelHeight96DPI(int rows)
    {
      int height = (A4Height96DPI - 2 * LabelStandardTopMargin96DPI) / rows;

      // Ensure they fit.

      if (height * rows > A4Height96DPI - 2 * LabelStandardTopMargin96DPI)
      {
        --height;
      }

      return height;
    }
  }
}