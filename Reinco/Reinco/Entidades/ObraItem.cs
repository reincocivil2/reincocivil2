﻿using Reinco.Interfaces.Obra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Reinco.Recursos;

namespace Reinco.Entidades
{
    public class ObraItem
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        public int idObra { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public int idPropietario { get; set; }
        public int idUsuario { get; set; }
        public string plantilla { get; set; }
        public string colorObra { get; set; }
        public int idPropietarioObra { get; set; }
        public int idPlantillaObra { get; set; }



        #region +---- Comandos ---+
        public ICommand asignarPlantilla { get; private set; }
        public ICommand editarObra { get; private set; }
        public ICommand eliminar { get; private set; }
        
        #endregion


        #region +---- Constructor ----+
        public ObraItem()
        {
            asignarPlantilla = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new AsignarPlantilla(this.idObra, this.nombre));
            });

            editarObra = new Command(() =>
            {
                App.ListarObra.Navigation.PushAsync(new AgregarObra(this.idObra, this.codigo, this.nombre, 
                    this.idPropietario,this.idUsuario,this.idPropietarioObra));
            });
            eliminar = new Command(async() =>
            {
                // Eliminar logica de programacion aqui
                try
                {
                    bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "Eliminar idObra = " + idObra, "Aceptar", "Cancelar");
                    object[,] variables = new object[,] { { "idPropietarioObra", idPropietarioObra }, { "idObra", idObra } };
                    dynamic result = await Servicio.MetodoGetString("ServicioPropietarioObra.asmx", "EliminarPropietarioObra", variables);
                    Mensaje = Convert.ToString(result);
                    if (result != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Eliminar Obra", Mensaje, "OK");

                        // Recargando La lista
                        //ObraItems.Clear();
                        //CargarObraItems();
                        // 
                        return;
                    }
                    //
                    // Evento Refrescar La Lista
                }
                catch (Exception ex)
                {
                    await mensaje.MostrarMensaje("Eliminar Obra", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
                }
                finally
                {
                }
                //App.Current.MainPage.DisplayAlert("Titulo", this.nombre + this.idObra, "Acpetar");

            });
        } 
        #endregion


    }
}
