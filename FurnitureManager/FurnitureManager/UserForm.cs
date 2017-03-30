using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataManagement;
using Controller;

namespace FurnitureManager {
    public partial class UserForm : Form {
        public enum OperationEnum { Add, Modify };
        private OperationEnum operation;
        private AdminForm adminForm;
        
        public OperationEnum Operation {
            get {
                return operation;
            }

            set {
                operation = value;
                if (operation == OperationEnum.Add)
                    operationButton.Text = "Add new user";
                else if (operation == OperationEnum.Modify) {
                    operationButton.Text = "Modify new User";
                    passwordTextBox.Enabled = false;
                    repeatPasswordTextBox.Enabled = false;
                }
            }
        }

        public User User {
            set {
                idTextBox.Text = value.id.ToString();
                usernameTextBox.Text = value.username;
                adminCheckBox.Checked = value.isAdmin;
            }
        }
            
        public UserForm(AdminForm af) {
            InitializeComponent();
            operation = OperationEnum.Add;
            adminForm = af;
        }

        public void setDataFields(User usr) {
            if (operation == OperationEnum.Modify) {
                usernameTextBox.Text = usr.username;
                adminCheckBox.Checked = usr.isAdmin;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            adminForm.Enabled = true;
            this.Hide();
        }

        private void showErrorMessage(string text) {
            //MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorMessageLabel.Text = text;
        }

        private void operationButton_Click(object sender, EventArgs e) {
            UserController uc = new UserController();
            if (operation == OperationEnum.Add) {                
                try {
                    string username = usernameTextBox.Text;
                    string password1 = passwordTextBox.Text;
                    string password2 = repeatPasswordTextBox.Text;
                    bool admin = adminCheckBox.Checked;
                    uc.addNewUser(username, password1, password2, admin);
                    adminForm.reloadUsersTable();
                    adminForm.Enabled = true;
                    this.Hide();
                } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connecting to the database.");
                } catch(Controller.Exceptions.DifferentPasswordsException ex) {
                    showErrorMessage("You must type the same password.");
                } catch(Controller.Exceptions.InvalidUsernameException ex) {
                    showErrorMessage("The username must have at least 6 characters and " + 
                        "must be not used by anothers.");
                }
            } else {
                try {
                    int id = System.Int32.Parse(idTextBox.Text);
                    string username = usernameTextBox.Text;
                    bool admin = adminCheckBox.Checked;
                    uc.modifyUser(id, username, admin);
                    adminForm.reloadUsersTable();
                    adminForm.Enabled = true;
                    this.Hide();
                } catch(DataManagement.Exceptions.MySqlConnectionException ex) {
                    showErrorMessage("Error when connectiong to the database.");
                } catch(Controller.Exceptions.InvalidUsernameException ex) {
                    showErrorMessage("The username must 6 at least 6 characters and " +
                        "must be not used by anothers.");
                }
            }            
        }
    }
}
