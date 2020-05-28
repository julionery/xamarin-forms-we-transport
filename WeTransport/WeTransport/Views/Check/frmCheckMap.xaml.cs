using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeTransport.Components;
using WeTransport.Helpers;
using WeTransport.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace WeTransport.Views.Check
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmCheckMap : Rg.Plugins.Popup.Pages.PopupPage
    {
        #region + Variáveis Globais

        GPS compGPS;
        PedidoModel Registro;
        CheckModal checkRegistro;
        public Command AddInicialLocationCommand { get; set; }
        public event EventHandler<bool> ItemAtualizado;

        #endregion

        #region + Construtores

        public frmCheckMap(PedidoModel _pedido)
        {
            InitializeComponent();

            if (Settings.isService)
                btnSalvar.IsVisible = true;
            else
                btnSalvar.IsVisible = false;

            ToastProgress.Show();
            compGPS = new GPS();
            Registro = _pedido;
            checkRegistro = CheckHelper.ShowCheckInByPedido(_pedido.ID);

            if (checkRegistro == null)
                lblError.IsVisible = true;
            else
                lblError.IsVisible = false;

            InitializeMap();

            AddInicialLocationCommand = new Command(async () => await AddInicialLocation());
        }

        private void InitializeMap()
        {
            map.MapType = MapType.Street;
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;

            map.MyLocationButtonClicked += async (sender, e) =>
            {
                if (compGPS.IsLocationAvailable())
                {
                    await AddPinMap();
                }
            };
        }

        #endregion

        #region + Métodos

        private async Task AddPinMap()
        {
            if (checkRegistro != null)
            {
                Plugin.Geolocator.Abstractions.Position pos = await compGPS.GetCurrentLocation();

                if (pos != null && pos.Latitude != 0 && pos.Longitude != 0)
                {
                    Position _position = new Position(pos.Latitude, pos.Longitude);

                    var MyPos = new Pin()
                    {
                        Position = _position,
                        Label = "MINHA POSIÇÃO"
                    };
                    map.Pins.Clear();
                    map.Pins.Add(MyPos);
                    map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(_position, 15D, 0d, 0d));
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(MyPos.Position, Distance.FromMeters(2000)), true);
                }
                else
                {
                    map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(new Position(-17.7920769, -50.9238804), 13D, 0d, 0d));
                }
            }

        }

        private void BuscarLocalizacao()
        {
            try
            {
                if (checkRegistro != null)
                {
                    ToastProgress.Show();

                    var posMarcada = new Pin()
                    {
                        Position = new Position(checkRegistro.LATITUDE, checkRegistro.LONGITUDE),
                        Label = string.Format("#{0}", Registro.COD_FRETE.ToString().Split('-')[0].ToUpper()),
                        Address = "POSIÇÃO MARCADA"
                    };
                    map.Pins.Clear();
                    map.Pins.Add(posMarcada);
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(posMarcada.Position, Distance.FromMeters(500)), true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }
        }

        private async Task AddInicialLocation()
        {
            try
            {
                if (checkRegistro != null && checkRegistro.LATITUDE != 0 && checkRegistro.LONGITUDE != 0)
                {
                    Position _position = new Position(checkRegistro.LATITUDE, checkRegistro.LONGITUDE);

                    var posMarcada = new Pin()
                    {
                        Position = _position,
                        Label = string.Format("#{0}", Registro.COD_FRETE.ToString().Split('-')[0].ToUpper()),
                        Address = "POSIÇÃO MARCADA"
                    };
                    map.Pins.Clear();
                    map.Pins.Add(posMarcada);
                    map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(_position, 15D, 0d, 0d));
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(posMarcada.Position, Distance.FromMeters(2000)), true);
                }
                else
                {
                    Plugin.Geolocator.Abstractions.Position pos = await compGPS.GetCurrentLocation();
                    if (pos != null && pos.Latitude != 0 && pos.Longitude != 0)
                    {
                        Position _position = new Position(pos.Latitude, pos.Longitude);
                        map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(_position, 15D, 0d, 0d));
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(_position, Distance.FromMeters(2000)), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }
        }

        #endregion

        #region + Eventos

        private async void btnAtualizar_Clicked(object sender, EventArgs e)
        {
            try
            {
                ToastProgress.Show();
                await AddInicialLocation();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }
        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool result = await DisplayAlert("SALVAR", "Deseja realizar o CHECK-IN?", "SIM", "NÃO");

                if (result)
                {
                    ToastProgress.Show();

                    CheckModal checkin = new CheckModal();
                    Plugin.Geolocator.Abstractions.Position pos = await compGPS.GetCurrentLocation();

                    if(pos == null || pos.Latitude == 0 || pos.Longitude == 0)
                    {
                        UserDialogs.Instance.Toast("Não foi possível encontrar sua localização!", TimeSpan.FromSeconds(3));
                        return;
                    }

                    checkin.COD_PEDIDO = Registro.ID;
                    checkin.LATITUDE = pos.Latitude;
                    checkin.LONGITUDE = pos.Longitude;

                    await CheckHelper.AddCheckIn(checkin);
                    UserDialogs.Instance.Toast("Check-in realizado com Sucesso!", TimeSpan.FromSeconds(3));
                    await Navigation.PopPopupAsync();

                    ItemAtualizado?.Invoke(this, true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ToastProgress.Hide();
            }
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PopPopupAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region + Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AddInicialLocationCommand.Execute(null);
        }

        #endregion
    }
}