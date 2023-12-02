using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Path = System.IO.Path;

namespace PDFtoMD;

internal static class Program
{
    private const string InputDirectory = @"C:\Games Development\C#\PDFtoMD\PDFtoMD\Input";
    private const string OutputDirectory = @"C:\Games Development\C#\PDFtoMD\PDFtoMD\Output";

    private static void Main()
    {
        foreach (string filePath in Directory.GetFiles(InputDirectory, "*.pdf"))
        {
            try
            {
                ConvertPdfToMarkdown(filePath, OutputDirectory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {filePath}: {ex.Message}");
            }
        }
    }

    private static void ConvertPdfToMarkdown(string filePath, string outputDirectory)
    {
        using PdfReader reader = new PdfReader(filePath);
        StringBuilder cleanedText = new StringBuilder();

        for (int i = 1; i <= reader.NumberOfPages; i++)
        {
            string text = PdfTextExtractor.GetTextFromPage(reader, i);
            cleanedText.Append(CleanText(text));
        }

        using StreamWriter writer = new StreamWriter(Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(filePath) + ".md"));
        writer.Write(cleanedText.ToString());
    }
    
    private static string CleanText(string text)
    {
        text = Regex.Replace(text, @"\bNotes\b", "");
        text = Regex.Replace(text, @"Dr\.Fone iPhone \(210\) \d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2} (AM|PM)", "");
        text = Regex.Replace(text, @"(.+?)\s+\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}\s+\1", "");
        return text;
    }
}