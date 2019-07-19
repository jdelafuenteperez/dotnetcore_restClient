using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace feriadosapp
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Ejemplo -> dotnet core 2.2 Cliente API Rest");
            ProccessRepositories().Wait();
        }


        private static async Task ProccessRepositories()
        {
            //Creamos el objecto que convertirá los datos
            var serializer = new DataContractJsonSerializer(typeof(List<Feriado>)
            ,new DataContractJsonSerializerSettings{

                //Configuramos el Serializador para que pueda convertir el formato de fecha "yyyy-MM-dd" a un objeto DateTime
                DateTimeFormat = new System.Runtime.Serialization.DateTimeFormat("yyyy-MM-dd")
            });

            //Obtenemos el stream de datos desde el API Rest de forma asincrona
            var stream = client.GetStreamAsync("https://feriados-cl-api.herokuapp.com/feriados/");

            //Convertimos con el Serializador el stream de datos a una lista objetos 'Feriado'
            var feriados =serializer.ReadObject(await stream) as List<Feriado>;

            int counter =0;

            //Imprimimos los feriados en pantalla.
            foreach(var f in feriados)
            {
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("                        Feriado Número " + (++counter));
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine(" - Nombre : "+f.title);
                Console.WriteLine(" - Tipo : "+f.extra);
                Console.WriteLine(" - Fecha : "+f.date.ToString("dd-MM-yyyy"));
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine(Environment.NewLine);
            }
        }
    }

    public class Feriado
    {
        /*
        *   La clase feriado la utilizaremos para convertir los datos recibidos desde el 
        *   API Rest a un objeto en C#
        */
        
        public DateTime date;
        public string title;
        public string extra;
    }
}
