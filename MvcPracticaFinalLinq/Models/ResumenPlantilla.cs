namespace MvcPracticaFinalLinq.Models
{
    public class ResumenPlantilla
    {
        public int Personas { get; set; }
        public int MaximoSalario { get; set; }
        public int SumaSalarios { get; set; }
        public double MediaSalarial { get; set; }
        public List<Plantilla> Empleados { get; set; }
    }
}
