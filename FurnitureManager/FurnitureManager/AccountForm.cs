using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FurnitureManager {
    public partial class AccountForm : Form {
        private IAbstractForm abstractForm;

        public DataManagement.User User {
            set {
                idTextBox.Text = value.id.ToString();
                usernameTextBox.Text = value.username;
            }
        }

        public AccountForm(IAbstractForm form) {
            InitializeComponent();
            abstractForm = form;
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            abstractForm.setEnabled();
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e) {
            Controller.UserController uc = new Controller.UserController();
            try {
                uc.modifyAccount(currentPasswordTextBox.Text, int.Parse(idTextBox.Text),
                    usernameTextBox.Text, newPasswordTextBox.Text, repeatNewPasswordTextBox.Text);
                abstractForm.setNewUsername(usernameTextBox.Text);
                abstractForm.setEnabled();
                this.Close();
            } catch(Controller.Exceptions.InvalidPasswordException ex) {
                errorMessageLabel.Text = "The given password is not valid.";
            } catch(Controller.Exceptions.DifferentPasswordsException ex) {
                errorMessageLabel.Text = "The given new passwords are different.";
            } catch(Controller.Exceptions.InvalidUsernameException ex) {
                errorMessageLabel.Text = "The introduced username is invalid. " +
                    "The username must have at least 6 characters and must be unique.";
            }
        }
    }
}
