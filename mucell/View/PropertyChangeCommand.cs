using System;
using System.Collections.Generic;
using System.Text;

namespace MuCell.View
{
    class PropertyChangeCommand : ICommand
    {
        private Object obj;
        private String propertyName;
        private Object value;
        private Object oldValue;
        public PropertyChangeCommand(Object obj, String propertyName, Object value, Object oldValue)
        {
            this.obj = obj;
            this.propertyName = propertyName;
            this.value = value;
            this.oldValue = oldValue;
        }

        #region ICommand Members

        public void doAction()
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }

        public void undoAction()
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, oldValue, null);
        }

        #endregion
    }
}
