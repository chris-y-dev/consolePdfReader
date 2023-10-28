
using System.Text;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Pdf.Recognition;
using System.Linq;
using System.Drawing;

namespace consolePdfReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            const float DPI = 72;
            using (var fs = File.OpenRead(Path.Combine("pls-q3-23.pdf")))
            {
                // Initialize GcPdf
                var pdfDoc = new GcPdfDocument();
                // Load a PDF document
                pdfDoc.Load(fs);

                // The approx table bounds:
                var tableBounds = new RectangleF(0, 2.5f * DPI, 8.5f * DPI, 3.75f * DPI);

                // TableExtractOptions allow to fine-tune table recognition accounting for
                // specifics of the table formatting:
                var tableExtrctOpt = new TableExtractOptions();
                var GetMinimumDistanceBetweenRows = tableExtrctOpt.GetMinimumDistanceBetweenRows;

                // In this particular case, we slightly increase the minimum distance between rows
                // to make sure cells with wrapped text are not mistaken for two cells:
                tableExtrctOpt.GetMinimumDistanceBetweenRows = (list) =>
                {
                    var res = GetMinimumDistanceBetweenRows(list);
                    //return res * 1.2f;
                    return res;
                };

                // CSV: list to keep table data from all pages:
                var data = new List<List<string>>();



                for (int i = 0; i < pdfDoc.Pages.Count; ++i)
                {
                    // Get the table at the specified bounds:
                    var itable = pdfDoc.Pages[i].GetTable(tableBounds, tableExtrctOpt);
                    if (itable != null)
                    {
                        for (int row = 0; row < itable.Rows.Count; ++row)
                        {
                            // CSV: add next data row ignoring headers:
                            if (row > 0)
                                data.Add(new List<string>());
                            for (int col = 0; col < itable.Cols.Count; ++col)
                            {
                                var cell = itable.GetCell(row, col);
                                if (cell == null && row > 0)
                                    data.Last().Add("");
                                else
                                {
                                    if (cell != null && row > 0)
                                        data.Last().Add($"\"{cell.Text}\"");
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Data, {data}");

                for (int i = 0; i < data.Count; ++i)
                {
                    for (int j = 0; j < data[i].Count; ++j)
                    {
                        Console.WriteLine(data[i][j]);
                    }
                }






                }
        }
    }
}