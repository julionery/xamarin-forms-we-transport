using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class ChatModal
    {
        private Guid id;
        private Guid cod_frete;
        private string cod_usuario;
        private string cod_prestador;

        public Guid ID { get => id; set => id = value; }
        public Guid COD_FRETE { get => cod_frete; set => cod_frete = value; }
        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public string COD_PRESTADOR { get => cod_prestador; set => cod_prestador = value; }
    }

    public class vwChat : ChatModal
    {
        private PessoaModel pessoa;
        private string dsc_cod_frete;

        public PessoaModel Pessoa { get => pessoa; set => pessoa = value; }
        public string DSC_COD_FRETE { get => dsc_cod_frete; set => dsc_cod_frete = value; }
    }
}
