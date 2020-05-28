using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Models
{
    public enum MenuItemType
    {
        Check,
        Frete,
        Notificacao,
        Pedido,
        Perfil,
        Sobre,
        Start,
        Veiculo
    }
    public class MenuItemModel
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
