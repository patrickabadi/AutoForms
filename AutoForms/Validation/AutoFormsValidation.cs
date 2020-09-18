using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForms
{
    public class AutoFormsValidation
    {
        private readonly Action _clearValidation;
        private readonly Func<bool> _checkValidation;

        public AutoFormsValidation(Action clearValidation, Func<bool> checkValidation)
        {
            _clearValidation = clearValidation;
            _checkValidation = checkValidation;
        }

        public void ClearValidation() => _clearValidation.Invoke();

        public bool CheckValidation() => _checkValidation.Invoke();
    }
}
