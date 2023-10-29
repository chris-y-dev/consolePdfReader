
using System.Text;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Pdf.Recognition;
using System.Linq;
using System.Drawing;
using System.ComponentModel;

namespace consolePdfReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*****This is the PDF reader.");
            Console.WriteLine("***Argument 1: PDF file name (e.g. tablefile.pdf)");
            Console.WriteLine("***Argument 2: Output file name (e.g. insertedData.csv)");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //No arguments
            if(args.Length < 1 ) {
                Console.WriteLine("No PDF file provided");
                return;
            }

            try
            {
                File.OpenRead(Path.Combine(args[0]));

                const float DPI = 72;
                using (var fs = File.OpenRead(Path.Combine(args[0])))
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
                    
                    //tableExtrctOpt.GetMinimumDistanceBetweenRows = (list) =>
                    //{
                    //    var res = GetMinimumDistanceBetweenRows(list);
                    //    return res * 1.2f;
                    //    //return res;
                    //};

                    // Row of cells
                    var data = new List<List<string>>();

                    for (int i = 0; i < pdfDoc.Pages.Count; ++i)
                    {
                        // Get the table at the specified bounds:
                        var itable = pdfDoc.Pages[i].GetTable(tableBounds, tableExtrctOpt);
                        if (itable != null)
                        {
                            for (int row = 0; row < itable.Rows.Count; ++row)
                            {
                                //Create new array and add cells
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
                                        {

                                            data.Last().Add($"\"{cell.Text}\"");

                                        }
                                    }
                                }
                            }
                        }
                    }

                    string directoryPath = @"H:\code\csharp\consolePdfReader\consolePdfReader\bin\Debug\net6.0";

                    //Check for target CSV file
                    string destinationFileName = "";
                    if (args.Length == 2)
                    {
                        destinationFileName = args[1];
                    } else
                    {
                        destinationFileName= "myData.csv";
                    }

                    //Delete all existing csv content (if exists)
                    File.WriteAllText(Path.Combine(directoryPath, destinationFileName), "");

                    //Loop through each row and print/write
                    for (int i = 0; i < data.Count; ++i)
                    {
                        Console.WriteLine($"====== Row {i} ======");
                        var lineToWrite = "";
                        //File.AppendAllLines(directoryPath, data[i]);
                        for (int j = 0; j < data[i].Count; ++j)
                        {
                            lineToWrite += data[i][j];
                            if (j + 1 < data[i].Count)
                            {
                                lineToWrite += ",";
                            }
                            Console.WriteLine(data[i][j]);
                        }
                        File.AppendAllText(Path.Combine(directoryPath, destinationFileName), lineToWrite);
                        lineToWrite = "";
                        File.AppendAllText(Path.Combine(directoryPath, destinationFileName), "\n");
                    }



                }
            }
            catch (Exception ex)
            {
                //Catch if file not found
                Console.WriteLine(ex.Message);
            }
        }
    }
}