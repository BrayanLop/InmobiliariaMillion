using Microsoft.AspNetCore.Mvc;
using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;

namespace InmobiliariaMillion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TrazabilidadPropiedadController : ControllerBase
    {
        private readonly ITrazabilidadPropiedadServicio _trazabilidadPropiedadServicio;

        public TrazabilidadPropiedadController(ITrazabilidadPropiedadServicio trazabilidadPropiedadServicio)
        {
            _trazabilidadPropiedadServicio = trazabilidadPropiedadServicio;
        }


    }
}