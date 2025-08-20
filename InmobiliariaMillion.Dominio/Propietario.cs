namespace InmobiliariaMillion.Dominio
{
    public class Propietario
    {
        public string IdPropietario { get; set; }
        
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public string FechaNacimiento { get; set; }
        
        public ICollection<Propiedad> Propiedades { get; set; }
        
        public void ActualizarInformacion(string nuevoNombre, string nuevaDireccion)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre del propietario es requerido");
                
            Nombre = nuevoNombre;
            Direccion = nuevaDireccion;
        }
    }
}