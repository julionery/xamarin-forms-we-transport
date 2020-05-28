using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class PessoaModel
    {
        private string cod_usuario;
        private string nome;
        private string email;
        private string telefone;
        private string cpf;
        private string tipo;
        private string cnh;

        public string COD_USUARIO { get => cod_usuario; set => cod_usuario = value; }
        public string NOME { get => nome; set => nome = value; }
        public string EMAIL { get => email; set => email = value; }
        public string TELEFONE { get => telefone; set => telefone = value; }
        public string CPF { get => cpf; set => cpf = value; }
        public string TIPO { get => tipo; set => tipo = value; }
        public string CNH { get => cnh; set => cnh = value; }
    }
}
