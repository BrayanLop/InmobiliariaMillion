using MongoDB.Bson;

namespace InmobiliariaMillion.Dominio.Entidades
{
    public class Propietario
    {
        public Propietario() { }

        public Propietario(ObjectId id, string idPropietario, string nombre, string direccion, string foto = null, DateTime? fechaNacimiento = null)
        {
            _id = id;
            IdPropietario = idPropietario ?? ObjectId.GenerateNewId().ToString();
            Nombre = nombre;
            Direccion = direccion;
            Foto = foto;
            FechaNacimiento = fechaNacimiento ?? DateTime.MinValue;

            ActualizarInformacion(nombre, direccion);
        }

        public ObjectId _id { get; set; }
        public string IdPropietario { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public void ActualizarInformacion(string nuevoNombre, string nuevaDireccion)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vacío");
            if (nuevoNombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener mas de 50 caracteres");
            if (string.IsNullOrWhiteSpace(nuevaDireccion))
                throw new ArgumentException("La dirección no puede estar vacía");
            if (FechaNacimiento > DateTime.Now)
                throw new ArgumentException("La fecha de nacimiento no puede ser mayor a la actual");

            Nombre = nuevoNombre;
            Direccion = nuevaDireccion;
        }
    }
}