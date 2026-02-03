using Microsoft.Data.SqlClient;
using MvcPracticaFinalLinq.Models;
using System.Data;
using System.Diagnostics.Metrics;

namespace MvcPracticaFinalLinq.Repositories
{
    #region

//    alter VIEW V_MARTA
//AS
//SELECT
    //u.IDUSUARIO,
//    u.NOMBRE,
//    u.APELLIDOS,
//    u.EMAIL,
//    u.IMAGEN,

//    c.NOMBRE AS NombreCurso,

//    i.quiere_ser_capitan,

//    e.fecha_evento,

//    a.nombre AS NombreActividad

//FROM USUARIOSTAJAMAR u
//INNER JOIN CURSOSTAJAMAR c
//    ON c.IDCURSO = u.IDCURSO

//INNER JOIN INSCRIPCIONES i
//    ON i.id_usuario = u.IDUSUARIO

//INNER JOIN EVENTO_ACTIVIDADES ea
//    ON ea.IdEventoActividad = i.IdEventoActividad

//INNER JOIN EVENTOS e
//    ON e.id_evento = ea.IdEvento

//INNER JOIN ACTIVIDADES a
//    ON a.id_actividad = ea.IdActividad;

//    select* from V_MARTA

    #endregion
    public class RepositoryUsuarios
    {
        private DataTable tablaUsuarios;

        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryUsuarios()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COSASMARTES;User ID=sa;Trust Server Certificate=True";
            string sql = "select * from V_MARTA";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaUsuarios = new DataTable();
            adapter.Fill(this.tablaUsuarios);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Marta> GetUsuarios()
        {
            var consulta = from datos in this.tablaUsuarios.AsEnumerable() select datos;
            List<Marta> usuarios = new List<Marta>();
            foreach (var row in consulta)
            {
                Marta usuario = new Marta
                {
                    idUsuario = (int)row["IDUSUARIO"],
                    nombre = row["NOMBRE"].ToString(),
                    apellidos = row["APELLIDOS"].ToString(),
                    email = row["EMAIL"].ToString(),
                    imagen = row["IMAGEN"].ToString(),
                    nombreCurso = row["NombreCurso"].ToString(),
                    quiereSerCapitan = (bool)row["quiere_ser_capitan"],
                    fechaEvento = (DateTime)row["fecha_evento"],
                    nombreActividad = row["NombreActividad"].ToString()
                };
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        public Marta FindUsuario(int idusuario)
        {
            var consulta = from datos in this.tablaUsuarios.AsEnumerable()
                           where (int)datos["IDUSUARIO"] == idusuario
                           select datos;
            Marta usuario = new Marta
            {
                idUsuario = (int)consulta.FirstOrDefault()["IDUSUARIO"],
                nombre = consulta.FirstOrDefault()["NOMBRE"].ToString(),
                apellidos = consulta.FirstOrDefault()["APELLIDOS"].ToString(),
                email = consulta.FirstOrDefault()["EMAIL"].ToString(),
                imagen = consulta.FirstOrDefault()["IMAGEN"].ToString(),
                nombreCurso = consulta.FirstOrDefault()["NombreCurso"].ToString(),
                quiereSerCapitan = (bool)consulta.FirstOrDefault()["quiere_ser_capitan"],
                fechaEvento = (DateTime)consulta.FirstOrDefault()["fecha_evento"],
                nombreActividad = consulta.FirstOrDefault()["NombreActividad"].ToString()
            };
            return usuario;
        }
    }
}
