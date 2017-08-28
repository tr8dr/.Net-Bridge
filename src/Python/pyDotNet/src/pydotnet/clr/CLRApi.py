#
# General:
#      This file is pary of .NET Bridge
#
# Copyright:
#      2010 Jonathan Shore
#      2017 Jonathan Shore and Contributors
#
# License:
#      Licensed under the Apache License, Version 2.0 (the "License");
#      you may not use this file except in compliance with the License.
#      You may obtain a copy of the License at:
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
#      Unless required by applicable law or agreed to in writing, software
#      distributed under the License is distributed on an "AS IS" BASIS,
#      WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#      See the License for the specific language governing permissions and
#      limitations under the License.
#


from pydotnet.core.io import *
from pydotnet.clr.ctrl import *
from pydotnet.clr.CLRMessage import CLRMessage
from pydotnet.core.utils import SystemUtils

from types import MethodType
import unittest
import numpy as np
import pandas as pd
import subprocess
import socket



class CLRApi:
    """
    CLR Bridge Api: This interface gives access to the .NET CLR, allowing one to create
    objects, call methods, get / set properties, etc.

    Typical usage is to create an object with api.new (classname, ...) and then interact
    with the returned object proxy.
    """

    Classes = {}
    Api = None

    def __init__(self, hostname="", port=56789, dll=None, server_args=[]):
        """
        Initialize CLR API

        :param hostname: the name of the host to connect to (defaults to localhost)
        :param port: the port # to connect to (defaults to 56789)
        :param dll: an optional .NET dll to load (defaults to None)
        :param server_args: an optional list of CLRServer arguments         
        """
        self.hostname = hostname
        self.port = port
        self.cin = None
        self.cout = None
        self.dll = dll
        self.server_args = server_args
        self.connect()
        Api = self


    def connect(self):
        """
        Connect to CLR
        """
        if self.cin is not None:
            return

        def newServer ():
            server = "%s/server/CLRServer.exe" % SystemUtils.package_path()
            hostname = "localhost" if self.hostname == "" else self.hostname
            url = "svc://%s:%d" % (hostname, self.port)

            serverargs = self.server_args
            if self.dll:
                serverargs = serverargs + ["-dll", self.dll]

            if SystemUtils.is_unix():
                self.pid = subprocess.Popen(["mono", "--llvm", server] + self.server_args).pid
            else:
                self.pid = subprocess.Popen([server] + self.server_args).pid


        client = SocketUtils.connect(self.hostname, self.port, startup=newServer)

        self.cin = BinarySocketReader(client)
        self.cout = BinarySocketWriter(client)
        self.cin.api = self
        self.cout.api = self

        
    def close(self):
        """
        Close the connection
        """
        self.cout.close()
        self.cin.close()
        self.cin = None
        self.cout = None


    def new(self, classname: str, *args):
        """
        create a new CLR object

        :param classname: fully qualified or partially qualified class name (i.e. 'com.stg.models.MyClass' or 'MyClass')
        :param *args: optional arguments to the constructor
        """
        ## create object request
        req = CLRCreateObject (classname, args)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        return rep.toValue()


    def callstatic(self, classname: str, methodname: str, *args):
        """
        call class method on CLR-side object

        :param classname: fully qualified or partially qualified class name (i.e. 'com.stg.models.MyClass' or 'MyClass')
        :param methodname: name of method / function to call
        :param *args: arguments to method / function
        """
        ## create object request
        req = CLRCallStaticMethod (classname, methodname, args)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        return rep.toValue()


    def call(self, objectId: int, methodname: str, *args):
        """
        call method on CLR-side object (this is only used internally)

        :param objectId: object ID of previously created object
        :param methodname: name of method / function to call
        :param *args: arguments to method / function        
        """
        ## create object request
        req = CLRCallMethod (objectId, methodname, args)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        return rep.toValue()


    def ctor(self, ctorexpr: str):
        """
        create class from ctor expression (i.e.  "ZTime(12,30,22,100)")

        :param ctorexpr: expression representing class contructor to be called (for example 'ZTime(12,30,22,100)')
        """
        return self.callstatic ("com.pydotnet.common.reflection.Creator", "NewByCtor", ctorexpr)


    def getProperty(self, objectId: int, property: str):
        """
        get property on CLR-side object (this is only used internally)

        :param objectId: object ID of previously created object
        :param property: name of property to get
        """
        ## create object request
        req = CLRGetProperty (objectId, property)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        return rep.toValue()


    def setProperty(self, objectId: int, property: str, value):
        """
        set property on CLR-side object (this is only used internally)

        :param objectId: object ID of previously created object
        :param property: name of property to set
        :param value: value of property to be set
        """
        ## create object request
        req = CLRSetProperty (objectId, property, value)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        rep.toValue()


    def getIndexed(self, objectId: int, idx: int):
        """
        get indexed on CLR-side object (this is only used internally)

        :param objectId: object ID of previously created object
        :param idx: index to retrieve
        """
        ## create object request
        req = CLRGetIndexed (objectId, idx)
        CLRMessage.write (self.cout, req)

        ## process result
        rep = CLRMessage.read (self.cin)
        return rep.toValue()


    def protect (self, objectId: int):
        """
        protect an object from GC (this is only used internally)
        """
        pass


    def release (self, objectId: int):
        """
        release an object for GC (this is only used internally)
        """
        req = CLRRelease (objectId)
        CLRMessage.write (self.cout, req)



    def classFor (self, classname: str):
        """
        get class template for named type (this is only used internally)
        """
        klass = CLRApi.Classes.get (classname, None)
        if klass is not None:
            return klass
        else:
            ## create request
            req = CLRTemplateReq (classname)
            CLRMessage.write (self.cout, req)

            ## process result
            rep = CLRMessage.read (self.cin)
            info = rep.toValue()
            klass = CLRObject.proxyClassFor (classname, info)
            CLRApi.Classes[classname] = klass
            return klass


    def objectFor (self, objectId: int, classname: str):
        """
        get object proxy for CLR obj (this is only used internally)
        """
        klass = self.classFor(classname)
        obj = klass (objectId, classname, self)
        return obj


    @staticmethod
    def get():
        """
        Get singleton instance of API
        """
        if CLRApi.Api is not None:
            return CLRApi.Api
        else:
            CLRApi.Api = CLRApi()
            return CLRApi.Api





