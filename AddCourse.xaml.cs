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
    public partial class AddCourse : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Term _term;
        public AddCourse(Term term)
        {
            InitializeComponent();
            _term = term;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

            private async void Button_Clicked(object sender, EventArgs e)
        {
            var course = new Course();
            course.CourseName = CourseName.Text;
            course.StartDate = StartDate.Date;
            course.EndDate = EndDate.Date;
            course.Status = (string)CourseStatus.SelectedItem;
            course.InstructorName = InstructorName.Text;
            course.InstructorPhone = InstructorPhone.Text;
            course.InstructorEmail = InstructorEmail.Text;
            course.Notifications = Notifications.On ? 1 : 0;
            course.Notes = Notes.Text;
            course.Term = _term.Id;

            if (FieldCheck.IsNull(CourseName.Text) &&
                FieldCheck.IsNull(InstructorName.Text) &&
                FieldCheck.IsNull(InstructorPhone.Text))
            {
                if (FieldCheck.IsValidEmail(InstructorEmail.Text))
                {
                    if (course.StartDate < course.EndDate)
                    {

                        await _conn.InsertAsync(course);

                        await Navigation.PopModalAsync();
                    }
                    else await DisplayAlert("Error.", "Start date must come before end date", "Ok");
                }
                else await DisplayAlert("Error.", "All fields should be populated.", "Ok");
            }
            else await DisplayAlert("Error.", "Please provide a valid email address", "Ok");
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}