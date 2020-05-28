using System;

namespace WeTransport.Models
{
    public class EstadoModel
    {
        private Guid id;
        private string nome;
        private string uf;

        public Guid ID { get => id; set => id = value; }
        public string NOME { get => nome; set => nome = value; }
        public string UF { get => uf; set => uf = value; }
    }
}
