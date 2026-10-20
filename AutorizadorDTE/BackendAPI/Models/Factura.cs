namespace AutorizadorDTE.BackendAPI.Models
{
    public class Factura
    {
        private string? numeroFactura;
        private DateTime? fecha;
        private Contribuyente? NITemisor;
        private Contribuyente? NITreceptor;
        private decimal? valor;
        private decimal? IVA;
        private decimal? total;
        private long? codigoAutorizacion;
        private bool? autorizada;

        public Factura(string? numeroFactura, DateTime? fechaEmision, Contribuyente? 
        NITemisor, Contribuyente? NITreceptor, decimal valor, decimal IVA, 
        decimal total)
        {
            this.numeroFactura = numeroFactura;
            this.fecha = fechaEmision ?? DateTime.Now;
            this.NITemisor = NITemisor;
            this.NITreceptor = NITreceptor;
            this.valor = valor;
            this.IVA = IVA;
            this.total = total;
            this.codigoAutorizacion = null;
            this.autorizada = null;
        }

        public void setNumeroFactura(string? numeroFactura)
        {
            this.numeroFactura = numeroFactura;
        }
        public string? getNumeroFactura()
        {
            return this.numeroFactura;
        }

        public void setFechaEmision(DateTime? fechaEmision)
        {
            this.fecha = fechaEmision ?? DateTime.Now;
        }
        public DateTime? getFechaEmision()
        {
            return this.fecha;
        }

        public void setEmisor(Contribuyente? emisor)
        {
            this.NITemisor = emisor;
        }
        public Contribuyente? getEmisor()
        {
            return this.NITemisor;
        }

        public void setReceptor(Contribuyente? receptor)
        {
            this.NITreceptor = receptor;
        }
        public Contribuyente? getReceptor()
        {
            return this.NITreceptor;
        }

        public void setValor(decimal valor)
        {
            this.valor = valor;
        }
        public decimal? getValor()
        {
            return this.valor;
        }

        public void setIVA(decimal IVA)
        {
            this.IVA = IVA;
        }
        public decimal? getIVA()
        {
            return this.IVA;
        }

        public void setTotal(decimal total)
        {
            this.total = total;
        }
        public decimal? getTotal()
        {
            return this.total;
        }

        public void setAutorizada(bool autorizada)
        {
            this.autorizada = autorizada;
        }
        public bool? getAutorizada()
        {
            return this.autorizada;
        }
        public void setCodigoAutorizacion(long? codigoAutorizacion)
        {
            this.codigoAutorizacion = codigoAutorizacion;
        }
        public long? getCodigoAutorizacion()
        {
            return this.codigoAutorizacion;
        }
    }
}