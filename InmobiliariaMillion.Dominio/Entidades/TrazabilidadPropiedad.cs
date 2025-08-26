using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace InmobiliariaMillion.Dominio.Entidades
{
    public class TrazabilidadPropiedad
    { 
        public TrazabilidadPropiedad() {}   

        public TrazabilidadPropiedad(ObjectId id, string idTrazabilidadPropiedad, DateTime fechaVenta, string nombre, decimal valor, decimal impuesto, string idPropiedad)
        {
            _id = id;
            IdTrazabilidadPropiedad = idTrazabilidadPropiedad ?? ObjectId.GenerateNewId().ToString();
            FechaVenta = fechaVenta;
            Nombre = nombre;
            Valor = valor;
            Impuesto = impuesto;
            IdPropiedad = idPropiedad;

            ActualizarInformacion(fechaVenta, nombre, valor);
        }

        public ObjectId _id { get; set; }
        public string IdTrazabilidadPropiedad { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
        public decimal Impuesto { get; set; }
        public string IdPropiedad { get; set; } // Solo el ID, sin navegación

        public void ActualizarInformacion(DateTime fechaVenta, string nuevoNombre, decimal nuevoValor)
        {
            if (string.IsNullOrWhiteSpace(nuevoNombre))
                throw new ArgumentException("El nombre no puede estar vacío");
            if (nuevoNombre.Length > 50)
                throw new ArgumentException("El nombre no puede tener mas de 50 caracteres");
            if (nuevoValor <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor a cero");
            if (fechaVenta > DateTime.Now)
                throw new ArgumentException("La fecha de venta no puede superior a la actual");
        }

    }
}