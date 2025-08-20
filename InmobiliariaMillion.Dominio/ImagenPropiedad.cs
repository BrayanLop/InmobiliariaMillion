namespace InmobiliariaMillion.Dominio
{
    public class ImagenPropiedad
    {
        public string IdImagenPropiedad { get; set; }
        
        public string IdPropiedad { get; set; }
        
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }
        
        public Propiedad Propiedad { get; set; }
        
        public void Habilitar()
        {
            Habilitada = true;
        }
        
        public void Deshabilitar()
        {
            Habilitada = false;
        }
    }
}