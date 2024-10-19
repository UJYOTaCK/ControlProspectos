using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlProspectos.Entidades
{
    public class Prospecto
    {
        public int prospectoId { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string calle { get; set; }
        public string numero { get; set; }
        public string colonia { get; set; }
        public string codigoPostal { get; set; }
        public string telefono { get; set; }
        public string rfc { get; set; }
        public int estatus { get; set; }
        public string observaciones { get; set; }
        public int ultimaAct { get; set; }
    }
}
