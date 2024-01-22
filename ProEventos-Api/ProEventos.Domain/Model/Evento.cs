using System;
using ProEventos.Domain.Model;

namespace Proeventos.Domain.Model
{
    public class Evento
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime? DataEvento { get; set; }
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string ImagemUrl { get; set; }
        public IEquatable<Lote> Lote { get; set; }
        public IEquatable<RedeSocial> RedesSociais { get; set; }
        public IEquatable<PalestranteEvento> PalestrantesEventos { get; set; }
 
    }
}