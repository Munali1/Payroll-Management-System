using DinkToPdf;
using DinkToPdf.Contracts;
using Payroll.Application.Interfaces;


namespace Payroll.Infrastructure.ExternalServices
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConverter converter;

        public PdfGenerator(IConverter converter)
        {
            this.converter = converter;
        }
        public byte[] GeneratePdfFromHtml(string html)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = html
                }
            }
            };

            return converter.Convert(doc);
        }
    }
    }

