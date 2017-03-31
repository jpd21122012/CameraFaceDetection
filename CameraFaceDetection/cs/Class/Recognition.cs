using Microsoft.ProjectOxford.Face;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using CameraFaceDetection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FaceVerification.Class { 
public class Recognition
{
        public static string idlist = "";
        public static string httpidlist = "";
        public static string idReturned = "";
        public static double idconfReturned;
        public string responsejson ="";
        public double responseconf = 0;
        ConexionBD bd = new ConexionBD();
        public async Task HttpFindSimilarAsync(string fileLocation)
        {
            Debug.WriteLine("Entered in HTTPFIND...");
            using (var fileStream = File.OpenRead(fileLocation))
            {
                try
                {
                    var client1 = new FaceServiceClient("12476023b4c349939778c49e5db321d6");
                    var faces = await client1.DetectAsync(fileStream, true);
                    Debug.WriteLine(" > " + faces.Length + " detected.");
                    Debug.WriteLine(" >> IdActual: " + faces[0].FaceId.ToString());
                    httpidlist = faces[0].FaceId.ToString();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
                fileStream.Dispose();
            }
            try
            {
               Guid idGuid= Guid.Parse(httpidlist);
                var client1 = new FaceServiceClient("12476023b4c349939778c49e5db321d6");
                var faces = await client1.FindSimilarAsync(idGuid, "21122012", 1);
                Debug.WriteLine(" >> IdActual: " + faces[0].PersistedFaceId.ToString());
                Debug.WriteLine(" >> Conf: " + faces[0].Confidence.ToString());
                idReturned = faces[0].PersistedFaceId.ToString();
                string dou =faces[0].Confidence.ToString();
                idconfReturned= Double.Parse(dou);
                //Evaluar si es mayor a .7 entonces regresa los datos de la persona
                if (idconfReturned >= .67)
                {
                    Debug.WriteLine("Usuario encontrado");
                    // busqueda en la base de datos
                    Debug.WriteLine("Entrando a metodo bd.visualizar info de sujeto \n");
                    //Descomentarbd.VisualizarInfo(idReturned);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }


        }

        //PersistentIdList pil = new PersistentIdList();
        //    public async void DetecFacesAndDisplayResult(string fileLocation, string subscriptionKey,string name,int edad)
        //{
        //    using (var fileStream = File.OpenRead(fileLocation))
        //    {
        //        try
        //        {
        //            var client = new FaceServiceClient(subscriptionKey);
        //            var faces = await client.DetectAsync(fileStream, true);
        //            Console.WriteLine(" > " + faces.Length + " detected.");
        //                  Console.WriteLine(" >> Id: " + faces[0].FaceId.ToString());
        //                GuardarInfo(name,edad, faces[0].FaceId.ToString());
        //                Comparer.idActual = faces[0].FaceId.ToString();
        //            }
        //            catch (Exception exception)
        //        {
        //            Console.WriteLine(exception.ToString());
        //        }
        //            fileStream.Close();
        //        }
        //       // pil.AddListId("Image.jpg", "12476023b4c349939778c49e5db321d6", name, edad);
        //    }
    }
}