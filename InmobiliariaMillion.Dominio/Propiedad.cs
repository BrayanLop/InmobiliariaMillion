namespace InmobiliariaMillion.Dominio
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
        public int Año { get; set; }
        
        public string IdPropietario { get; set; }
        
        public Propietario Propietario { get; set; }
        
        public ICollection<ImagenPropiedad> ImagenesPropiedad { get; set; }
        public ICollection<TrazabilidadPropiedad> TrazabilidadesPropiedad { get; set; }

        
        // Métodos de dominio con reglas de negocio
        public void ActualizarPrecio(decimal nuevoPrecio, string razon)
        {
            if (nuevoPrecio <= 0)
                throw new ArgumentException("El nuevo precio debe ser mayor a cero");
                                
            if (string.IsNullOrWhiteSpace(razon) && nuevoPrecio != _precio)
                throw new ArgumentException("El cambio de precio requiere una razón");
                
            _precio = nuevoPrecio;
        }
        
        public void AgregarImagen(ImagenPropiedad imagen)
        {
            if (ImagenesPropiedad?.Count >= 10)
                throw new InvalidOperationException("La propiedad no puede tener más de 10 imágenes");
                
            ImagenesPropiedad ??= new List<ImagenPropiedad>();
            ImagenesPropiedad.Add(imagen);
        }
        
        public void EliminarImagen(string idImagen)
        {
            if (ImagenesPropiedad == null) return;
            
            var imagen = ImagenesPropiedad.FirstOrDefault(i => i.IdImagenPropiedad == idImagen);
            if (imagen != null)
            {
                ImagenesPropiedad.Remove(imagen);
            }
        }
        
        public void RegistrarVenta(decimal valorVenta, decimal impuesto, string nombreComprador)
        {
            if (valorVenta <= 0)
                throw new ArgumentException("El valor de venta debe ser mayor a cero");
                
            if (string.IsNullOrWhiteSpace(nombreComprador))
                throw new ArgumentException("El nombre del comprador es requerido");
                
            var trazabilidad = new TrazabilidadPropiedad
            {
                IdTrazabilidadPropiedad = Guid.NewGuid().ToString(),
                FechaVenta = DateTime.Now,
                Nombre = nombreComprador,
                Valor = valorVenta,
                Impuesto = impuesto,
                IdPropiedad = this.IdPropiedad
            };
            
            TrazabilidadesPropiedad ??= new List<TrazabilidadPropiedad>();
            TrazabilidadesPropiedad.Add(trazabilidad);
        }
        
        public bool EstaDisponible()
        {
            // Lógica de negocio: una propiedad está disponible si no tiene ventas recientes
            return TrazabilidadesPropiedad?.Any(t => t.FechaVenta > DateTime.Now.AddDays(-30)) != true;
        }
        
        public decimal CalcularPrecioConImpuestos(decimal tasaImpuesto)
        {
            if (tasaImpuesto < 0 || tasaImpuesto > 1)
                throw new ArgumentException("La tasa de impuesto debe estar entre 0 y 1");
                
            return Precio * (1 + tasaImpuesto);
        }
    }
}