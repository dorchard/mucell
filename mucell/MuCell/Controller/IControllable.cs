using System;
using System.Collections.Generic;
using System.Text;


namespace MuCell.Controller
{

    /// <summary>
    /// Specifies that an object can be controller using a given controller.
    /// Typically this will be a user interface, such as a menu screen.
    /// </summary> 
    public interface IControllable<T>
    {

       void setController(T controller);
    }
}

