using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcPracticaFinalLinq.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcPracticaFinalLinq.Repositories
{

#region Stored Procedures

//    create procedure SP_PLANTILLA_UPSERT
//(@hospitalcod int, @salacod int, @empleadono int, @apellido nvarchar(50), @funcion nvarchar(50), @turno nvarchar(50), @salario int)
//as
//    if exists(select* from PLANTILLA where EMPLEADO_NO= @empleadono)
//    begin
//        update PLANTILLA set HOSPITAL_COD=@hospitalcod, SALA_COD=@salacod, APELLIDO=@apellido, FUNCION=@funcion, T=@turno, SALARIO=@salario where EMPLEADO_NO=@empleadono
//    end
//    else
//    begin
//        insert into PLANTILLA(HOSPITAL_COD, SALA_COD, EMPLEADO_NO, APELLIDO, FUNCION, T, SALARIO) values(@hospitalcod, @salacod, @empleadono, @apellido, @funcion, @turno, @salario)
//    end
//go

#endregion
    public class RepositoryPlantilla
    {

        private DataTable tablaPlantilla;

        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPlantilla()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from PLANTILLA";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaPlantilla = new DataTable();
            adapter.Fill(this.tablaPlantilla);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Plantilla> GetPlantillas()
        {
            var consulta =
                from datos in this.tablaPlantilla.AsEnumerable()
                select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Plantilla> plantillas = new List<Plantilla>();
                foreach (var row in consulta)
                {
                    Plantilla pla = new Plantilla
                    {
                        hospitalcod = row.Field<int>("HOSPITAL_COD"),
                        salacod = row.Field<int>("SALA_COD"),
                        empleadono = row.Field<int>("EMPLEADO_NO"),
                        apellido = row.Field<string>("APELLIDO"),
                        funcion = row.Field<string>("FUNCION"),
                        turno = row.Field<string>("T"),
                        salario = row.Field<int>("SALARIO")
                    };
                    plantillas.Add(pla);
                }
                return plantillas;
            }
        }

        public Plantilla FindPlantilla(int empleadono)
        {
            var consulta = from datos in
                               this.tablaPlantilla.AsEnumerable()
                           where datos.Field<int>("EMPLEADO_NO") == empleadono
                           select datos;
            var row = consulta.First();
            Plantilla pla = new Plantilla
            {
                hospitalcod = row.Field<int>("HOSPITAL_COD"),
                salacod = row.Field<int>("SALA_COD"),
                empleadono = row.Field<int>("EMPLEADO_NO"),
                apellido = row.Field<string>("APELLIDO"),
                funcion = row.Field<string>("FUNCION"),
                turno = row.Field<string>("T"),
                salario = row.Field<int>("SALARIO")
            };
            return pla;
        }

        public void UpsertPlantilla(int hospitalcod, int salacod, int empleadono, string apellido, string funcion, string turno, int salario)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@hospitalcod", hospitalcod);
            this.com.Parameters.AddWithValue("@salacod", salacod);
            this.com.Parameters.AddWithValue("@empleadono", empleadono);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@funcion", funcion);
            this.com.Parameters.AddWithValue("@turno", turno);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeletePlantilla(int empleadono)
        {
            string sql = "delete from PLANTILLA where EMPLEADO_NO=@empleadono";
            this.com.Parameters.AddWithValue("@empleadono", empleadono);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.OpenAsync();
            this.com.ExecuteNonQueryAsync();
            this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
        public ResumenPlantilla GetPlantillaFuncion(string funcion)
        {
            var consulta = from datos in
                               this.tablaPlantilla.AsEnumerable()
                           where datos.Field<string>("FUNCION") == funcion
                           select datos;
            if (consulta.Count() == 0)
            {
                ResumenPlantilla model = new ResumenPlantilla
                {
                    Personas = 0,
                    MaximoSalario = 0,
                    MediaSalarial = 0,
                    Empleados = null
                };
                return model;
            }
            else
            {
                int personas = consulta.Count();
                int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
                double media = consulta.Average(x => x.Field<int>("SALARIO"));
                List<Plantilla> empleados = new List<Plantilla>();
                foreach (var row in consulta)
                {
                    Plantilla pla = new Plantilla
                    {
                        hospitalcod = row.Field<int>("HOSPITAL_COD"),
                        salacod = row.Field<int>("SALA_COD"),
                        empleadono = row.Field<int>("EMPLEADO_NO"),
                        apellido = row.Field<string>("APELLIDO"),
                        funcion = row.Field<string>("FUNCION"),
                        turno = row.Field<string>("T"),
                        salario = row.Field<int>("SALARIO")
                    };
                    empleados.Add(pla);
                }
                ResumenPlantilla model = new ResumenPlantilla
                {
                    Personas = personas,
                    MaximoSalario = maximo,
                    MediaSalarial = media,
                    Empleados = empleados
                };
                return model;
            }
        }

        public List<string> GetFunciones()
        {
            var consulta = (from datos in
                               this.tablaPlantilla.AsEnumerable()
                            select datos.Field<string>("FUNCION")).Distinct();
            return consulta.ToList();
        }
    }
}