class CLRObject:
    """
    Proxy for CLR objects
    """

    _classnames = {}
    _internal = set(["objectId", "classname", "api", "cache"])

    ##
    ##  Inner Classes
    ##

    class CallMethod:
        """
        Instance Method Call Stub
        """
        def __init__(self, obj, method: str, api: CLRApi = None):
            self.obj = obj
            self.method = method
            self.api = api if api is not None else CLRApi.get()

        def __call__ (self, *args):
            return self.api.call (self.obj.objectId, self.method, *args)


    class CallStaticMethod:
        """
        Static Method Call Stub
        """
        def __init__(self, classname: str, method: str, api: CLRApi = None):
            self.classname = classname
            self.method = method
            self.api = api if api is not None else CLRApi.get()

        def __call__ (self, *args):
            return self.api.call (self.classname, self.method, *args)


    ##
    ##  Ctor
    ##


    def __init__(self, objectId: int = 0, classname="", api=None):
        self.objectId = objectId
        self.classname = classname
        self.api = api if api is not None else CLRApi.get()
        self.cache = {}


    ##
    ##  Meta
    ##

    @property
    def ClassName(self):
        """
        name of class without namespace
        """
        return self.classname.split('.')[-1]


    def addMethod (self, methodname: str, fun):
        """
        Add a method to this proxy object, defined outside
        """
        self.__dict__[methodname] = MethodType(fun, self)


    def __getitem__(self, idx: int):
        """
        Get ith element
        """
        return self.api.getIndexed (self.objectId, idx)


    def __del__(self):
        """
        Delete object, releasing for GC
        """
        self.api.release (self.objectId)


    def __str__(self):
        """
        String representation of object
        """
        return "<%s: %d>" % (self.classname, self.objectId)


    def __getattribute__(self, name):
        """
        Internal resolution of methods & properties
        """
        try:
            return super().__getattribute__(name)
        except AttributeError:
            pass

        klass = self.__class__

        ## see if precached
        functor = self.cache.get (name, None)
        if functor is not None:
            return functor

        ## see if is a method
        if name in klass._methods:
            functor = CLRObject.CallMethod (self, name, self.api)
            self.cache[name] = functor
            return functor

        ## see if is a property
        if name in klass._properties:
            return self.api.getProperty (self.objectId, name)
        else:
            raise AttributeError ("attempted to get or call unknown method or property: %s" % name)


    def __setattr__(self, name, value):
        """
        Internal resolution of set property
        """
        klass = self.__class__
        ## see if is a property
        if name in klass._properties:
            return self.api.setProperty (self.objectId, name, value)
        else:
            super().__setattr__(name, value)


    @staticmethod
    def proxyClassFor (classname: str, info: dict):
        """
        Create  new type for given class
        """
        proxyname = classname.split(".")[-1]
        cnt = CLRObject._classnames.get(proxyname, 0)
        if cnt > 0:
            CLRObject._classnames[proxyname] = cnt+1
            proxyname = "%s%d" % (proxyname, cnt+1)
        else:
            CLRObject._classnames[proxyname] = 1

        def _init_ (self, objectId, classname, api):
            CLRObject.__init__ (self, objectId, classname, api)

        ## basic class
        properties = info["properties"]
        methods = info["methods"]
        classmethods = info["classmethods"]

        dict = {
            "__init__": _init_,
            "_properties": set(properties),
            "_methods": set(methods) }

        ## add static functions
        for method in classmethods:
            functor = CLRObject.CallStaticMethod (classname, method)
            dict[method] = functor

        newclass = type(proxyname, (CLRObject,),dict)
        return newclass





