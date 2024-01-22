using ProEventos.Domain.Model;

namespace ProEventos.Domain.Model
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }
        public int EventoId { get; set; }
        public Palestrante Palestrante { get; set; }
        public Evento Evento { get; set; }
    }
}