using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class VeiculoModel
    {
        private Guid id;
        private string cod_usuario;
        private string nome;
        private string descricao;
        private int tipo;
        private int tipo_caminhao;
        private int tipo_carroceria;
        private string placa;
        private string modelo;
        private string cor;
        private string obs;

        public Guid ID { get => id; set => id = value; }
        public string NOME { get => nome; set => nome = value; }
        public string DESCRICAO { get => descricao; set => descricao = value; }
        public int TIPO { get => tipo; set => tipo = value; }
        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public int TIPO_CAMINHAO { get => tipo_caminhao; set => tipo_caminhao = value; }
        public int TIPO_CARROCERIA { get => tipo_carroceria; set => tipo_carroceria = value; }
        public string PLACA { get => placa; set => placa = value; }
        public string MODELO { get => modelo; set => modelo = value; }
        public string OBS { get => obs; set => obs = value; }
        public string COR { get => cor; set => cor = value; }
    }

    public class vwVeiculo : VeiculoModel
    {
        private string dsc_tipo;
        public string DSC_TIPO { get => dsc_tipo; set => dsc_tipo = value; }
    }
}
