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
    public partial class AddAssessment : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Course course;

        public AddAssessment(Course course)
        {
            InitializeComponent();
            this.course = course;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {

            await _conn.CreateTableAsync<Assessment>();
            var objectiveSelect = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where Course = '{course.Id}' And Type = 'Objective'");
            var performanceSelect = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where Course = '{course.Id}' And Type = 'Performance'");
            if (objectiveSelect.Count == 0)
            {
                AssessmentType.Items.Add("Objective");
            }
            if (performanceSelect.Count == 0)
            {
                AssessmentType.Items.Add("Performance");
            }
            if (objectiveSelect.Count == 1)
            {
                AssessmentType.Items.Remove("Objective");
            }
            if (performanceSelect.Count == 1)
            {
                AssessmentType.Items.Remove("Performance");
            }
            base.OnAppearing();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void Add_Assessment_Clicked(object sender, EventArgs e)
        {
            var assessment = new Assessment();
            assessment.Title = AssessmentName.Text;
            assessment.StartDate = StartDate.Date;
            assessment.EndDate = EndDate.Date;
            assessment.Course = course.Id;
            assessment.Type = (string)AssessmentType.SelectedItem;

            if (FieldCheck.IsNull(AssessmentName.Text))
            {
                if (assessment.StartDate < assessment.EndDate)
                {
                    await _conn.InsertAsync(assessment);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Start date must come before end date", "Ok");
            }
            else await DisplayAlert("Error.", "All fields should be populated.", "Ok");

        }
    }
}