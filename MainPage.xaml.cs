using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.LocalNotifications;
using System.Runtime.InteropServices;

namespace C971_Mobile_App
{
    [Table("Terms")]
    public class Term
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    [Table("Courses")]
    public class Course
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int Term { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; }
        public int Notifications { get; set; }
    }
    [Table("Assessments")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public int Course { get; set; }
        public int Notifications { get; set; }
    }
    public partial class MainPage : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        public ObservableCollection<Term> termList;
        private bool pushNotification = true;
        public MainPage()
        {
            InitializeComponent();
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            termListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Term_Click);
        }
        protected override async void OnAppearing()
        {
            await _conn.CreateTableAsync<Term>();
            await _conn.CreateTableAsync<Course>();
            await _conn.CreateTableAsync<Assessment>();

            var termList = await _conn.Table<Term>().ToListAsync();
            var courseList = await _conn.Table<Course>().ToListAsync();
            var assessmentList = await _conn.Table<Assessment>().ToListAsync();
    
            //notification handling
            if (pushNotification)
            {
                pushNotification = false;
                int courseId = 0;
                int assessmentId = courseId;
                foreach (Course course in courseList)
                {
                    courseId++;
                    if (course.Notifications == 1)
                    {
                        if (course.StartDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{course.CourseName} begins today!", courseId);
                        if (course.EndDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{course.CourseName} ends today!", courseId);
                    }
                }

                foreach (Assessment assessment in assessmentList)
                {
                    assessmentId++;
                    if (assessment.Notifications == 1)
                    {
                        if (assessment.StartDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} begins today!", assessmentId);
                        if (assessment.EndDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} ends today!", assessmentId);
                    }
                }
            }
            this.termList = new ObservableCollection<Term>(termList);
            termListView.ItemsSource = this.termList;
            base.OnAppearing();
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new AddTerm(this));
        }
        async private void Term_Click(object sender, ItemTappedEventArgs e)
        {
            Term term = (Term)e.Item;
            await Navigation.PushModalAsync(new TermPage(term));
        }
    }
}
