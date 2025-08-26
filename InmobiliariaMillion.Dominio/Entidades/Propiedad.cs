using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;

namespace InmobiliariaMillion.Dominio.Entidades
{
    public class Propiedad
    {
        public Propiedad()
        {

        }

        public Propiedad(ObjectId id, string idPropiedad, string nombre, decimal precio, string direccion, string codigoInterno, int anio, string idPropietario)
        {
            _id = id;
            IdPropiedad = idPropiedad ?? ObjectId.GenerateNewId().ToString();
            Nombre = nombre;
            Precio = precio;
            Direccion = direccion;
            CodigoInterno = codigoInterno;
            Anio = anio;
            IdPropietario = idPropietario;

            ActualizarInformacion(nombre, direccion, precio);
        }

        [BsonId]
        [BsonElement("_id")]
        public ObjectId _id { get; set; }

        public string IdPropiedad { get; set; }

        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de la propiedad no puede estar vacío");
                _nombre = value;
            }
        }

        private decimal _precio;
        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio de la propiedad debe ser mayor a cero");
                _precio = value;
            }
        }

        public string Direccion { get; set; }
        public string CodigoInterno { get; set; }
        public int Anio { get; set; }
        public string IdPropietario { get; set; }

        //Validaciones

        public decimal CalcularPrecioConImpuestos(decimal tasaImpuesto)
        {
            if (tasaImpuesto < 0 || tasaImpuesto > 1)
                throw new ArgumentException("La tasa de impuesto debe estar entre 0 y 1");

            return Precio * (1 + tasaImpuesto);
        }

        public void ActualizarInformacion(string nuevoNombre, string nuevaDireccion, decimal nuevoPrecio)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vacío");
            if (nuevoNombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener mas de 50 caracteres");
            if (string.IsNullOrWhiteSpace(nuevaDireccion))
                throw new ArgumentException("La dirección no puede estar vacía");
            if (nuevoPrecio <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor a cero");

            _nombre = nuevoNombre;
            _precio = nuevoPrecio;
        }
    }
}