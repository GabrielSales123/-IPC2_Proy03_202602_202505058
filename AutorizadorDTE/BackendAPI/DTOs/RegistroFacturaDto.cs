namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class RegistroFacturaDto
    {
        public string TiempoOriginal { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public string NitEmisor { get; set; } = string.Empty;
        public string NitReceptor { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public bool Autorizada { get; set; }
        public long? CodigoAutorizacion { get; set; }
        public List<string> Errores { get; set; } = new();
    }
}
