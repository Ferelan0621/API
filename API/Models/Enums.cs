using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace API.Models
{
    
    public enum EstadoLaboratorio
    {
        Disponible,
        Ocupado,
        Mantenimiento,
        Limpieza
    }

    public enum Rol
    {
        Administrador,
        Administrativo,
        Docente,
        Mantenimiento,
        Intendencia
    }
}
