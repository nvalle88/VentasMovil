﻿using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
/// <summary>
/// En el APISERVICE se realizan los metodos que nos facilitan el consumo de el api,
/// estos metodos son tareas asincronas ya que dependemos de la respuesta del servicio en la nube
/// </summary>
namespace AppDemo.Services
{
    public class ApiService
    {
        DialogService dialogService;
        NavigationService navigationService;
        string URL_ws = Constants.Constants.WebServiceURL;

        public ApiService()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }
        public async Task<Response> Login(LoginRequest data)
        {
            try
            {               

                var request = JsonConvert.SerializeObject(data);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWEB);
                var url = "/api/webapi";
                var response = await client.PostAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }
                var request2 = JsonConvert.SerializeObject(new VendedorRequest { Correo = data.userid });
                var body2 = new StringContent(request2, Encoding.UTF8, "application/json");
                var client2 = new HttpClient();
                client2.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url2 = "api/Vendedores/VendedorbyEmail";
                var response2 = await client2.PostAsync(url2, body2);
                if (response2 != null)
                {

                    var result2 = await response2.Content.ReadAsStringAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "Login OK",
                        Result = JsonConvert.DeserializeObject<VendedorRequest>(result2)
                    };
                }

               

                return new Response
                {
                    IsSuccess = false,
                    Message = "error",
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }


        }
        #region cliente
        public async Task<Response> SetPhotoAsync(int multaId, Stream stream)
        {
            try
            {
                var array = ReadFully(stream);

                var photoRequest = new PhotoRequest
                {
                    Id = multaId,
                    Array = array,
                };

                var request = JsonConvert.SerializeObject(photoRequest);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Multas/SetFoto";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Foto asignada Ok",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);


                return ms.ToArray();
            }
        }
        #endregion
        public async Task<List<Cliente>> GetAllClients()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Clientes";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(result);
                return clientes;
            }
            catch (Exception)
            {
                return null;
            }
        }
        //api/Vendedores/VendedorbyEmail
        public async Task<List<ClienteRequest>> GetMyClient()
        {
            try
            {
                var me = new ClienteRequest {
                IdVendedor=Settings.userId,
                IdEmpresa=Settings.companyId};

                var request = JsonConvert.SerializeObject(me);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Clientes/ListarClientesPorVendedor";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<ClienteRequest>>(result);
                return clientes;
            }
            catch(Exception ex)
            {
                return null;
            }
        }            
        public async Task<List<TipoCompromiso>> GetTipoCompromiso()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/TipoCompromisoes";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var Lista = JsonConvert.DeserializeObject<List<TipoCompromiso>>(result);
                return Lista;
            }
            catch
            {
                return null;
            }

        }
        /// <summary>
        /// Obtiene los clientes cercanos
        /// </summary>
        /// <param name="position"></param>
        /// <param name="Radio"></param>
        /// <returns></returns>
        public async Task<List<Cliente>> GetNearClients(Helpers.GeoUtils.Position position, double Radio)
        {
            try
            {
                var ncr = new NearClientRequest
                {
                    Position = position,
                    myId = Settings.userId,     
                    radio= Radio,                    
                };
                var request = JsonConvert.SerializeObject(ncr);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Clientes/GetNearClients";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(result);
                return clientes;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ObservableCollection<PinRequest>> GetParqueados()
        {
            try
            {
                var request = JsonConvert.SerializeObject(navigationService.GetAgenteActual());
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Parqueos/GetParqueados";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var parqueados = JsonConvert.DeserializeObject<ObservableCollection<PinRequest>>(result);
                return parqueados;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<Response> postNewClient(ClienteRequest cliente)
        {

            try
            {

                var request = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Clientes/InsertarCliente";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    new Response
                    {
                        IsSuccess = false,
                        Message = "error",

                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                var cliente_ = JsonConvert.DeserializeObject<Cliente>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = cliente_
                };
            }
            catch (Exception)
            {
                return null;
                throw;
            }


        }
        public async Task<List<TipoCliente>> GetClientTypes()
        {
            Empresa empresa = new Empresa
            {
                IdEmpresa = Settings.companyId
            };

            try
            {
                var request = JsonConvert.SerializeObject(empresa);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/TiposClientes/ListarTiposClientes";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var tipoclientes = JsonConvert.DeserializeObject<List<TipoCliente>>(result);
                return tipoclientes;
            }
            catch
            {
                return null;
            }



        }
        public async Task<List<PinRequest>> Getparking()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Parqueos/GetParqueados";
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var parqueos = JsonConvert.DeserializeObject<List<PinRequest>>(result);
                return parqueos;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Position>> GetMyPolygon(int _agenteId)
        {
            try
            {
                Agente _agente = new Agente { Id = _agenteId };
                var request = JsonConvert.SerializeObject(_agente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Sectors/GetMyPolygon";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    List<Position> position = new List<Position>();
                    return position;
                }
                var result = await response.Content.ReadAsStringAsync();
                var PuntoSector = JsonConvert.DeserializeObject<List<PuntoSector>>(result);
                List<Position> MyPolygon = new List<Position>();
                foreach (var punto in PuntoSector)
                    MyPolygon.Add(new Position(punto.Latitud, punto.Longitud));
                return MyPolygon;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task PostLogPosition(LogPosition position)
        {
            try
            {
                var request = JsonConvert.SerializeObject(position);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/LogPositions";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return;
                }
                var result = await response.Content.ReadAsStringAsync();
                return;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            

            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async Task<Response> Checkin(CheckinRequest visita)
        {
            try
            {
                var request = JsonConvert.SerializeObject(visita);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Vista/Insertar";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "error",
                    }; ;
                }
                var result = await response.Content.ReadAsStringAsync();
                var visitadata= JsonConvert.DeserializeObject<Response>(result);
                return visitadata;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "",
                };
            }
        }
        public async Task<Response> SetFileAsync(string Id, int Tipo, Stream stream)
        {
            try
            {
                var array = ReadFully(stream);

                var archivoRequest = new ArchivoRequest
                {
                    Id = Id,
                    Tipo=Tipo,
                    Array = array,
                };

                var request = JsonConvert.SerializeObject(archivoRequest);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "Api/Archivos/Insertar";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var imagen = JsonConvert.DeserializeObject<Response>(result);
             //   var ruta = JsonConvert.DeserializeObject <string>(imagen.Result.ToString());

                return imagen;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Response> SubirInforme(Informe informe)
        {
            try
            {
                var request = JsonConvert.SerializeObject(informe);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(URL_ws);
                var url = "simed/api/Informes";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "error",
                    }; ;
                }
                var result = await response.Content.ReadAsStringAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                }; ;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "",
                };
            }
        }
        public async Task<Response> ClienteData(ClienteRequest cliente)
        {
            try
            {
                var request = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Clientes/DatosCliente";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "error",
                    }; ;
                }
                var result = await response.Content.ReadAsStringAsync();
                var clientedata = JsonConvert.DeserializeObject<Response>(result);
                return clientedata;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "",
                };
            }
        }
        public async Task<EstadisticosClienteRequest> DatosEstadisticos(ClienteRequest cliente)
        {
            try
            {
                var request = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Clientes/VerEstadisticosCliente";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null ;
                }
                var result = await response.Content.ReadAsStringAsync();
                var statiticsdata = JsonConvert.DeserializeObject<EstadisticosClienteRequest>(result);
                return statiticsdata;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Response>ActualizarCompromiso(Compromiso compromiso)
        {
            try
            {
                var request = JsonConvert.SerializeObject(compromiso);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Compromiso/ActualizarCompromiso";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Actualizado",

                };
               
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<EventoRequest>> AgendaPorVendedor()
        {
            try
            {
                var vendedor = new VendedorRequest
                {
                    IdVendedor = Settings.userId,
                };
                
                var request = JsonConvert.SerializeObject(vendedor);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Agendas/ListarEventosPorVendedor";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var result = await response.Content.ReadAsStringAsync();
                var AgendaData = JsonConvert.DeserializeObject<List<EventoRequest>>(result);
                return AgendaData;

                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public async Task<Response> AgregarAgenda(Agenda agenda)
        {
            try
            {
                var request = JsonConvert.SerializeObject(agenda);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Agendas/Agregar";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "error",
                    }; ;
                }

                var result = await response.Content.ReadAsStringAsync();
                var agendadata = JsonConvert.DeserializeObject<Response>(result);


                return agendadata;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "",
                };
            }
        }
        public async Task LogRuta(LogRutaVendedor Ruta)
        {
            try
            {
                var request = JsonConvert.SerializeObject(Ruta);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/LogRutaVendedors";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Error");
                }
             
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        public async Task<Response> VerificarCodigo(RecuperarContrasenaRequest Codigo)
        {
            try
            {
                var request = JsonConvert.SerializeObject(Codigo);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Usuarios/VerificarCodigo";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "error",
                    }; ;
                }

                var result = await response.Content.ReadAsStringAsync();
                var codedata = JsonConvert.DeserializeObject<Response>(result);


                return codedata;
                //  var log = JsonConvert.DeserializeObject<LogPosition>(result);            
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "",
                };
            }

        }
        public async Task<Response> PasswordChange(RecuperarContrasenaRequest data)
        {
            try
            {

                var request = JsonConvert.SerializeObject(data);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWEB);
                var url = "/api/webapi/CambiarContraseña";
                var response = await client.PostAsync(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var PasswordData = JsonConvert.DeserializeObject<Response>(result);
                return PasswordData;

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }


        }

        public async Task<DistanciaRequest> GetDistancia(DistanciaRequest distanciaRequest)
        {
            try
            {
                var request = JsonConvert.SerializeObject(distanciaRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(Constants.Constants.VentasWS);
                var url = "api/Vendedores/DistanciaVendedor";
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new DistanciaRequest
                    {
                        isSet=false,
                        DistanciaSeguimiento=0.1                       
                    }; ;
                }
                var result = await response.Content.ReadAsStringAsync();
                var distancia = JsonConvert.DeserializeObject<Response>(result);
                return (DistanciaRequest) distancia.Resultado;

            }
            catch (Exception ex)
            {
                return new DistanciaRequest
                {
                    isSet = false,
                    DistanciaSeguimiento = 0.1,
                };
            }

        }



    }
}