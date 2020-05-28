using System;

namespace WeTransport.Models
{
    public class FreteModel
    {
        private Guid id;
        private string cod_usuario;
        private VeiculoModel veiculo;
        private string cidade_partida;
        private string cidade_destino;
        private EstadoModel estado_partida;
        private EstadoModel estado_destino;
        private double valor;
        private int tipo;
        private int disponibilidade;
        private DateTime disponibilidade_inicial;
        private DateTime disponibilidade_final;
        private string obs;
        private int status;

        public Guid ID { get => id; set => id = value; }
        public int TIPO { get => tipo; set => tipo = value; }
        public string OBS { get => obs; set => obs = value; }
        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public VeiculoModel VEICULO { get => veiculo; set => veiculo = value; }
        public string CIDADE_PARTIDA { get => cidade_partida; set => cidade_partida = value; }
        public string CIDADE_DESTINO { get => cidade_destino; set => cidade_destino = value; }
        public double VALOR { get => valor; set => valor = value; }
        public int DISPONIBILIDADE { get => disponibilidade; set => disponibilidade = value; }
        public DateTime DISPONIBILIDADE_INICIAL { get => disponibilidade_inicial; set => disponibilidade_inicial = value; }
        public DateTime DISPONIBILIDADE_FINAL { get => disponibilidade_final; set => disponibilidade_final = value; }
        public EstadoModel ESTADO_PARTIDA { get => estado_partida; set => estado_partida = value; }
        public EstadoModel ESTADO_DESTINO { get => estado_destino; set => estado_destino = value; }
        public int STATUS { get => status; set => status = value; }
    }

    public class vwFrete : FreteModel
    {
        private string dsc_status;
        private string dsc_tipo;
        private string dsc_disponibilidade;
        private string dsc_valor;
        private string dsc_cor;
        private string dsc_tipo_veiculo;
        private string dsc_cod_frete;

        public string DSC_STATUS { get => dsc_status; set => dsc_status = value; }
        public string DSC_TIPO { get => dsc_tipo; set => dsc_tipo = value; }
        public string DSC_DISPONIBILIDADE { get => dsc_disponibilidade; set => dsc_disponibilidade = value; }
        public string DSC_VALOR { get => dsc_valor; set => dsc_valor = value; }
        public string DSC_COR { get => dsc_cor; set => dsc_cor = value; }
        public string DSC_TIPO_VEICULO { get => dsc_tipo_veiculo; set => dsc_tipo_veiculo = value; }
        public string DSC_COD_FRETE { get => dsc_cod_frete; set => dsc_cod_frete = value; }
    }
}
