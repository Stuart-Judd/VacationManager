using System.Collections.Generic;
using Caliburn.Micro;
using Ninject;
using VacationManager.Common.Model;
using VacationManager.Ui.Resources;
using VacationManager.Ui.Services;
using Vm.BusinessObjects.Employees;

namespace VacationManager.Ui.Components.Context
{
    public class ContextViewModel : Screen, IContextViewModel
    {
        #region Private fields

        private string _rolesMessage;
        private string _welcomeMessage;
        private Employee _currentEmployee;

        #endregion

        public static ContextStrings Localization
        {
            get
            {
                return new ContextStrings();
            }
        }

        #region External dependencies

        [Inject]
        public IUiService UiService { get; set; }

        [Inject]
        public IDataService DataService { get; set; }

        #endregion

        #region Binding properties

        public string RolesMessage
        {
            get { return _rolesMessage; } 
            set 
            { 
                _rolesMessage = value;
                NotifyOfPropertyChange(() => RolesMessage);
            }
        }

        public string WelcomeMessage
        {
            get { return _welcomeMessage; } 
            set 
            { 
                _welcomeMessage = value;
                NotifyOfPropertyChange(() => WelcomeMessage);
            }
        }

        #endregion

        public Employee CurrentEmployee
        {
            get { return _currentEmployee; } 
            set
            {
                _currentEmployee = value;
                if (_currentEmployee != null)
                {
                    RolesMessage = string.Format(ContextStrings.RolesMessageFormat, GetRoles());
                    WelcomeMessage = string.Format(ContextStrings.WelcomeMessageFormat, _currentEmployee.Firstname, _currentEmployee.Surname);
                }
            }
        }

        public bool IsExecutive
        {
            get { return _currentEmployee.IsIn(EmployeeRoles.Executive); }
        }

        public bool IsManager
        {
            get { return _currentEmployee.IsIn(EmployeeRoles.Manager); }
        }
        
        public bool IsHr
        {
            get { return _currentEmployee.IsIn(EmployeeRoles.Hr); }
        }

        public IEnumerable<IResult> Populate()
        {
            yield return UiService.ShowBusy();

            // TODO: in future server should return employee determined by checking caller identity.
            // for the moment, in order to be able to test more easily we are specifying the id of 
            // the employee we consider to be the caller.
            var result = DataService.Fetch<Employee>(Configuration.CurrentEmployeeId);
            yield return result;

            yield return UiService.HideBusy();

            if (result.Error == null)
                CurrentEmployee = result.Result;
            else
                yield return UiService.ShowMessageBox(result.Error.Message, GlobalStrings.ErrorCaption);
        }

        private string GetRoles()
        {
            var message = string.Empty;

            if (IsExecutive)
                message += " " + ContextStrings.ExecutiveRole;
            if (IsManager)
                message += " " + ContextStrings.ManagerRole;
            if (IsHr)
                message += " " + ContextStrings.HrRole;

            return message;
        }
    }
}