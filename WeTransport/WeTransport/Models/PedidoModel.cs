using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class PedidoModel
    {
        private Guid id;
        private Guid cod_frete;
        private string cod_usuario;
        private string cod_prestador;
        private string descricao;
        private string obs;
        private string local_destino;
        private DateTime data_envio;
        private DateTime data_solicitacao;
        private int status;
        private bool inativo_user;
        private bool inativo_prestador;

        public Guid ID { get => id; set => id = value; }
        public Guid COD_FRETE { get => cod_frete; set => cod_frete = value; }
        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public string COD_PRESTADOR { get => cod_prestador; set => cod_prestador = value; }
        public int STATUS { get => status; set => status = value; }
        public string DESCRICAO { get => descricao; set => descricao = value; }
        public string OBS { get => obs; set => obs = value; }
        public string LOCAL_DESTINO { get => local_destino; set => local_destino = value; }
        public DateTime DATA_ENVIO { get => data_envio; set => data_envio = value; }
        public DateTime DATA_SOLICITACAO { get => data_solicitacao; set => data_solicitacao = value; }
        public bool INATIVO_USER { get => inativo_user; set => inativo_user = value; }
        public bool INATIVO_PRESTADOR { get => inativo_prestador; set => inativo_prestador = value; }
    }

    public class vwPedido : PedidoModel
    {
        private string dsc_status;
        private PessoaModel pessoa;
        private string dsc_cod_frete;
        private string dsc_cor;
        private vwFrete vwFrete;

        public string DSC_STATUS { get => dsc_status; set => dsc_status = value; }
        public PessoaModel Pessoa { get => pessoa; set => pessoa = value; }
        public string DSC_COD_FRETE { get => dsc_cod_frete; set => dsc_cod_frete = value; }
        public string DSC_COR { get => dsc_cor; set => dsc_cor = value; }
        public vwFrete FRETE { get => vwFrete; set => vwFrete = value; }
    }
}
