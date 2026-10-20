namespace AutorizadorDTE.BackendAPI.Models
{
    public class Contribuyente
    {
        private string? nombre;
        private string? NIT;

        public Contribuyente(string? nombre, string? NIT)
        {
            this.nombre = nombre;
            this.NIT = NIT;
        }

        public void setNombre(string? nombre)
        {
            this.nombre = nombre;
        }
        public string? getNombre()
        {
            return this.nombre;
        }

        public void setNIT(string? NIT)
        {
            this.NIT = NIT;
        }
        public string? getNIT()
        {
            return this.NIT;
        }
    }
}