using System;
using System.Collections.Generic;

using Xamarin.Forms;
using customerapp.Dto;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace customerapp
{
	public partial class MissionListPage : ContentPage
	{
		ObservableCollection<Mission> missions = new ObservableCollection<Mission>();

		Order order;

		public MissionListPage (Order order)
		{
			InitializeComponent ();
			this.order = order;

			MissionListView.ItemsSource = missions;
			MissionListView.ItemSelected += MissionSelected;
		}

		protected async override void OnAppearing(){
			base.OnAppearing ();

			missions.Clear ();

			var missionsFromServer = await App.Rest.GetAllMissions (order.Id);
			foreach (Mission each in missionsFromServer) {
				missions.Add (each);
			}
		}

		async void MissionSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var selectedMission = e.SelectedItem as Mission;
			await Navigation.PushAsync(new MissionPage(selectedMission));
		}
	}
}

