using System;
using System.Collections.Generic;
using System.Text;

namespace WeTransport.Validations
{
    public class ErrorValidator
    {
        public static string ValidaErrosAuth(string msg)
        {
            string error = "";

            if (msg.Equals("The email address is badly formatted."))
                error = "E-mail inválido.";

            if (msg.Equals("The password is invalid or the user does not have a password."))
                error = "E-mail ou senha inválidos!";

            if (msg.Equals("The email address is already in use by another account."))
                error = "Este e-mail já está em uso para outro usuário!";
            
            if (msg.Equals("There is no user record corresponding to this identifier. The user may have been deleted."))
                error = "E-mail não cadastrado!";

            if (error.Equals(""))
                error = msg;

            return error;
        }
    }
}
