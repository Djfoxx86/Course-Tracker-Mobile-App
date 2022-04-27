using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentDetail : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Assessment SelectedAssessment;
        public AssessmentDetail(Assessment assessment)
        {
            InitializeComponent();
            SelectedAssessment = assessment;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override void OnAppearing()
        {
            AssessmentName.Text = SelectedAssessment.Title;
            StartDate.Text = SelectedAssessment.StartDate.ToString("MM/dd/yy");
            EndDate.Text = SelectedAssessment.EndDate.ToString("MM/dd/yy");
            AssessmentType.Text = SelectedAssessment.Type;
            Notifications.Text = SelectedAssessment.Notifications == 1 ? "ON" : "OFF";
            base.OnAppearing();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Edit_Assessment_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditAssessment(SelectedAssessment));
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void Remove_Assessment_Click(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(SelectedAssessment);
                await Navigation.PopModalAsync();
            }
        }
    }
}