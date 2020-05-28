using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public class CheckModal
    {
        private Guid id;
        private Guid cod_pedido;
        private DateTime dat_inclusao;
        private double latitude;
        private double longitude;

        public Guid ID { get => id; set => id = value; }
        public Guid COD_PEDIDO { get => cod_pedido; set => cod_pedido = value; }
        public DateTime DAT_INCLUSAO { get => dat_inclusao; set => dat_inclusao = value; }
        public double LATITUDE { get => latitude; set => latitude = value; }
        public double LONGITUDE { get => longitude; set => longitude = value; }
    }

}
