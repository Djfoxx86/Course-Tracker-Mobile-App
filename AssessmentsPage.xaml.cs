using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Course currentCourse;
        private ObservableCollection<Assessment> _assessmentList;
        public AssessmentsPage(Course currentCourse)
        {
            InitializeComponent();
            CourseName.Text = currentCourse.CourseName;
            this.currentCourse = currentCourse;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            assessmentListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Assessment_Tapped);

        }

        protected override async void OnAppearing()
        {
            CourseName.Text = currentCourse.CourseName;
            await _conn.CreateTableAsync<Assessment>();
            var assessmentList = await _conn.QueryAsync<Assessment>($"Select * From Assessments Where Course = '{currentCourse.Id}'");
            _assessmentList = new ObservableCollection<Assessment>(assessmentList);
            assessmentListView.ItemsSource = _assessmentList;
            base.OnAppearing();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Add_Assessment_Click(object sender, EventArgs e)
        {
            await _conn.CreateTableAsync<Assessment>();
            var assessmentCount = await _conn.QueryAsync<Assessment>($"Select Type From Assessments Where Course = '{currentCourse.Id}'");
            if (assessmentCount.Count == 2)
            {
                await DisplayAlert("Alert", "Up to two exams can be added per course.", "Ok");
            }
            else await Navigation.PushModalAsync(new AddAssessment(currentCourse));
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void Assessment_Tapped(object sender, ItemTappedEventArgs e)
        {
            Assessment assessment = (Assessment)e.Item;
            await Navigation.PushModalAsync(new AssessmentDetail(assessment));
        }
    }
}