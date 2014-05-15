using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin.Types;

namespace Flasky
{

    //heres some notes from flask
    //If a response object of the correct type is returned it’s directly returned from the view.
    //If it’s a string, a response object is created with that data and the default parameters.
    //If a tuple is returned the items in the tuple can provide extra information. Such tuples have to be in the form (response, status, headers) or (response, headers) where at least one item has to be in the tuple. The status value will override the status code and headers can be a list or dictionary of additional header values.
    //If none of that works, Flask will assume the return value is a valid WSGI application and convert that into a response object.

    public interface IResponse
    {
        void Render(Owin.Types.OwinResponse response);
    }

    public class JsonResponse<T> : IResponse
    {
        public void Render(OwinResponse response)
        {
            throw new NotImplementedException();
        }
    }

    public interface IRenderer
    {
        void Render(Owin.Types.OwinResponse response, object model);
    }
}
