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
    public partial class EditCourse : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Course currentCourse;
        public EditCourse(Course currentCourse)
        {
            InitializeComponent();
            this.currentCourse = currentCourse;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            await _conn.CreateTableAsync<Course>();

            CourseName.Text = currentCourse.CourseName;
            CourseStatus.SelectedItem = currentCourse.Status;
            StartDate.Date = currentCourse.StartDate;
            EndDate.Date = currentCourse.EndDate;
            InstructorName.Text = currentCourse.InstructorName;
            InstructorPhone.Text = currentCourse.InstructorPhone;
            InstructorEmail.Text = currentCourse.InstructorEmail;
            Notes.Text = currentCourse.Notes;
            if (currentCourse.Notifications == 1)
            {
                Notifications.On = true;
            }
            base.OnAppearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            currentCourse.CourseName = CourseName.Text;
            currentCourse.Status = (string)CourseStatus.SelectedItem;
            currentCourse.StartDate = StartDate.Date;
            currentCourse.EndDate = EndDate.Date;
            currentCourse.InstructorName = InstructorName.Text;
            currentCourse.InstructorEmail = InstructorEmail.Text;
            currentCourse.InstructorPhone = InstructorPhone.Text;
            currentCourse.Notes = Notes.Text;
            currentCourse.Notifications = Notifications.On ? 1 : 0;

            if (FieldCheck.IsNull(CourseName.Text) &&
                FieldCheck.IsNull(InstructorName.Text) &&
                FieldCheck.IsNull(InstructorPhone.Text))
            {
                if (FieldCheck.IsValidEmail(InstructorEmail.Text))
                {
                    if (currentCourse.StartDate < currentCourse.EndDate)
                    {
                        await _conn.UpdateAsync(currentCourse);
                        await Navigation.PopModalAsync();
                    }
                    else await DisplayAlert("Error.", "Start date must come before end date", "Ok");
                }
                else await DisplayAlert("Error.", "All fields should be populated.", "Ok");
            }
            else await DisplayAlert("Error.", "Please provide a valid email address", "Ok");
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