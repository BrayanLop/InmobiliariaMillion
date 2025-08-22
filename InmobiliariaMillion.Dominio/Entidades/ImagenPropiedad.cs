namespace InmobiliariaMillion.Dominio.Entidades
{
    public class ImagenPropiedad
    {
        public string IdImagenPropiedad { get; set; }
        public string IdPropiedad { get; set; } // Solo el ID, sin navegación
        public string Archivo { get; set; }
        public bool Habilitada { get; set; }

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