namespace API.Services 
{
    public class NotificadorLaboratorios
    {
        public event Action<string>? OnLaboratorioActualizado;
        public void NotificarCambio(string jsonLaboratorios) => OnLaboratorioActualizado?.Invoke(jsonLaboratorios);
    }
    public class NotificadorPrestamos
    {
        public event Action<string>? OnPrestamosActualizado;
        public void NotificarCambio(string jsonLaboratorios) => OnPrestamosActualizado?.Invoke(jsonLaboratorios);
    }
}