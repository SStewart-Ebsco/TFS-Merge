using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace ER_BestPickReports
{
    public class MyCustomValidator : CustomValidator
    {
        protected override bool EvaluateIsValid()
        {
            // Get the value from ControlToValidate
            string val = this.GetControlValidationValue(this.ControlToValidate);

            // Call the page's event validation function even if there is
            // no value in ControlToValidate
            return this.OnServerValidate(val);
        }
    }
}