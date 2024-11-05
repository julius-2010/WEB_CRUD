using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWEB.Models
{
    public class ParametroFiltro
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string TipoMovimiento { get; set; }
        public string NroDocumento { get; set; }
    }
}