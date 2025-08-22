namespace InmobiliariaMillion.Dominio.Entidades
{
    public class Propiedad
    {
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

        public void ActualizarPrecio(decimal nuevoPrecio, string razon)
        {
            if (nuevoPrecio <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor a cero");

            if (string.IsNullOrWhiteSpace(razon) && nuevoPrecio != _precio)
                throw new ArgumentException("El cambio de precio requiere una razón");

            _precio = nuevoPrecio;
        }

        public bool EstaDisponible()
        {
            return true;
        }

        public decimal CalcularPrecioConImpuestos(decimal tasaImpuesto)
        {
            if (tasaImpuesto < 0 || tasaImpuesto > 1)
                throw new ArgumentException("La tasa de impuesto debe estar entre 0 y 1");

            return Precio * (1 + tasaImpuesto);
        }
    }
}