using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation
{
    class PropertyValidationState: IDisposable
    {
        String actual = null;

        public IDisposable BeginPropertyValidation( String propertyName ) 
        {
            Ensure.That( actual ).Is( null );

            this.actual = propertyName;

            return this;
        }

        public Boolean IsValidatingProperty( String propertyName ) 
        {
            return this.actual == propertyName;
        }

        public void Dispose()
        {
            this.actual = null;
        }
    }
}
