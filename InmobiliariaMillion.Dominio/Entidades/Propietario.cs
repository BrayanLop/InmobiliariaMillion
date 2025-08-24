using MongoDB.Bson;

namespace InmobiliariaMillion.Dominio.Entidades
{
    public class Propietario
    {
        public ObjectId _id { get; set; }
        public string IdPropietario { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public void ActualizarInformacion(string nuevoNombre, string nuevaDireccion)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vac�o");
            if (string.IsNullOrWhiteSpace(nuevaDireccion))
                throw new ArgumentException("La direcci�n no puede estar vac�a");

            Nombre = nuevoNombre;
            Direccion = nuevaDireccion;
        }
    }
}