##
##  UNIT TESTS
##

class TestCLRMessage(unittest.TestCase):


    def test_create(self):
        import pydotnet.clr.CLRInitialization

        ## ints
        o1 = CLRMessage.createByObj (2)
        self.assertTrue(isinstance(o1, pydotnet.clr.data.CLRInt32))

        ## floats
        o2 = CLRMessage.createByObj (3.14)
        self.assertTrue(isinstance(o2, pydotnet.clr.data.CLRFloat64))

        ## boolean
        b1 = CLRMessage.createByObj (True)
        self.assertTrue(isinstance(b1, pydotnet.clr.data.CLRBool))

        ## string
        s1 = CLRMessage.createByObj ("this is a test")
        self.assertTrue(isinstance(s1, pydotnet.clr.data.CLRString))

        ## vectors
        o2l = CLRMessage.createByObj ([3.14, 1.34])
        self.assertTrue(isinstance(o2l, pydotnet.clr.data.CLRVector))
        o3l = CLRMessage.createByObj (np.array([3.14, 1.34]))
        self.assertTrue(isinstance(o3l, pydotnet.clr.data.CLRVector))
        o4l = CLRMessage.createByObj (np.array([[3.14], [1.34]]))
        self.assertTrue(isinstance(o4l, pydotnet.clr.data.CLRVector))
        o5l = CLRMessage.createByObj (pd.DataFrame([3.14, 1.34], index=['a','b']))
        self.assertTrue(isinstance(o5l, pydotnet.clr.data.CLRVector))
        o6l = CLRMessage.createByObj (pd.DataFrame([3.14, 1.34]).as_matrix())
        self.assertTrue(isinstance(o6l, pydotnet.clr.data.CLRVector))

        ## matrices
        m1 = CLRMessage.createByObj (pd.DataFrame([[2.0, 0,0], [0,3.0,0], [0,0,23.0]], index=['a','b','c'], columns=['a','b','c']))
        self.assertTrue(isinstance(m1, pydotnet.clr.data.CLRMatrix))

        ## arrays
        a1 = CLRMessage.createByObj (np.array([1,3,5,7], dtype=np.int32))
        self.assertTrue(isinstance(a1, pydotnet.clr.data.CLRInt32Array))
        a2 = CLRMessage.createByObj (np.array([1,3,5,7], dtype=np.int64))
        self.assertTrue(isinstance(a2, pydotnet.clr.data.CLRInt64Array))
        a3 = CLRMessage.createByObj (["this", "that"])
        self.assertTrue(isinstance(a3, pydotnet.clr.data.CLRStringArray))
        a4 = CLRMessage.createByObj (pd.DataFrame([3.14, 1.34], index=['a','b']).index.values)
        self.assertTrue(isinstance(a4, pydotnet.clr.data.CLRStringArray))


    def test_api1(self):
        ## ints
        import pydotnet.clr.CLRInitialization
        clr = CLRApi()
        obj = clr.new ("DateTime", 2014,1,1)
        self.assertTrue(obj.Year == 2014)


if __name__ == '__main__':
    unittest.main()

