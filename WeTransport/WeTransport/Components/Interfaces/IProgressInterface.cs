namespace WeTransport.Components.Interfaces
{
    public interface IProgressInterface
    {
        void Show(string tittle = "Carregando...");
        void ShowToast(string tittle = "Carregando...");
        void Hide();
    }
}
