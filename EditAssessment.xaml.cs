using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessment : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Assessment assessment;

        public EditAssessment(Assessment assessment)
        {
            InitializeComponent();
            this.assessment = assessment;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            await _conn.CreateTableAsync<Assessment>();

            AssessmentName.Text = assessment.Title;
            StartDate.Date = assessment.StartDate;
            EndDate.Date = assessment.EndDate;

            if (assessment.Notifications == 1)
            {
                Notifications.On = true;
            }
            base.OnAppearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            assessment.Title = AssessmentName.Text;
            assessment.StartDate = StartDate.Date;
            assessment.EndDate = EndDate.Date;
            assessment.Notifications = Notifications.On ? 1 : 0;

            if (FieldCheck.IsNull(AssessmentName.Text))
            {
                if (assessment.StartDate < assessment.EndDate)
                {
                    await _conn.UpdateAsync(assessment);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Start date must come before end date", "Ok");
            }
            else await DisplayAlert("Error.", "All fields should be populated.", "Ok");
        }


        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}