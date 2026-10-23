namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class ConsultaContribuyenteDto
    {
        public string Nit { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public List<DetalleContribuyenteDto> Detalle { get; set; } = new();
        public int TotalFacturas { get; set; }
        public decimal MontoTotalRango { get; set; }
    }

    public class DetalleContribuyenteDto
    {
        public string Fecha { get; set; } = string.Empty;
        public int CantidadFacturas { get; set; }
        public decimal MontoTotal { get; set; }
    }

    public class ResumenIvaDto
    {
        public string Fecha { get; set; } = string.Empty;
        public List<DetalleIvaDto> Detalle { get; set; } = new();
    }

    public class DetalleIvaDto
    {
        public string Nit { get; set; } = string.Empty;
        public decimal IvaEmitido { get; set; }
        public decimal IvaRecibido { get; set; }
    }

    public class ResumenRangoDto
    {
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public string Modo { get; set; } = string.Empty;
        public List<DetalleResumenRangoDto> Detalle { get; set; } = new();
        public decimal TotalRango { get; set; }
    }

    public class DetalleResumenRangoDto
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }

    public class ReporteErroresDto
    {
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public List<DetalleReporteErroresDto> Detalle { get; set; } = new();
    }

    public class DetalleReporteErroresDto
    {
        public string Fecha { get; set; } = string.Empty;
        public int FacturasRecibidas { get; set; }
        public int NitEmisorInvalido { get; set; }
        public int NitReceptorInvalido { get; set; }
        public int NitEmisorInexistente { get; set; }
        public int NitReceptorInexistente { get; set; }
        public int IvaMalCalculado { get; set; }
        public int TotalMalCalculado { get; set; }
        public int ReferenciaDuplicada { get; set; }
        public int FacturasSinErrores { get; set; }
        public int EmisoresDistintos { get; set; }
        public int ReceptoresDistintos { get; set; }
    }
}
