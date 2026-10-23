namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class SolicitudFacturaDto
    {
        public string TIEMPO { get; set; } = string.Empty;
        public string REFERENCIA { get; set; } = string.Empty;
        public string NIT_EMISOR { get; set; } = string.Empty;
        public string NIT_RECEPTOR { get; set; } = string.Empty;
        public decimal VALOR { get; set; }
        public decimal IVA { get; set; }
        public decimal TOTAL { get; set; }
    
    }
}