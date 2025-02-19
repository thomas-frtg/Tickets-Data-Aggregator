
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.Export;

const string TicketsFolder = @"C:\MyDev\C#\01_Projects\Tickets-Data-Aggregator\Tickets-Data-Aggregator\Tickets-Data-Aggregator\tickets";

try
{
    var ticketsAggregator = new TicketsAggregator(
        TicketsFolder,
        new PdfReader());
    
    ticketsAggregator.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Sorry an unexpected error occurred: " + ex.Message);
}

public class TicketsAggregator
{
    private readonly string _ticketsFolder;
    private readonly IFileReader _fileReader;

    public TicketsAggregator(string ticketsFolder, IFileReader fileReader)
    {
        _ticketsFolder = ticketsFolder;
        _fileReader = fileReader;
    }

    public void Run()
    {     
        foreach (var filePath in Directory.GetFiles(_ticketsFolder, "*.pdf"))
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"File: " + Path.GetFileName(filePath));
            string text = _fileReader.Read(filePath);
            var split = text.Split(
                new[] { "Date", "Time", "Title" });
        }               
    }
}

public interface IFileReader
{
    public string Read(string directoryPath);
}

public class PdfReader : IFileReader
{
    public string Read(string directoryPath)
    {
        string fileContent = string.Empty;
        using PdfDocument document = PdfDocument.Open(directoryPath);                        
        for (int pageNumber = 1; pageNumber <= document.NumberOfPages; pageNumber++)
        {                
            Page page = document.GetPage(pageNumber);                
            fileContent += page.Text;                                
        }                    
        return fileContent;
    }
}