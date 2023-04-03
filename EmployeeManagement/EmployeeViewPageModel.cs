using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class EmployeeViewPageModel : INotifyPropertyChanged
    {
        private int _currentPage;
        private List<int> _pageNumbers;

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        public List<int> PageNumbers
        {
            get { return _pageNumbers; }
            set { _pageNumbers = value; OnPropertyChanged("PageNumbers"); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task RefreshPageNumbers()
        {
            var pageSize = 10; 
            var totalEmployees = await new EmployeeService().GetTotalEmployees();
            var totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);
            PageNumbers = Enumerable.Range(1, totalPages).ToList();
        }

    }

}
