using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReportControlSystem
{
    /// <summary>
    /// Interaction logic for CategoryAddingDialog.xaml
    /// </summary>
    public partial class CategoryAddingDialog : Window
    {
        Window _parent;
        Boolean categoty_type;
        Category oldCategory;

        internal event EventHandler<ObjectPassedEventArgs> CategoryPassed;

        public CategoryAddingDialog()
        {
            InitializeComponent();
        }

        internal CategoryAddingDialog(Window form)
            : this()
        {
            _parent = form;
            btnSave.Click += BTN_Save_Clicked;
        }

        internal CategoryAddingDialog(Window form, Category c)
            : this(form)
        {
            oldCategory = c;
            btnSave.Click += BTN_Save_Edit_Clicked;
            LoadCategoryToDialog();
        }

        private void LoadCategoryToDialog()
        {
            txt_cagetory_name.Text = oldCategory.Category_Name;
            txt_cagetory_des.Text = oldCategory.Category_Description;
            //comCategoryType.SelectedIndex = oldCategory._category_Type_bit;
        }

        
        private void LoadTypeToBox(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            
            data.Add("Negative"); // 0
            data.Add("Positive"); // 1
            
            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            if (oldCategory == null)
            {
                // ... Make the first item selected.
                comboBox.SelectedIndex = 0;
            }
            else
            {
                comboBox.SelectedIndex = oldCategory._category_Type_bit;
            }
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;

            categoty_type = value.StartsWith("P") ? true : false;
        }

        private void Windows_Closed(object sender, EventArgs e)
        {
            _parent.Show();
            this.Close();
        }

        private void BTN_Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
            _parent.Show();
          //  this.Close();
        }

        private void BTN_Save_Clicked(object sender, RoutedEventArgs e)
        {
            String cat_Name = txt_cagetory_name.Text;
            String cat_Des = txt_cagetory_des.Text;

            Category c = new Category(cat_Name, categoty_type, cat_Des);

            ObjectPassedEventArgs newCat = new ObjectPassedEventArgs();
           

            newCat.item = c;

            if (CategoryPassed != null)
            {
                CategoryPassed(this, newCat);
            }
        }

        void BTN_Save_Edit_Clicked(object sender, RoutedEventArgs e)
        {
            oldCategory.Category_Name = txt_cagetory_name.Text;
            oldCategory.Category_Description = txt_cagetory_des.Text;
            oldCategory.Category_Type_bit = Convert.ToInt32(categoty_type);

            ObjectPassedEventArgs updatedCat = new ObjectPassedEventArgs();

            updatedCat.item = oldCategory;

            if (CategoryPassed != null)
            {
                CategoryPassed(this, updatedCat);
            }

        }
    }
}
