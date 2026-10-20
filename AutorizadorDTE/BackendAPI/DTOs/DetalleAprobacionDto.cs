namespace AutorizadorDTE.BackendAPI.Models
{
    public class DetalleAprovacionDto
    {
        private string? numeroFactura;
        private long? codigoAutorizacion;

        public void setNumeroFactura(string? numeroFactura)
        {
            this.numeroFactura = numeroFactura;
        }
        public string? getNumeroFactura()
        {
            return this.numeroFactura;
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