using AppDemo.Classes;
using AppDemo.Helpers;
using System;
using System.Threading.Tasks;
/// <summary>
/// SignalR nos ayuda a mantener el realtime en mensajes pero lo utilizamos para enviar nuestra posicion actual
/// </summary>
namespace AppDemo.Services
{
    public class SignalRService
    {
        public static SignalRClient SignalRClient = new SignalRClient(Constants.Constants.SignalRWeb);
        DialogService dialogService = new DialogService();
        /// <summary>
        /// esta tarea permite enviar la posicion segun los parametros de latitud y longitud
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public async Task SendPosition(float lat, float lon)
        {
            await SignalRClient.Start().ContinueWith(task =>
                 {
                     if (task.IsFaulted)
                         dialogService.ShowMessage("Error", "Error al enviar datos de posición en tiempo real: " + task.Exception.InnerExceptions[0].Message);
                 }
                   );
            LivePositionRequest lpr = new LivePositionRequest
            {
                EmpresaId = Settings.companyId,
                AgenteId = Settings.userId,
                Nombre = Settings.UserName,
                fecha = DateTime.Now,
                Lat = lat,
                Lon = lon
            };
            SignalRClient.SendMessage(lpr);
        }
    }
}