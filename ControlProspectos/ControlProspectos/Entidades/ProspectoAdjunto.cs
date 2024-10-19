using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlProspectos.Entidades
{
    public class ProspectoAdjunto
    {
        public int prospectoAdjuntoId { get; set; }
        public int prospectoId { get; set; }
        public int tipoDocumentoId { get; set; }
        public string tipoDocumento { get; set; }
        public string pathFile { get; set; }
        public int estatus { get; set; }
        public DateTime fechaCreacion { get; set; }
        public byte[] fileUpload { get; set; }
        public int ultimaAct { get; set; }
    }
}
