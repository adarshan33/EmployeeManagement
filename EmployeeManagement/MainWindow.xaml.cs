using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EmployeeViewPageModel _viewModel;
        private EmployeeService employeeService;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new EmployeeViewPageModel();
            DataContext = _viewModel;
            employeeService= new EmployeeService();
        }

        private async void GetDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> employees = await employeeService.GetAllEmployees();
            EmployeesGrid.ItemsSource = employees;
            await _viewModel.RefreshPageNumbers();

        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtSearch.Text, out int id))
            {
                   var result = await employeeService.GetEmployeeById(id);
                    if (result.email != null)
                    {
                    // Display employee details
                    List<Employee> employees = new List<Employee>() { result };
                    EmployeesGrid.ItemsSource = employees;
                    }
                    else
                    {
                        MessageBox.Show("Employee not found.");
                    }
            }
                else
                {
                    // Search by name
                    var empresult = await employeeService.GetEmployeesByName(txtSearch.Text);
                    if (empresult.Count > 0)
                    {
                    // Display employee details
                    EmployeesGrid.ItemsSource = empresult;
                    }
                    else
                    {
                        MessageBox.Show("No employees found.");
                    }
                }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var genderItem = (ComboBoxItem)cbGender.SelectedItem;
            var gender = genderItem.Content.ToString();
            var statusItem = (ComboBoxItem)cbStatus.SelectedItem;
            var status = statusItem.Content.ToString();
            var newEmployee = new Employee
            {
                name = txtName.Text,
                email = txtEmail.Text,
                gender = gender,
                status = status
            };

            try
            {
                var addedEmployee = await employeeService.CreateEmployee(newEmployee);
                MessageBox.Show($"Added employee");             
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Given Employee Details are Already added");
            }
        }

        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            string idToDelete = txtSearch.Text;
            bool isDeleted = await employeeService.DeleteEmployee(idToDelete);

            if (isDeleted)
            {
                MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                List<Employee> employees = await employeeService.GetAllEmployees();
                EmployeesGrid.ItemsSource = employees;
            }
            else
            {
                MessageBox.Show("Failed to delete employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage >= 1)
            {
                _viewModel.CurrentPage--;
            }
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentPage < _viewModel.PageNumbers.Count)
            {
                _viewModel.CurrentPage++;
            }
        }

        private async void PageDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageDropdown.SelectedIndex != -1)
            {
                int page = (int)pageDropdown.SelectedItem;
                try
                {
                    List<Employee> employees = await employeeService.GetEmployeesByPage(page);
                    EmployeesGrid.ItemsSource = employees;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to retrieve employees. Error message: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
