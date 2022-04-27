using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Course currentCourse;
        public CoursePage(Course course)
        {
            InitializeComponent();
            currentCourse = course;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }
        protected override void OnAppearing()
        {
            courseName.Text = currentCourse.CourseName;
            Status.Text = currentCourse.Status;
            StartDate.Text = currentCourse.StartDate.ToString("MM/dd/yy");
            EndDate.Text = currentCourse.EndDate.ToString("MM/dd/yy");
            InstructorName.Text = currentCourse.InstructorName;
            InstructorPhone.Text = currentCourse.InstructorPhone;
            InstructorEmail.Text = currentCourse.InstructorEmail;
            Notes.Text = currentCourse.Notes;
            Notifications.Text = currentCourse.Notifications == 1 ? "ON" : "OFF";
            base.OnAppearing();


        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Edit_Course_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditCourse(currentCourse));
        }

        private async void Remove_Course_Click(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "This course and related assessments will be removed from the course tracker. Are you sure you want to remove this course?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(currentCourse);
                await Navigation.PopModalAsync();
            }
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void Assessments_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AssessmentsPage(currentCourse));
        }

        private async void ShareButton_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = Notes.Text,
                Title = "Share course notes!"
            });
        }
    }
}