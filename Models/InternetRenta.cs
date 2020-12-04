namespace AspNet5InternetRenta.Models
{
    public class InternetRenta
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public System.DateTime FechaCorte {get;set;}
        public int Cantidad {get;set;}
        public bool Cortado {get;set;}
    }
}
